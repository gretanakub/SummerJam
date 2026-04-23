using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TimeManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Progress Bar")]
    [SerializeField] private Image barFill;          // Image Type = Filled หรือ Simple (scale X)
    [SerializeField] private BarMode barMode = BarMode.FillAmount;

    public enum BarMode
    {
        FillAmount,   // Image → Image Type: Filled
        ScaleX        // Image ธรรมดา scale ตาม localScale.x
    }

    // ──────────────────────────────────────────────────
    //  State
    // ──────────────────────────────────────────────────
    private float _duration;
    private bool _isRunning;
    private Coroutine _timerCoroutine;

    public event Action OnTimerFinished;

    // ──────────────────────────────────────────────────
    //  Public API
    // ──────────────────────────────────────────────────

    /// <summary>
    /// ตั้งเวลา (วินาที) แล้วเริ่มนับทันที
    /// </summary>
    public void StartTimer(float seconds)
    {
        if (_isRunning)
            StopTimer();

        _duration = Mathf.Max(0f, seconds);
        SetBar(1f);                              // bar เต็มตอนเริ่ม
        _timerCoroutine = StartCoroutine(CountDown());
    }

    /// <summary>
    /// หยุด/รีเซ็ต Timer
    /// </summary>
    public void StopTimer()
    {
        if (_timerCoroutine != null)
            StopCoroutine(_timerCoroutine);

        _isRunning = false;
        UpdateDisplay(0f);
        SetBar(0f);
    }

    // ──────────────────────────────────────────────────
    //  Core
    // ──────────────────────────────────────────────────

    private IEnumerator CountDown()
    {
        _isRunning = true;
        float remaining = _duration;

        while (remaining > 0f)
        {
            UpdateDisplay(remaining);
            SetBar(remaining / _duration);       // normalize 0–1
            yield return null;
            remaining -= Time.deltaTime;
        }

        UpdateDisplay(0f);
        SetBar(0f);
        _isRunning = false;

        OnFinished();
    }

    // ──────────────────────────────────────────────────
    //  Display helpers
    // ──────────────────────────────────────────────────

    private void UpdateDisplay(float seconds)
    {
        if (timerText == null) return;

        int mins = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", mins, secs);
    }

    /// <param name="t">ค่า 0–1 (1 = เต็ม, 0 = หมด)</param>
    private void SetBar(float t)
    {
        if (barFill == null) return;

        t = Mathf.Clamp01(t);

        switch (barMode)
        {
            case BarMode.FillAmount:
                barFill.fillAmount = t;
                break;

            case BarMode.ScaleX:
                Vector3 s = barFill.rectTransform.localScale;
                barFill.rectTransform.localScale = new Vector3(t, s.y, s.z);
                break;
        }
    }
    private ScoreManager scoreManager;
    private void OnFinished()
    {
        scoreManager = FindObjectOfType<ScoreManager>();

        if (timerText != null)
            timerText.text = "00:00";


        OnTimerFinished?.Invoke();
        Debug.Log("[TimerTMP] Time's up!");

        Debug.Log(scoreManager.GetScore());

        OnTimerFinished?.Invoke();
    }
}
