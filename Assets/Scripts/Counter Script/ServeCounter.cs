using UnityEngine;
using System.Linq;

public class ServeCounter : MonoBehaviour, ICounter
{
    [SerializeField] private KitchenObjectSO[] serveableMenuSOArray;

    private OrderManager orderManager;

    private void Start()
    {
        orderManager = OrderManager.Instance;
    }

    public void Interact(PlayerController player)
    {
        if (!player.HasKitchenObject())
        {
            Debug.Log("ต้องถือเมนูก่อนถึงจะเสิร์ฟได้");
            return;
        }

        KitchenObjectSO heldSO = player.GetKitchenObject().GetKitchenObjectSO();

        bool isServeable = serveableMenuSOArray.Any(s => s == heldSO);

        if (!isServeable)
        {
            Debug.Log("เมนูนี้เสิร์ฟไม่ได้");
            return;
        }

        bool success = orderManager.TryServeOrder(heldSO.objectName);

        if (success)
            player.GetKitchenObject().DestroySelf();
    }
}