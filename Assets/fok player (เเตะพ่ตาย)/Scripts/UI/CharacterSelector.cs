using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public static CharacterSelector Instance;
    public CharacterData selectedCharacter;

    void Awake()
    {
        // ไม่ให้ถูกทำลายตอนเปลี่ยน Scene
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
}