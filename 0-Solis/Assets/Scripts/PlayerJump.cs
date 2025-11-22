using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Components")]
    public Animator anim;
    public Rigidbody2D rb;
    public Transform feet;

    [Header("Jump Settings")]
    public float jumpForce = 20f;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    public float groundRange = 0.0002f;
    public bool isAlive = true;

    [Header("Jump Tracking")]
    public bool canJump = false;
    public int jumpTimes = 0;

    [Header("Jump Sounds")]
    public AudioSource[] jumpSounds;      // Assign your jump sound AudioSources here
    public float minPitch = 0.95f;        // Minimum pitch variation
    public float maxPitch = 1.05f;        // Maximum pitch variation

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check if grounded to reset jumps
        if (IsGrounded() && jumpTimes <= 0)
            canJump = true;
        else
            canJump = false;

        // Jump input
        if (Input.GetButtonDown("Jump") && canJump && isAlive)
            Jump();
    }

    public void Jump()
    {
        jumpTimes += 1;

        // Apply jump force
        rb.linearVelocity = Vector2.up * jumpForce;

        // Trigger jump animation
        if (anim != null)
            anim.SetTrigger("Jump");

        // Play a random jump sound with random pitch
        if (jumpSounds.Length > 0)
        {
            int index = Random.Range(0, jumpSounds.Length);
            AudioSource sfx = jumpSounds[index];

            // Random pitch
            sfx.pitch = Random.Range(minPitch, maxPitch);
            sfx.Play();
        }
    }

    public bool IsGrounded()
    {
        Collider2D groundCheck = Physics2D.OverlapCircle(feet.position, groundRange, groundLayer);
        Collider2D enemyCheck = Physics2D.OverlapCircle(feet.position, groundRange, enemyLayer);

        if (groundCheck != null || enemyCheck != null)
        {
            jumpTimes = 0;
            return true;
        }

        return false;
    }
}
