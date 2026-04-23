using UnityEngine;
using UnityEngine.UI;

public class BlenderProgressUI : MonoBehaviour
{
    [SerializeField] private BlenderCounter blenderCounter;
    [SerializeField] private Image barImage;

    private void Start()
    {
        blenderCounter.OnBlendingProgressChanged += BlenderCounter_OnBlendingProgressChanged;
        barImage.fillAmount = 0f;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        blenderCounter.OnBlendingProgressChanged -= BlenderCounter_OnBlendingProgressChanged;
    }

    private void BlenderCounter_OnBlendingProgressChanged(float progressNormalized)
    {
        barImage.fillAmount = progressNormalized;
        gameObject.SetActive(progressNormalized > 0f);
    }
}