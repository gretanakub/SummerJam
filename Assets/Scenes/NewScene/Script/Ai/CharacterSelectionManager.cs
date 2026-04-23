// ================================================================
//  CharacterSelectionManager.cs
//  วางบน GameObject "CharacterSelectionManager" ใน CharacterSelect Scene
//  อ่าน PlayerCount จาก GameSessionData แล้วควบคุม Flow การเลือกทั้งหมด
// ================================================================
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance { get; private set; }

    [Header("─── Slot Container ───")]
    [Tooltip("HorizontalLayoutGroup ที่จะวาง PlayerSlot_Selection Prefab")]
    public Transform selectionSlotsContainer;

    [Tooltip("Prefab: PlayerSlot_Selection (มี Script PlayerSlotSelectionUI)")]
    public GameObject playerSlotSelectionPrefab;

    [Header("─── Character Panel ───")]
    [Tooltip("GameObject ที่มี Script CharacterSelectionPanel")]
    public CharacterSelectionPanel characterPanel;

    [Header("─── Buttons ───")]
    [Tooltip("ปุ่ม Play — ซ่อนอยู่จนกว่าทุกคนจะเลือกครบ")]
    public Button startGameButton;

    [Header("─── Scene ───")]
    public string nextSceneName = "GameScene";

    [Header("─── Character Data ───")]
    [Tooltip("กำหนด Sprite + AccentColor ของตัวละครแต่ละตัวใน Inspector")]
    public List<CharacterInfo> characterInfoList = new List<CharacterInfo>();

    // ── State ─────────────────────────────────────────────────
    private int _totalPlayers;
    private int _currentIndex;
    private List<PlayerData> _players        = new List<PlayerData>();
    private List<PlayerSlotSelectionUI> _slotUIs = new List<PlayerSlotSelectionUI>();

    // ── Lifecycle ─────────────────────────────────────────────
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        InitDefaultCharacterInfo();
    }

    private void Start()
    {
        // อ่านจำนวนผู้เล่นจาก Scene ก่อนหน้า
        _totalPlayers = GameSessionData.PlayerCount;
        if (_totalPlayers <= 0)
        {
            Debug.LogWarning("[CharacterSelectionManager] PlayerCount = 0 → ใช้ fallback = 2 (Editor only)");
            _totalPlayers = 2;
        }
        BeginSelection();
    }

    // ── Selection Flow ────────────────────────────────────────

    private void BeginSelection()
    {
        startGameButton?.gameObject.SetActive(false);

        _players.Clear();
        _slotUIs.Clear();
        foreach (Transform child in selectionSlotsContainer) Destroy(child.gameObject);

        for (int i = 0; i < _totalPlayers; i++)
        {
            _players.Add(new PlayerData(i));
            var go    = Instantiate(playerSlotSelectionPrefab, selectionSlotsContainer);
            var slot  = go.GetComponent<PlayerSlotSelectionUI>();
            slot?.Initialize(_players[i]);
            if (slot != null) _slotUIs.Add(slot);
        }

        _currentIndex = 0;
        ActivateSlot(0);
    }

    private void ActivateSlot(int index)
    {
        for (int i = 0; i < _slotUIs.Count; i++)
            _slotUIs[i].SetActive(i == index);

        characterPanel.Show(_players[index]);
    }

    // ── Public API ────────────────────────────────────────────

    /// <summary>CharacterSelectionPanel เรียกเมื่อผู้เล่นกดเลือกตัวละคร</summary>
    public void OnCharacterSelected(CharacterType character)
    {
        _players[_currentIndex].character = character;
        _slotUIs[_currentIndex].SetCharacter(character);
        _slotUIs[_currentIndex].SetActive(false);
        characterPanel.Hide();

        _currentIndex++;

        if (_currentIndex < _totalPlayers)
            ActivateSlot(_currentIndex);
        else
        {
            GameSessionData.SetPlayers(_players); // → บันทึก OUTPUT
            startGameButton?.gameObject.SetActive(true);
        }
    }

    /// <summary>ผูกกับปุ่ม Play ใน Inspector (OnClick)</summary>
    public void StartGame() => SceneManager.LoadScene(nextSceneName);

    /// <summary>ดึง CharacterInfo ตาม type (ใช้โดย Slot/Panel)</summary>
    public CharacterInfo GetCharacterInfo(CharacterType type)
        => characterInfoList.Find(c => c.type == type);

    // ── Default Data ──────────────────────────────────────────
    private void InitDefaultCharacterInfo()
    {
        if (characterInfoList.Count > 0) return;
        characterInfoList = new List<CharacterInfo>
        {
            new CharacterInfo { type = CharacterType.Commando,   displayName = "Commando",   accentColor = new Color(0.2f, 0.6f, 1.0f) },
            new CharacterInfo { type = CharacterType.Swordman,   displayName = "Swordman",   accentColor = new Color(1.0f, 0.4f, 0.1f) },
            new CharacterInfo { type = CharacterType.Gunslinger, displayName = "Gunslinger", accentColor = new Color(0.9f, 0.8f, 0.1f) },
            new CharacterInfo { type = CharacterType.Vanguard,   displayName = "Vanguard",   accentColor = new Color(0.6f, 0.2f, 1.0f) },
        };
    }
}
