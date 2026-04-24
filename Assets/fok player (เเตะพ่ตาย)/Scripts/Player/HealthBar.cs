using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public HealthSystem healthSystem;
    public int currentHealth;

    void Start()
    {
        healthSystem.onHealthChanged.AddListener(UpdateBar);
    }

    void LateUpdate()
    {
        // ยก Health Bar ขึ้นเหนือหัว
        transform.position = transform.parent.position + new Vector3(0, 1.5f, 0);
    
        // หันหน้าหา Camera เสมอ
        transform.rotation = Camera.main.transform.rotation;
    }

    void UpdateBar(float value)
    {
        slider.value = value;

        Image fill = slider.fillRect.GetComponent<Image>();
        if (value > 0.5f)       fill.color = Color.green;
        else if (value > 0.25f) fill.color = Color.yellow;
        else                    fill.color = Color.red;
    }
}