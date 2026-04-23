using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayerHUD
    {
        public GameObject panel;
        public TextMeshProUGUI heartsText;
        public TextMeshProUGUI playerName;
        public TextMeshProUGUI ammoText;
    }

    public PlayerHUD[] playerHUDs;
    private GameObject[] players;
    private bool[] hudInitialized;

    void Start()
    {
        foreach (var hud in playerHUDs)
            hud.panel.SetActive(false);

        hudInitialized = new bool[playerHUDs.Length];
    }

    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length && i < playerHUDs.Length; i++)
        {
            if (players[i] == null) continue;

            if (!playerHUDs[i].panel.activeSelf)
                playerHUDs[i].panel.SetActive(true);

            if (!hudInitialized[i])
            {
                playerHUDs[i].playerName.text = "P" + (i + 1);

                WeaponSystem weapon = players[i].GetComponent<WeaponSystem>();
                if (weapon != null)
                {
                    int index = i;
                    weapon.onAmmoChanged.AddListener((current, reserve) =>
                    {
                        if (playerHUDs[index].ammoText == null) return;
                        if (weapon.weaponData != null &&
                            weapon.weaponData.weaponType == WeaponData.WeaponType.Katana)
                            playerHUDs[index].ammoText.text = "∞";
                        else
                            playerHUDs[index].ammoText.text = $"{current} / {reserve}";
                    });

                    if (weapon.weaponData != null)
                        playerHUDs[i].ammoText.text = $"{weapon.currentAmmo} / {weapon.reserveAmmo}";
                }

                PlayerHealthSystem hp = players[i].GetComponent<PlayerHealthSystem>();
                if (hp != null)
                {
                    int index = i;
                    hp.onHeartsChanged.AddListener((hearts) =>
                    {
                        UpdateHearts(index, hearts, hp.maxHearts);
                    });

                    UpdateHearts(i, hp.currentHearts, hp.maxHearts);
                }

                hudInitialized[i] = true;
            }
        }

        for (int i = players.Length; i < playerHUDs.Length; i++)
            playerHUDs[i].panel.SetActive(false);
    }

    void UpdateHearts(int playerIndex, int hearts, int maxHearts)
    {
        if (playerHUDs[playerIndex].heartsText == null) return;

        string display = "";
        for (int i = 0; i < maxHearts; i++)
            display += i < hearts ? "O " : "X ";

        playerHUDs[playerIndex].heartsText.text = display;
    }
}