using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    private OrderManager orderManager;
    private TimeManager timeManager;
    private ScoreManager scoreManager;

    void Start()
    {
        orderManager = FindObjectOfType<OrderManager>();
        timeManager = FindObjectOfType<TimeManager>();
        scoreManager = FindObjectOfType<ScoreManager>();

        // เริ่ม timer เกม
        timeManager.StartTimer(360); // 2 นาที

        // เริ่มสุ่ม order
        orderManager.StartOrdering();

        // หยุดสุ่มเมื่อหมดเวลา
        timeManager.OnTimerFinished += () =>
        {
            orderManager.StopOrdering();
            Debug.Log("หมดเวลา! คะแนนสุดท้าย = " + scoreManager.GetScore());
        };
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            scoreManager.IncreaseScore(100);
        }
    }
}