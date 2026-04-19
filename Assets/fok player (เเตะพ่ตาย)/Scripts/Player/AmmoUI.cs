using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    public WeaponSystem weaponSystem;
    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        if (weaponSystem != null)
            weaponSystem.onAmmoChanged.AddListener(UpdateAmmoUI);

        // แสดงค่าเริ่มต้น
        if (weaponSystem != null && weaponSystem.weaponData != null)
            UpdateAmmoUI(weaponSystem.currentAmmo, weaponSystem.weaponData.maxAmmo);
    }

    void UpdateAmmoUI(int current, int max)
    {
        if (weaponSystem.weaponData.weaponType == WeaponData.WeaponType.Katana)
            text.text = "∞";
        else
            text.text = $"{current} / {max}";
    }
}