using UnityEngine;

[CreateAssetMenu(fileName = "KitchenObjectSO", menuName = "ScriptableObjects/KitchenObjectSO")]
public class KitchenObjectSO : ScriptableObject
{
    public Transform prefab;
    public Sprite icon;
    public string objectName;
}