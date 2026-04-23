using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectUI : MonoBehaviour
{
    public CharacterData[] characters;
    public Image[] characterImages;

    void Start()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            Debug.Log($"Character {i}: {characters[i].characterName}, Portrait: {characters[i].portrait}");
            if (characterImages[i] != null && characters[i].portrait != null)
                characterImages[i].sprite = characters[i].portrait;
        }
    }

    public void SelectCharacter(int index)
    {
        if (CharacterSelector.Instance == null)
        {
            GameObject obj = new GameObject("CharacterSelector");
            obj.AddComponent<CharacterSelector>();
        }

        CharacterSelector.Instance.SelectCharacter(characters[index]);
        SceneManager.LoadScene("Test");
    }
}