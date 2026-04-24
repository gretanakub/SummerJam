using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BlenderIngredientIconUI : MonoBehaviour
{
    [SerializeField] private BlenderCounter blenderCounter;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private GameObject iconPrefab;

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
        if (progress == 0f && !blenderCounter.IsBlending() && !blenderCounter.IsBlendingDone())
            ClearIcons();
    }

    private void UpdateIcons()
    {
        Debug.Log("UpdateIcons called | ingredients = " + blenderCounter.GetIngredientList().Count);

        foreach (Transform child in iconContainer)
            Destroy(child.gameObject);

        List<KitchenObjectSO> ingredients = blenderCounter.GetIngredientList();
        foreach (KitchenObjectSO ingredient in ingredients)
        {
            Debug.Log("สร้าง icon ของ " + ingredient.objectName + " | icon = " + ingredient.icon);
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