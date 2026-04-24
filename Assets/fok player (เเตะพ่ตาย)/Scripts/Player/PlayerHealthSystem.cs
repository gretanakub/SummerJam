using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthSystem : MonoBehaviour
{
    public int maxHearts = 3;
    public int currentHearts;

    public UnityEvent<int> onHeartsChanged;
    public UnityEvent onDeath;

    void Start()
    {
        currentHearts = maxHearts;
        onHeartsChanged.Invoke(currentHearts);
    }

    public void TakeDamage(int damage)
    {
        currentHearts--;
        onHeartsChanged.Invoke(currentHearts);

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayPlayerHit();

        if (currentHearts <= 0)
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayPlayerDeath();

            onDeath.Invoke();
            Destroy(gameObject);
        }
    }

    public void Heal(int amount) // ← เพิ่มตรงนี้
    {
        currentHearts = Mathf.Clamp(currentHearts + amount, 0, maxHearts);
        onHeartsChanged.Invoke(currentHearts);
    }

    public float GetHealthPercent()
    {
        return (float)currentHearts / maxHearts;
    }
}