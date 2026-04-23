using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelectMenu : MonoBehaviour
{
    public static int numberOfPlayers = 1;

    public void SelectPlayers(int count)
    {
        numberOfPlayers = count;
        SceneManager.LoadScene("CharacterSelect");
    }
}