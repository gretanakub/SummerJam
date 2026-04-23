using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject playerSlotPrefab;
    public Transform container;

    [Header("Controls")]
    public Button startButton;

    private void Start()
    {
        // ตัวอย่าง: สั่งให้สร้าง 4 ช่องตอนเริ่มเกม
        // คุณสามารถเปลี่ยนเลข 4 เป็นตัวแปรอื่นได้ตามต้องการ
        RefreshLobby(4);

        if (startButton != null)
            startButton.onClick.AddListener(OnStartGame);
    }

    // ฟังก์ชันใหม่สำหรับสั่ง Update จำนวนคนใน Lobby
    public void RefreshLobby(int playerCount)
    {
        // 1. ลบของเก่าออกให้หมดก่อนสร้างใหม่
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        // 2. ล้างข้อมูลเก่าใน RoleManager ด้วย (ถ้ามี)
        if (RoleManager.Instance != null)
        {
            RoleManager.Instance.playerRoles.Clear();
        }

        // 3. สร้าง Prefab ตามจำนวนที่ระบุมาใน Parameter
        for (int i = 1; i <= playerCount; i++)
        {
            GameObject newSlot = Instantiate(playerSlotPrefab, container);
            PlayerSlotUI slotScript = newSlot.GetComponent<PlayerSlotUI>();

            if (slotScript != null)
            {
                slotScript.playerId = i; // กำหนด ID 1, 2, 3...
            }
        }

        Debug.Log($"สร้างช่องผู้เล่นเรียบร้อยทั้งหมด: {playerCount} ช่อง");
    }

    void OnStartGame()
    {
        var allRoles = RoleManager.Instance.playerRoles;
        Debug.Log("=== ข้อมูลส่งออกเมื่อกด Start ===");

        foreach (var entry in allRoles)
        {
            Debug.Log($"Player {entry.Key}: {entry.Value}");
        }
    }
}