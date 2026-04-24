
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class StoryScreen : MonoBehaviour
{
    public string nextSceneName = "test";

    void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
