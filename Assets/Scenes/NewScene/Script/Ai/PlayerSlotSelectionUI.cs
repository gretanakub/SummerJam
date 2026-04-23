// ================================================================
//  PlayerSlotSelectionUI.cs
//  วางบน Prefab: PlayerSlot_Selection
//  แสดงชื่อผู้เล่น + กรอบ highlight + รูปตัวละครที่เลือก
// ================================================================
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSlotSelectionUI : MonoBehaviour
{
    [Header("─── References (ลากจาก Prefab) ───")]
    public TextMeshProUGUI playerNameText;     // "Player 1"
    public TextMeshProUGUI characterNameText;  // "(P1)Swordman"
    public Image characterPortrait;            // รูปตัวละคร
    public Image borderImage;                  // กรอบ — เปลี่ยนสีตอน active

    [Header("─── Colors ───")]
    public Color activeColor   = new Color(0.5f, 0.3f, 1.0f); // ม่วง
    public Color inactiveColor = new Color(0.3f, 0.3f, 0.3f); // เทา

    private PlayerData _data;

    /// <summary>เรียกจาก CharacterSelectionManager ตอนสร้าง Slot</summary>
    public void Initialize(PlayerData data)
    {
        _data = data;
        if (playerNameText    != null) playerNameText.text    = data.playerName;
        if (characterNameText != null) characterNameText.text = string.Empty;
        if (characterPortrait != null) characterPortrait.gameObject.SetActive(false);
        SetActive(false);
    }

    /// <summary>เปิด/ปิด highlight กรอบ</summary>
    public void SetActive(bool isActive)
    {
        if (borderImage != null)
            borderImage.color = isActive ? activeColor : inactiveColor;
    }

    /// <summary>อัปเดต UI เมื่อผู้เล่นเลือกตัวละคร</summary>
    public void SetCharacter(CharacterType type)
    {
        var info = CharacterSelectionManager.Instance.GetCharacterInfo(type);
        string name = info != null ? info.displayName : type.ToString();

        if (characterNameText != null)
            characterNameText.text = $"(P{_data.playerIndex + 1}){name}";

        if (characterPortrait != null && info?.portrait != null)
        {
            characterPortrait.sprite = info.portrait;
            characterPortrait.gameObject.SetActive(true);
        }
    }
}
