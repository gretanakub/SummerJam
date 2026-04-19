using UnityEngine;
using System.Collections;

public class PlayerDash : MonoBehaviour
{
    public float dashSpeed = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 2f;

    [Header("Trail")]
    public TrailRenderer trailRenderer;

    public float lastDashTime = -99f;
    public bool isDashing = false;

    private CharacterController controller;
    private PlayerInputHandler inputHandler;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        inputHandler = GetComponent<PlayerInputHandler>();
        if (trailRenderer != null)
            trailRenderer.emitting = false;
    }

    void Update()
    {
        if (inputHandler.DashInputDown && CanDash() && !isDashing)
            StartCoroutine(DashCoroutine());
    }

    bool CanDash() => Time.time >= lastDashTime + dashCooldown;

    IEnumerator DashCoroutine()
    {
        isDashing = true;
        lastDashTime = Time.time;
        Vector3 dashDirection = transform.forward;

        if (trailRenderer != null) trailRenderer.emitting = true;

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (trailRenderer != null) trailRenderer.emitting = false;
        isDashing = false;
    }
}