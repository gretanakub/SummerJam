using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayerHUD
    {
        public GameObject panel;
        public Slider hpBar;
        public TextMeshProUGUI playerName;
    }

    public PlayerHUD[] playerHUDs;

    private GameObject[] players;

    void Start()
    {
        foreach (var hud in playerHUDs)
            hud.panel.SetActive(false);

        players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length && i < playerHUDs.Length; i++)
        {
            playerHUDs[i].panel.SetActive(true);
            playerHUDs[i].playerName.text = "Player " + (i + 1);
        }
    }

void Update()
{
    for (int i = 0; i < players.Length && i < playerHUDs.Length; i++)
    {
        if (players[i] == null) continue; // ข้ามถ้า player ถูก destroy

        HealthSystem hp = players[i].GetComponent<HealthSystem>();
        if (hp != null)
            playerHUDs[i].hpBar.value = hp.GetHealthPercent();
    }
}
}