using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public  class OrderManager : MonoBehaviour
{
    void Awake()
    {
        Instance = this;
    }
    [Header("=== Prefabs ===")]
    [SerializeField] private GameObject oderPrefab;
    [SerializeField] private GameObject sutParnelPrefab;
    [SerializeField] private GameObject sutelCONPrefab;

    [Header("=== Container ===")]
    [SerializeField] private Transform oderContainer;

    public static OrderManager Instance;


    // ---------------------------------------------------------------
    public void SpawnOder(Sprite menuImg, int stepCount, float time = 45f, params Sprite[] stepImages)
    {
        GameObject oderGO = Instantiate(oderPrefab, oderContainer);

        Transform menuIcon = FindDeep(oderGO.transform, "MenuIcon");
        if (menuIcon != null) SetRoundedSprite(menuIcon.gameObject, menuImg);

        Transform timeBG = FindDeep(oderGO.transform, "TimeBG");
        if (timeBG != null) SetupTimer(timeBG, time, oderGO);

        Transform suteFrame = FindDeep(oderGO.transform, "SuteFrame");
        if (suteFrame == null) { Debug.LogError("ไม่พบ SuteFrame!"); return; }

        GameObject sutParnel = Instantiate(sutParnelPrefab, suteFrame);
        sutParnel.name = "SutParnel";

        for (int i = sutParnel.transform.childCount - 1; i >= 0; i--)
            Destroy(sutParnel.transform.GetChild(i).gameObject);

        for (int i = 0; i < stepCount; i++)
        {
            GameObject iconGO = Instantiate(sutelCONPrefab, sutParnel.transform);
            iconGO.name = $"SutelCON_{i}";

            Transform iconChild = FindDeep(iconGO.transform, "icon");
            if (iconChild != null && stepImages != null && i < stepImages.Length)
                SetRoundedSprite(iconChild.gameObject, stepImages[i]);
        }
    }

    public void SpawnOder(Sprite menuImg, int stepCount, params Sprite[] stepImages)
        => SpawnOder(menuImg, stepCount, 45f, stepImages);

    private void SafeDestroy(GameObject go)
    {
        if (go == null) return;
#if UNITY_EDITOR
        if (!Application.isPlaying)
            DestroyImmediate(go);
        else
#endif
            Destroy(go);
    }

    // ---------------------------------------------------------------
    // Timer
    // ---------------------------------------------------------------
    private void SetupTimer(Transform timeBG, float totalTime, GameObject oderGO)
    {
        Image timeBar = FindDeepComp<Image>(timeBG, "TimeBar");
        if (timeBar == null) return;

        timeBar.type = Image.Type.Filled;
        timeBar.fillAmount = 1f;

        StartCoroutine(Countdown(timeBar, totalTime, oderGO));
    }

    private IEnumerator Countdown(Image img, float duration, GameObject oderGO)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (oderGO == null) yield break;
            elapsed += Time.deltaTime;
            img.fillAmount = 1f - (elapsed / duration);
            yield return null;
        }

        SafeDestroy(oderGO);
    }

    // ---------------------------------------------------------------
    // SetRoundedSprite
    // ---------------------------------------------------------------
    private void SetRoundedSprite(GameObject target, Sprite sprite)
    {
        if (sprite == null) return;

        Image img = target.GetComponent<Image>();
        if (img == null) return;

        img.sprite = sprite;

        if (img.material != null)
        {
            img.material = new Material(img.material);
            img.material.SetTexture("_MainTex", sprite.texture);
        }

        foreach (var s in target.GetComponents<MonoBehaviour>())
        {
            if (s.GetType().Name == "ImageWithRoundedCorners")
            {
                StartCoroutine(ToggleRefresh(s));
                break;
            }
        }
    }

    private IEnumerator ToggleRefresh(MonoBehaviour script)
    {
        script.enabled = false;
        yield return null;
        script.enabled = true;
    }

    // ---------------------------------------------------------------
    // Utilities
    // ---------------------------------------------------------------
    private T FindDeepComp<T>(Transform parent, string childName) where T : Component
    {
        Transform t = FindDeep(parent, childName);
        return t != null ? t.GetComponent<T>() : null;
    }

    private Transform FindDeep(Transform parent, string name)
    {
        foreach (Transform t in parent.GetComponentsInChildren<Transform>(true))
            if (t.name == name) return t;
        return null;
    }

    // ---------------------------------------------------------------
    // Test
    // ---------------------------------------------------------------
#if UNITY_EDITOR
    [Header("=== TEST ===")]
    [SerializeField] private Sprite testMenuImg;
    [SerializeField] private int testStepCount = 3;
    [SerializeField] private float testTime = 45f;
    [SerializeField] private Sprite[] testStepImages;

    [ContextMenu("▶ Test SpawnOder")]
    private void TestSpawn() => SpawnOder(testMenuImg, testStepCount, testTime, testStepImages);
#endif
}