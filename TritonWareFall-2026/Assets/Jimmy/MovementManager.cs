using UnityEngine;
using System.Collections.Generic;

// Overcooked-style movement: WASD to move, Space to dash.
// Gets wobblier and more slippery as the chef gets drunk.
public class MovementManager : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float acceleration = 60f;

    public float dashSpeed = 16f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.6f;

    public float maxWobbleAngle = 35f;   // how much steering sways when fully drunk

    // Direction the chef is facing. Starts facing the camera (down).
    public Vector3 facing = Vector3.back;

    public Rigidbody rb;
    public Vector3 input;
    float dashTimer;
    float cooldownTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f).normalized;

        dashTimer -= Time.deltaTime;
        cooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0f)
        {
            dashTimer = dashDuration;
            cooldownTimer = dashCooldown;
        }
    }

    void FixedUpdate()
    {
        float drunk = DrunkManager.Drunk_Manager.Drunk_Amount;

        // Drunk wobble: rotate the input direction back and forth
        float wobble = Mathf.Sin(Time.time * 3f) * maxWobbleAngle * drunk;
        Vector3 direction = Quaternion.Euler(0f, 0f, wobble) * input;

        if (direction != Vector3.zero)
            facing = direction;

        Vector3 targetVelocity;
        if (dashTimer > 0f)
            targetVelocity = facing * dashSpeed;
        else
            targetVelocity = direction * moveSpeed;

        // Drunk = slippery: accelerate slower the drunker you are
        float accel = acceleration * (1f - drunk * 0.75f);
        Vector3 newVelocity = Vector3.MoveTowards(rb.linearVelocity, targetVelocity, accel * Time.fixedDeltaTime);
        if (dashTimer > 0f) newVelocity = targetVelocity;

        rb.linearVelocity = newVelocity;
    }
}