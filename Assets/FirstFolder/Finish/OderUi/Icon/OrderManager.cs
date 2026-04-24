using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }

    [Header("UI")]
    public Transform orderFrame;
    public GameObject[] orderPrefabs = new GameObject[8];

    [Header("Settings")]
    public float defaultTime = 45f;
    public float spawnInterval = 15f;
    public int maxOrders = 5;
    public int scorePerOrder = 100;
    public int penaltyPerFail = 50;

    private Dictionary<string, GameObject> prefabMap;
    private List<string> activeOrders = new List<string>();
    private Color colorFull = new Color(0.18f, 0.80f, 0.25f);
    private Color colorEmpty = new Color(1.00f, 0.00f, 0.00f);

    private string[] menuNames = new string[]
    {
        "Citrus Sunrise Blend",
        "Berry Tropical Rush",
        "Golden Pine Citrus",
        "Melon Sweet Splash",
        "Berry Citrus Punch",
        "Tropical Wave Juice",
        "Sunset Berry Lemon",
        "Orange Melon Chill"
    };

    private ScoreManager scoreManager;
    private bool isGameRunning = false;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        prefabMap = new Dictionary<string, GameObject>();
        foreach (var p in orderPrefabs)
            if (p != null) prefabMap[p.name] = p;
    }

    private void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
    }

    public void StartOrdering()
    {
        isGameRunning = true;
        scoreManager = FindFirstObjectByType<ScoreManager>();
        StartCoroutine(SpawnOrderRoutine());
    }

    public void StopOrdering()
    {
        isGameRunning = false;
        StopAllCoroutines();
    }

    private IEnumerator SpawnOrderRoutine()
    {
        SpawnRandomOrder();

        while (isGameRunning)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (activeOrders.Count < maxOrders)
                SpawnRandomOrder();
        }
    }

    private void SpawnRandomOrder()
    {
        List<string> available = menuNames
            .Where(m => prefabMap.ContainsKey(m))
            .ToList();

        if (available.Count == 0) return;

        string randomMenu = available[Random.Range(0, available.Count)];
        SpawnOrder(randomMenu, defaultTime);
    }

    public void SpawnOrder(string orderName, float time = -1)
    {
        if (!prefabMap.ContainsKey(orderName))
        {
            Debug.LogWarning($"ไม่พบ Prefab: {orderName}");
            return;
        }

        GameObject obj = Instantiate(prefabMap[orderName], orderFrame);
        obj.name = orderName + "_" + System.Guid.NewGuid().ToString().Substring(0, 4);

        activeOrders.Add(obj.name);

        float duration = (time > 0) ? time : defaultTime;

        Image bar = FindTimeBar(obj);
        if (bar != null)
        {
            bar.type = Image.Type.Filled;
            bar.fillMethod = Image.FillMethod.Horizontal;
            bar.fillAmount = 1f;
        }

        StartCoroutine(RunTimer(obj, bar, duration, orderName));
    }

    private IEnumerator RunTimer(GameObject obj, Image bar, float duration, string menuName)
    {
        float t = duration;

        while (t > 0f && obj != null)
        {
            t -= Time.deltaTime;
            float ratio = Mathf.Clamp01(t / duration);

            if (bar != null)
            {
                bar.fillAmount = ratio;
                bar.color = Color.Lerp(colorEmpty, colorFull, ratio);
            }

            yield return null;
        }

        if (obj == null) yield break;

        if (bar != null)
        {
            bar.fillAmount = 0f;
            bar.color = colorEmpty;
        }

        OnOrderTimeUp(obj);
    }

    private void OnOrderTimeUp(GameObject obj)
    {
        Debug.Log($"{obj.name} หมดเวลา!");
        activeOrders.Remove(obj.name);

        if (scoreManager != null)
            scoreManager.DecreaseScore(penaltyPerFail);

        Destroy(obj);
    }

    public bool TryServeOrder(string menuName)
    {
        string found = activeOrders
            .FirstOrDefault(o => o.StartsWith(menuName));

        if (found != null)
        {
            Transform orderObj = orderFrame
                .Cast<Transform>()
                .FirstOrDefault(c => c.name == found);

            if (orderObj != null)
                Destroy(orderObj.gameObject);

            activeOrders.Remove(found);

            if (scoreManager != null)
                scoreManager.IncreaseScore(scorePerOrder);

            Debug.Log($"เสิร์ฟ {menuName} สำเร็จ! +{scorePerOrder} คะแนน");
            return true;
        }

        Debug.Log($"ไม่มี order {menuName} อยู่!");
        return false;
    }

    private Image FindTimeBar(GameObject root)
    {
        Transform tb = root.transform.Find("BG/BG2/TimeBarBg/TimeBar");
        if (tb != null) return tb.GetComponent<Image>();

        return root.GetComponentsInChildren<Image>()
                   .FirstOrDefault(img => img.name == "TimeBar");
    }
}