using UnityEngine;
using Unity.Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineCamera gameCam;
    [SerializeField] private CinemachineCamera freeCam;

    private bool isFreeCamera = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isFreeCamera = !isFreeCamera;
            SwitchCamera(isFreeCamera);
            Debug.Log("Tab pressed! isFreeCamera = " + isFreeCamera);
        }
    }

    private void SwitchCamera(bool useFree)
    {
        if (useFree)
        {
            freeCam.Priority = 20;
            gameCam.Priority = 0;
        }
        else
        {
            gameCam.Priority = 20;
            freeCam.Priority = 0;
        }

        // เช็คว่า Priority เปลี่ยนจริงไหม
        Debug.Log($"gameCam Priority: {gameCam.Priority} | freeCam Priority: {freeCam.Priority}");
    }
}