using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private Image barImage; // ลาก Bar ใส่

    private void Start()
    {
        cuttingCounter.OnCuttingProgressChanged += CuttingCounter_OnCuttingProgressChanged;
        barImage.fillAmount = 0f;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        cuttingCounter.OnCuttingProgressChanged -= CuttingCounter_OnCuttingProgressChanged;
    }

    private void CuttingCounter_OnCuttingProgressChanged(float progressNormalized)
    {
        barImage.fillAmount = progressNormalized;
        gameObject.SetActive(progressNormalized > 0f);
    }
}