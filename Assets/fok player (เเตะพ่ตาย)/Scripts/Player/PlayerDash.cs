using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerDash : MonoBehaviour
{
    public float dashSpeed = 20f;       // ความเร็วตอน Dash
    public float dashDuration = 0.15f;  // Dash นานแค่ไหน
    public float dashCooldown = 2f;     // Cooldown

    [Header("Trail (ใส่ TrailRenderer ถ้ามี)")]
    public TrailRenderer trailRenderer;

    public float lastDashTime = -99f;
    public bool isDashing = false;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // ปิด Trail ตอนเริ่ม
        if (trailRenderer != null)
            trailRenderer.emitting = false;
    }

    void Update()
    {
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame && CanDash() && !isDashing)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    bool CanDash()
    {
        return Time.time >= lastDashTime + dashCooldown;
    }

    IEnumerator DashCoroutine()
    {
        isDashing = true;
        lastDashTime = Time.time;

        Vector3 dashDirection = transform.forward;

        // เปิด Trail
        if (trailRenderer != null)
            trailRenderer.emitting = true;

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // ปิด Trail
        if (trailRenderer != null)
            trailRenderer.emitting = false;

        isDashing = false;
    }
}