using UnityEngine;

[CreateAssetMenu(fileName = "BlenderRecipeSO", menuName = "ScriptableObjects/BlenderRecipeSO")]
public class BlenderRecipeSO : ScriptableObject
{
    public KitchenObjectSO[] inputArray; // input หลายตัว
    public KitchenObjectSO output;       // น้ำที่ได้
}