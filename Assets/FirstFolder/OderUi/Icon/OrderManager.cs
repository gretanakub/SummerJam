using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class OrderManager : MonoBehaviour
{
    public Transform orderFrame;        // Orderframe (มี Layout Group)
    public GameObject[] orderPrefabs = new GameObject[8];
    public float defaultTime = 45f;

    private Dictionary<string, GameObject> prefabMap;
    private Color colorFull = new Color(0.18f, 0.80f, 0.25f); // เขียว
    private Color colorEmpty = new Color(1.00f, 0.00f, 0.00f); // แดง

    void Awake()
    {
        prefabMap = new Dictionary<string, GameObject>();
        foreach (var p in orderPrefabs)
            if (p != null) prefabMap[p.name] = p;
    }

    // ── เรียกได้เรื่อยๆ แต่ละชิ้นมี Timer ของตัวเอง ──────
    public void SpawnOrder(string orderName, float time = -1)
    {
        if (!prefabMap.ContainsKey(orderName))
        {
            Debug.LogWarning($"ไม่พบ Prefab: {orderName}");
            return;
        }

        GameObject obj = Instantiate(prefabMap[orderName], orderFrame);
        obj.name = orderName;

        float duration = (time > 0) ? time : defaultTime;

        // หา TimeBar ใน Prefab นั้น
        Image bar = FindTimeBar(obj);

        if (bar != null)
        {
            bar.type = Image.Type.Filled;
            bar.fillMethod = Image.FillMethod.Horizontal;
            bar.fillAmount = 1f;
        }

        // เริ่ม Coroutine แยกต่างหากสำหรับ Order นี้
        StartCoroutine(RunTimer(obj, bar, duration));
    }

    // ── Coroutine: นับถอยหลังของแต่ละ Order ──────────────
    private IEnumerator RunTimer(GameObject obj, Image bar, float duration)
    {
        float t = duration;

        while (t > 0f)
        {
            t -= Time.deltaTime;
            float ratio = Mathf.Clamp01(t / duration); // ← เปลี่ยนชื่อตัวแปรจาก fill เป็น ratio

            if (bar != null)
            {
                bar.fillAmount = ratio;
                bar.color = Color.Lerp(colorEmpty, colorFull, ratio); // ← เพิ่มบรรทัดนี้
            }

            yield return null;
        }

        if (bar != null)
        {
            bar.fillAmount = 0f;
            bar.color = colorEmpty; // ← เพิ่มบรรทัดนี้
        }

        OnOrderTimeUp(obj);
    }

    // ── หมดเวลา: ลบ Order ออกจาก Layout ────────────────
    private void OnOrderTimeUp(GameObject obj)
    {
        Debug.Log($"{obj.name} หมดเวลา!");
        Destroy(obj);
        // Layout Group จะจัดตำแหน่งใหม่อัตโนมัติ
    }

    // ── ยกเลิก Order ด้วยมือ (เช่น ลูกค้าได้รับออเดอร์) ──
    public void CompleteOrder(string orderName)
    {
        Transform found = orderFrame
            .Cast<Transform>()
            .FirstOrDefault(c => c.name == orderName);

        if (found != null) Destroy(found.gameObject);
    }

    // ── Helper: หา TimeBar ใน Prefab ─────────────────────
    private Image FindTimeBar(GameObject root)
    {
        Transform tb = root.transform.Find("BG/BG2/TimeBarBg/TimeBar");
        if (tb != null) return tb.GetComponent<Image>();

        return root.GetComponentsInChildren<Image>()
                   .FirstOrDefault(img => img.name == "TimeBar");
    }
}