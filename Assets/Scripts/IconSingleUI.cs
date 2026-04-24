using UnityEngine;
using UnityEngine.UI;

public class IconSingleUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;

    public void SetIcon(Sprite sprite)
    {
        iconImage.sprite = sprite;
    }
}