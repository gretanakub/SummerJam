using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public static CharacterSelector Instance;
    public CharacterData selectedCharacter;
    public CharacterData[] allSelectedCharacters;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectCharacter(CharacterData data)
    {
        selectedCharacter = data;
    }

    public void SetAllCharacters(CharacterData[] characters)
    {
        allSelectedCharacters = characters;
    }

    public CharacterData GetCharacterForPlayer(int playerIndex)
    {
        if (allSelectedCharacters != null && playerIndex < allSelectedCharacters.Length)
            return allSelectedCharacters[playerIndex];
        return null;
    }
}