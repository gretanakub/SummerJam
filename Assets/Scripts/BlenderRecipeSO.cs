using UnityEngine;

[CreateAssetMenu(fileName = "BlenderRecipeSO", menuName = "ScriptableObjects/BlenderRecipeSO")]
public class BlenderRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;   // ของที่จะปั่น เช่น OrangeSlice
    public KitchenObjectSO output;  // ของที่ได้หลังปั่น เช่น OrangeJuice
}