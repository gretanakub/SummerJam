using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public UnityEvent<float> onHealthChanged;
    public UnityEvent onDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        onHealthChanged.Invoke((float)currentHealth / maxHealth);

        // เสียงโดนดาเมจ
    if (SoundManager.Instance != null)
        SoundManager.Instance.PlayPlayerHit();

    if (currentHealth <= 0)
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayPlayerDeath();

        onDeath.Invoke();
        Destroy(gameObject);
    }
}

    public float GetHealthPercent()
    {
        return (float)currentHealth / maxHealth;
    }
}