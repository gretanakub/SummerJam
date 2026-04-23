using UnityEngine;
using UnityEngine.UI;

public class MortarProgressUI : MonoBehaviour
{
    [SerializeField] private MortarCounter mortarCounter;
    [SerializeField] private Image barImage;

    private void Start()
    {
        mortarCounter.OnGrindingProgressChanged += MortarCounter_OnGrindingProgressChanged;
        barImage.fillAmount = 0f;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        mortarCounter.OnGrindingProgressChanged -= MortarCounter_OnGrindingProgressChanged;
    }

    private void MortarCounter_OnGrindingProgressChanged(float progressNormalized)
    {
        barImage.fillAmount = progressNormalized;
        gameObject.SetActive(progressNormalized > 0f);
    }
}