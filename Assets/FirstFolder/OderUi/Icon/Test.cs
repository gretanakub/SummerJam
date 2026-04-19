using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
    

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

        // 1.Citrus Sunrise Blend
        // 2.Berry Tropical Rush
        // 3.Golden Pine Citrus
        // 4.Melon Sweet Splash
        // 5.Berry Citrus Punch
        // 6.Tropical Wave Juice
        // 7.Sunset Berry Lemon
        // 8.Orange Melon Chill

        orderManager.SpawnOrder("Berry Citrus Punch");          // 45 วิ
        orderManager.SpawnOrder("Orange Melon Chill", 30f);     // 30 วิ
        orderManager.SpawnOrder("Orange Melon Chill");                 // 45 วิ
        timeManager.StartTimer(10);
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            scoreManager.IncreaseScore(100);
        }
    }
}
