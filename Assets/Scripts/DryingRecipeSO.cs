using UnityEngine;

[CreateAssetMenu(fileName = "DryingRecipeSO", menuName = "ScriptableObjects/DryingRecipeSO")]
public class DryingRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float dryingTimeMax; // วินาทีที่ใช้ตาก
}