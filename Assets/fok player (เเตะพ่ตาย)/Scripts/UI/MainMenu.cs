using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("PlayerSelect");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}