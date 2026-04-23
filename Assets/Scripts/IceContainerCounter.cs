using UnityEngine;

public class IceContainerCounter : MonoBehaviour, IKitchenObjectParent, ICounter
{
    [SerializeField] private KitchenObjectSO cupSO;
    [SerializeField] private KitchenObjectSO cupOfIceSO;
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public void Interact(PlayerController player)
    {
        if (!player.HasKitchenObject()) return;

        if (player.GetKitchenObject().GetKitchenObjectSO() == cupSO)
        {
            // เช็ค null ก่อน
            if (cupOfIceSO == null)
            {
                Debug.LogError("cupOfIceSO ยังไม่ได้ assign ใน Inspector!");
                return;
            }

            // Destroy Cup เดิมก่อน
            player.GetKitchenObject().DestroySelf();

            // แล้วค่อย Spawn Cup of Ice
            KitchenObject.SpawnKitchenObject(cupOfIceSO, player);
        }
        else
        {
            Debug.Log("ต้องถือ Cup ก่อนถึงจะใช้ได้");
        }
    }

    public Transform GetKitchenObjectFollowTransform() => counterTopPoint;
    public void SetKitchenObject(KitchenObject obj) => kitchenObject = obj;
    public KitchenObject GetKitchenObject() => kitchenObject;
    public void ClearKitchenObject() => kitchenObject = null;
    public bool HasKitchenObject() => kitchenObject != null;
}