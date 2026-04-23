using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private MonoBehaviour counter;
    [SerializeField] private GameObject[] visualGameObjectArray;

    private ICounter iCounter;

    private void Start()
    {
        iCounter = counter as ICounter;
        Hide();
    }

    private void Update()
    {
        PlayerController[] allPlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        bool anyPlayerSelecting = false;

        foreach (PlayerController player in allPlayers)
        {
            if (player.GetSelectedCounter() == iCounter)
            {
                anyPlayerSelecting = true;
                break;
            }
        }

        if (anyPlayerSelecting) Show();
        else Hide();
    }

    private void Show()
    {
        foreach (GameObject visual in visualGameObjectArray)
            visual.SetActive(true);
    }

    private void Hide()
    {
        foreach (GameObject visual in visualGameObjectArray)
            visual.SetActive(false);
    }
}