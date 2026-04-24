using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BlenderIngredientIconUI : MonoBehaviour
{
    [SerializeField] private BlenderCounter blenderCounter;
    [SerializeField] private Transform iconContainer; // parent ของ icon ทั้งหมด
    [SerializeField] private GameObject iconPrefab;   // prefab ของ icon แต่ละตัว

    private void Start()
    {
        blenderCounter.OnIngredientAdded += UpdateIcons;
        blenderCounter.OnBlendingDone += ClearIcons;
        blenderCounter.OnBlendingProgressChanged += CheckClear;
        ClearIcons();
    }

    private void OnDestroy()
    {
        blenderCounter.OnIngredientAdded -= UpdateIcons;
        blenderCounter.OnBlendingDone -= ClearIcons;
        blenderCounter.OnBlendingProgressChanged -= CheckClear;
    }

    private void CheckClear(float progress)
    {
        // ซ่อน icon ตอน reset
        if (progress == 0f && !blenderCounter.IsBlending() && !blenderCounter.IsBlendingDone())
            ClearIcons();
    }

    private void UpdateIcons()
    {
        // ลบ icon เดิมทั้งหมด
        foreach (Transform child in iconContainer)
            Destroy(child.gameObject);

        // สร้าง icon ใหม่จาก ingredientList
        List<KitchenObjectSO> ingredients = blenderCounter.GetIngredientList();
        foreach (KitchenObjectSO ingredient in ingredients)
        {
            GameObject iconObj = Instantiate(iconPrefab, iconContainer);
            iconObj.GetComponent<Image>().sprite = ingredient.icon;
        }
    }

    private void ClearIcons()
    {
        foreach (Transform child in iconContainer)
            Destroy(child.gameObject);
    }
}