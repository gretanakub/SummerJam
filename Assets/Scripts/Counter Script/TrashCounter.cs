using UnityEngine;

public class TrashCounter : MonoBehaviour, ICounter
{
    public void Interact(PlayerController player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
        }
    }
}