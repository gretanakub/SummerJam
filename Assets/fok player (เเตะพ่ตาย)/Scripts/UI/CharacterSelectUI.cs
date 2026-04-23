using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectUI : MonoBehaviour
{
    public CharacterData[] characters;
    public Image[] characterImages;
    public Button[] characterButtons; // เพิ่ม array ของปุ่ม

    private static CharacterData[] selectedCharacters;
    private static int currentPlayerSelecting = 0;
    private static bool[] characterTaken; // เช็คว่าตัวไหนถูกเลือกแล้ว

    public TMPro.TextMeshProUGUI playerTurnText;

    void Start()
    {
        currentPlayerSelecting = 0;
        int count = Mathf.Max(1, PlayerSelectMenu.numberOfPlayers);
        selectedCharacters = new CharacterData[count];
        characterTaken = new bool[characters.Length];

        for (int i = 0; i < characters.Length; i++)
        {
            if (characterImages[i] != null && characters[i].portrait != null)
                characterImages[i].sprite = characters[i].portrait;
        }

        UpdatePlayerTurnText();
    }

    void UpdatePlayerTurnText()
    {
        if (playerTurnText != null)
            playerTurnText.text = "Player " + (currentPlayerSelecting + 1) + " เลือกตัวละคร";
    }

    public void SelectCharacter(int index)
    {
        if (characterTaken[index]) return; // ถ้าถูกเลือกแล้วไม่ทำอะไร

        if (CharacterSelector.Instance == null)
        {
            GameObject obj = new GameObject("CharacterSelector");
            obj.AddComponent<CharacterSelector>();
        }

        // Grey Out ปุ่มที่เลือก
        characterTaken[index] = true;
        if (characterButtons[index] != null)
        {
            characterButtons[index].interactable = false;
            characterImages[index].color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }

        selectedCharacters[currentPlayerSelecting] = characters[index];
        currentPlayerSelecting++;

        if (currentPlayerSelecting >= PlayerSelectMenu.numberOfPlayers)
        {
            CharacterSelector.Instance.SetAllCharacters(selectedCharacters);
            SceneManager.LoadScene("Test");
        }
        else
        {
            UpdatePlayerTurnText();
        }
    }

    public static CharacterData GetCharacterForPlayer(int playerIndex)
    {
        if (selectedCharacters != null && playerIndex < selectedCharacters.Length)
            return selectedCharacters[playerIndex];
        return null;
    }
}