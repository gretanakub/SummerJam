using UnityEngine;
using System.Collections.Generic;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;

    // เก็บ player ที่กำลัง select counter นี้อยู่
    private List<PlayerController> selectingPlayers = new List<PlayerController>();

    private void Start()
    {
        // Subscribe event ของ player ทุกคนที่อยู่ในฉาก
        PlayerController[] allPlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        foreach (PlayerController player in allPlayers)
        {
            player.OnSelectedCounterChanged += OnSelectedCounterChanged;
        }

        Hide();
    }

    private void OnDestroy()
    {
        PlayerController[] allPlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        foreach (PlayerController player in allPlayers)
        {
            player.OnSelectedCounterChanged -= OnSelectedCounterChanged;
        }
    }

    private void OnSelectedCounterChanged(ClearCounter selectedCounter)
    {
        // ไม่ได้ใช้ เพราะไม่รู้ว่า player ไหนยิง event
        // ใช้ Update เช็คแทน
    }

    private void Update()
    {
        // เช็คทุก frame ว่ามี player ไหน select counter นี้อยู่ไหม
        PlayerController[] allPlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        bool anyPlayerSelecting = false;

        foreach (PlayerController player in allPlayers)
        {
            if (player.GetSelectedCounter() == clearCounter)
            {
                anyPlayerSelecting = true;
                break;
            }
        }

        if (anyPlayerSelecting)
            Show();
        else
            Hide();
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