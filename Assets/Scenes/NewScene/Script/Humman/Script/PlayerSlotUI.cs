using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSlotUI : MonoBehaviour
{
    public int playerId; // *** ต้องตั้งค่าใน Inspector ให้ต่างกัน 1, 2, 3, 4 ***
    private PlayerRole currentRole = PlayerRole.None;

    [Header("UI Elements")]
    public GameObject selectionPanel;
    public Button mainButton;
    public Image mainButtonImage;
    public TextMeshProUGUI centerText;

    [Header("Icons")]
    public Sprite commandoIcon, swordmanIcon, gunslingerIcon, vanguardIcon;

    [Header("Buttons")]
    public Button commandoBtn, swordmanBtn, gunslingerBtn, vanguardBtn;

    void Start()
    {
        selectionPanel.SetActive(false);
        mainButton.onClick.AddListener(() => selectionPanel.SetActive(!selectionPanel.activeSelf));

        commandoBtn.onClick.AddListener(() => RequestSelect(PlayerRole.Commando));
        swordmanBtn.onClick.AddListener(() => RequestSelect(PlayerRole.Swordman));
        gunslingerBtn.onClick.AddListener(() => RequestSelect(PlayerRole.Gunslinger));
        vanguardBtn.onClick.AddListener(() => RequestSelect(PlayerRole.Vanguard));
    }

    private void OnEnable()
    {
        RoleManager.OnRoleSelected += HandleRoleUIUpdate;
        RoleManager.OnAnyRoleChanged += RefreshDisabledButtons;
    }

    private void OnDisable()
    {
        RoleManager.OnRoleSelected -= HandleRoleUIUpdate;
        RoleManager.OnAnyRoleChanged -= RefreshDisabledButtons;
    }

    void RequestSelect(PlayerRole role)
    {
        // เช็คก่อนว่ามีคนอื่นเลือกไปหรือยัง
        if (RoleManager.Instance.IsRoleTaken(role) && currentRole != role) return;

        // ส่งคำขอไปที่ Manager
        RoleManager.Instance.SelectRole(playerId, role);
    }

    // ฟังก์ชันนี้จะทำงานเมื่อมีการเลือก Role
    void HandleRoleUIUpdate(int targetPlayerId, PlayerRole role)
    {
        // *** บัคแก้ตรงนี้: ต้องเช็คว่า ID ตรงกับเราไหม ถ้าไม่ใช่ห้ามเปลี่ยนรูป! ***
        if (targetPlayerId != playerId) return;

        currentRole = role;
        mainButtonImage.sprite = GetSpriteByRole(role);
        mainButtonImage.color = Color.white;
        centerText.text = ""; // ซ่อนข้อความ
        selectionPanel.SetActive(false);
    }

    void RefreshDisabledButtons()
    {
        // อัปเดตปุ่มสีเทาสำหรับทุกคน
        commandoBtn.interactable = !RoleManager.Instance.IsRoleTaken(PlayerRole.Commando) || currentRole == PlayerRole.Commando;
        swordmanBtn.interactable = !RoleManager.Instance.IsRoleTaken(PlayerRole.Swordman) || currentRole == PlayerRole.Swordman;
        gunslingerBtn.interactable = !RoleManager.Instance.IsRoleTaken(PlayerRole.Gunslinger) || currentRole == PlayerRole.Gunslinger;
        vanguardBtn.interactable = !RoleManager.Instance.IsRoleTaken(PlayerRole.Vanguard) || currentRole == PlayerRole.Vanguard;
    }

    Sprite GetSpriteByRole(PlayerRole role)
    {
        switch (role)
        {
            case PlayerRole.Commando: return commandoIcon;
            case PlayerRole.Swordman: return swordmanIcon;
            case PlayerRole.Gunslinger: return gunslingerIcon;
            case PlayerRole.Vanguard: return vanguardIcon;
            default: return null;
        }
    }
}