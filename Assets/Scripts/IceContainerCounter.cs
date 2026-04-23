using UnityEngine;

public class IceContainerCounter : MonoBehaviour, IKitchenObjectParent, ICounter
{
    [SerializeField] private KitchenObjectSO cupSO;        // Cup ที่ต้องถืออยู่
    [SerializeField] private KitchenObjectSO cupOfIceSO;   // Cup of Ice ที่จะได้หลังใช้
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public void Interact(PlayerController player)
    {
        if (!player.HasKitchenObject())
        {
            // ไม่ได้ถือของ → ไม่ทำอะไร
            return;
        }

        // เช็คว่าถือ Cup อยู่ไหม
        if (player.GetKitchenObject().GetKitchenObjectSO() == cupSO)
        {
            // ทำลาย Cup เดิม
            player.GetKitchenObject().DestroySelf();

            // Spawn Cup of Ice ให้ player
            KitchenObject.SpawnKitchenObject(cupOfIceSO, player);
        }
        else
        {
            // ถือของอื่นอยู่ → ไม่ทำอะไร
            Debug.Log("ต้องถือ Cup ก่อนถึงจะใช้ได้");
        }
    }

    public Transform GetKitchenObjectFollowTransform() => counterTopPoint;
    public void SetKitchenObject(KitchenObject obj) => kitchenObject = obj;
    public KitchenObject GetKitchenObject() => kitchenObject;
    public void ClearKitchenObject() => kitchenObject = null;
    public bool HasKitchenObject() => kitchenObject != null;
}