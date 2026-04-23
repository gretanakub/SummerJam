using UnityEngine;
using Unity.Cinemachine;

public class CameraZoomLimit : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private float minY = 5f;
    [SerializeField] private float maxY = 30f;
    [SerializeField] private float zoomSpeed = 2f;

    private CinemachineFollow followComponent;

    void Awake()
    {
        followComponent = cinemachineCamera.GetComponent<CinemachineFollow>();
    }

    void Update()
    {
        if (followComponent == null) return;

        // Scroll wheel zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 offset = followComponent.FollowOffset;
        offset.y -= scroll * zoomSpeed * 10f;

        // ล็อคไม่ให้เกินขอบเขต
        offset.y = Mathf.Clamp(offset.y, minY, maxY);
        followComponent.FollowOffset = offset;
    }
}