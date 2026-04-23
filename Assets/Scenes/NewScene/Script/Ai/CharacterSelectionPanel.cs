// ================================================================
//  CharacterSelectionPanel.cs
//  วางบน GameObject: CharacterPanel
//  Panel ที่โชว์ปุ่มตัวละคร 4 ตัว ให้ผู้เล่นคนปัจจุบันเลือก
// ================================================================
using System;                      // Action<T>
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;              // Button, Image
using TMPro;

public class CharacterSelectionPanel : MonoBehaviour
{
    [Header("─── References ───")]
    [Tooltip("ปุ่ม 4 ตัว ตามลำดับ: Commando, Swordman, Gunslinger, Vanguard")]
    public List<CharacterOptionButton> characterButtons;

    [Tooltip("ข้อความแสดงว่าใครกำลังเลือกอยู่")]
    public TextMeshProUGUI promptText;

    private void Awake() => gameObject.SetActive(false);

    /// <summary>เปิด Panel สำหรับผู้เล่นที่กำลังเลือก</summary>
    public void Show(PlayerData player)
    {
        gameObject.SetActive(true);

        if (promptText != null)
            promptText.text = $"{player.playerName} — เลือกตัวละคร";

        CharacterType[] types =
        {
            CharacterType.Commando,
            CharacterType.Swordman,
            CharacterType.Gunslinger,
            CharacterType.Vanguard
        };

        for (int i = 0; i < characterButtons.Count && i < types.Length; i++)
        {
            var info = CharacterSelectionManager.Instance.GetCharacterInfo(types[i]);
            characterButtons[i].Initialize(types[i], info, OnPicked);
        }
    }

    public void Hide() => gameObject.SetActive(false);

    private void OnPicked(CharacterType type)
        => CharacterSelectionManager.Instance.OnCharacterSelected(type);
}


// ================================================================
//  CharacterOptionButton.cs
//  วางบน Prefab ของปุ่มตัวละครแต่ละตัวภายใน CharacterPanel
// ================================================================
public class CharacterOptionButton : MonoBehaviour
{
    [Header("─── References ───")]
    public Button button;
    public Image portraitImage;
    public TextMeshProUGUI nameText;
    public Image frameImage;      // กรอบปุ่ม — เปลี่ยนสีตาม accentColor

    private CharacterType _type;
    private Action<CharacterType> _onSelect;

    public void Initialize(CharacterType type, CharacterInfo info, Action<CharacterType> onSelect)
    {
        _type = type;
        _onSelect = onSelect;

        if (nameText != null) nameText.text = info != null ? info.displayName : type.ToString();
        if (portraitImage != null && info?.portrait != null) portraitImage.sprite = info.portrait;
        if (frameImage != null && info != null) frameImage.color = info.accentColor;

        if (button == null) { Debug.LogError($"[CharacterOptionButton] ไม่ได้ลาก Button ใน Inspector ({gameObject.name})"); return; }
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => _onSelect?.Invoke(_type));
    }
}