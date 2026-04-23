using UnityEngine;
using UnityEngine.UI;

public class DryingProgressUI : MonoBehaviour
{
    [SerializeField] private DryingRackCounter dryingRackCounter;
    [SerializeField] private Image barImage;

    private void Start()
    {
        dryingRackCounter.OnDryingProgressChanged += DryingRackCounter_OnDryingProgressChanged;
        barImage.fillAmount = 0f;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        dryingRackCounter.OnDryingProgressChanged -= DryingRackCounter_OnDryingProgressChanged;
    }

    private void DryingRackCounter_OnDryingProgressChanged(float progressNormalized)
    {
        barImage.fillAmount = progressNormalized;
        gameObject.SetActive(progressNormalized > 0f);
    }
}