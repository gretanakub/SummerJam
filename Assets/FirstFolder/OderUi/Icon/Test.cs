using UnityEngine;

public class Test : MonoBehaviour
{
    private OrderManager orderManager;

    void Start()
    {
        // 1.Citrus Sunrise Blend
        // 2.Berry Tropical Rush
        // 3.Golden Pine Citrus
        // 4.Melon Sweet Splash
        // 5.Berry Citrus Punch
        // 6.Tropical Wave Juice
        // 7.Sunset Berry Lemon
        // 8.Orange Melon Chill

        orderManager = FindObjectOfType<OrderManager>();
        orderManager.SpawnOrder("Berry Citrus Punch");          // 45 วิ
        orderManager.SpawnOrder("Berry Citrus Punch", 30f);     // 30 วิ
        orderManager.SpawnOrder("Orange Melon Chill");                 // 45 วิ

        orderManager.CompleteOrder("Berry Citrus Punch");       // ลบออก (ทำสำเร็จ)
    }
}
