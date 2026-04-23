using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Settings")]
    [SerializeField] private int startScore = 0;
    [SerializeField] private int minScore = 0;      // ป้องกันติดลบ (ถ้าไม่ต้องการจำกัด ใส่ int.MinValue)

    private int _score;

    // ──────────────────────────────────────────────────
    //  Unity
    // ──────────────────────────────────────────────────

    private void Awake()
    {
        _score = startScore;
        UpdateDisplay();
    }

    // ──────────────────────────────────────────────────
    //  Public API
    // ──────────────────────────────────────────────────

    /// <summary>เพิ่ม Score</summary>
    public void IncreaseScore(int amount)
    {
        _score += amount;
        UpdateDisplay();
    }

    /// <summary>ลด Score (ไม่ต่ำกว่า minScore)</summary>
    public void DecreaseScore(int amount)
    {
        _score = Mathf.Max(minScore, _score - amount);
        UpdateDisplay();
    }

    /// <summary>ดึงค่า Score ปัจจุบัน (ใช้เมื่อ Time Up)</summary>
    public int GetScore() => _score;

    /// <summary>รีเซ็ต Score กลับเป็นค่าเริ่มต้น</summary>
    public void ResetScore()
    {
        _score = startScore;
        UpdateDisplay();
    }

    // ──────────────────────────────────────────────────
    //  Internal
    // ──────────────────────────────────────────────────

    private void UpdateDisplay()
    {
        if (scoreText != null)
            scoreText.text = _score.ToString();
    }
}
