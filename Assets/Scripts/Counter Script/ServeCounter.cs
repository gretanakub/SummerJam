using UnityEngine;
using System.Linq;

public class ServeCounter : MonoBehaviour, ICounter
{
    [SerializeField] private KitchenObjectSO[] serveableMenuSOArray;
    [SerializeField] private OrderManager orderManager; // ลากใส่ตรงนี้เลย

    public void Interact(PlayerController player)
    {
        if (!player.HasKitchenObject())
        {
            Debug.Log("ต้องถือเมนูก่อนถึงจะเสิร์ฟได้");
            return;
        }

        if (orderManager == null)
        {
            Debug.LogError("ไม่ได้ assign OrderManager ใน Inspector!");
            return;
        }

        KitchenObjectSO heldSO = player.GetKitchenObject().GetKitchenObjectSO();
        Debug.Log("ของที่ถืออยู่ = " + heldSO.objectName);

        foreach (var s in serveableMenuSOArray)
            Debug.Log("เช็ค " + heldSO.objectName + " กับ " + s?.objectName + " = " + (s == heldSO));

        bool isServeable = serveableMenuSOArray.Any(s => s == heldSO);
        Debug.Log("isServeable = " + isServeable);

        if (!isServeable)
        {
            Debug.Log("เมนูนี้เสิร์ฟไม่ได้");
            return;
        }

        bool success = orderManager.TryServeOrder(heldSO.objectName);
        Debug.Log("TryServeOrder = " + success);

        if (success)
            player.GetKitchenObject().DestroySelf();
    }
}