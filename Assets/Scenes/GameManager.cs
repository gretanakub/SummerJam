using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string hudSceneName = "HUD";
    [SerializeField] private float gameDuration = 360f; // เวลาเกมทั้งหมด

    public static GameManager Instance { get; private set; }

    private TimeManager timeManager;
    private OrderManager orderManager;
    private ScoreManager scoreManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // โหลด HUD Scene แบบ Additive
        if (!SceneManager.GetSceneByName(hudSceneName).isLoaded)
            SceneManager.LoadScene(hudSceneName, LoadSceneMode.Additive);

        // รอให้ HUD โหลดเสร็จก่อนค่อยเริ่มเกม
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == hudSceneName)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            StartGame();
        }
    }

    private void StartGame()
    {
        // หา Manager ทั้งหมดในทุกซีน
        timeManager = FindFirstObjectByType<TimeManager>();
        orderManager = FindFirstObjectByType<OrderManager>();
        scoreManager = FindFirstObjectByType<ScoreManager>();

        if (timeManager == null) Debug.LogError("ไม่พบ TimeManager!");
        if (orderManager == null) Debug.LogError("ไม่พบ OrderManager!");
        if (scoreManager == null) Debug.LogError("ไม่พบ ScoreManager!");

        // เริ่ม Timer
        timeManager?.StartTimer(gameDuration);

        // เริ่มสุ่ม Order
        orderManager?.StartOrdering();

        // หยุดเมื่อหมดเวลา
        if (timeManager != null)
            timeManager.OnTimerFinished += OnGameFinished;

        Debug.Log("เกมเริ่มแล้ว!");
    }

    private void OnGameFinished()
    {
        orderManager?.StopOrdering();
        Debug.Log("หมดเวลา! คะแนนสุดท้าย = " + scoreManager?.GetScore());

        // โหลดซีนจบเกม
        // SceneManager.LoadScene("Finish");
    }

    private void OnDestroy()
    {
        if (timeManager != null)
            timeManager.OnTimerFinished -= OnGameFinished;
    }
}