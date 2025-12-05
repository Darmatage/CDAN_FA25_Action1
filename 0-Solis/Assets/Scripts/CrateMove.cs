using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CrateMove : MonoBehaviour
{
    Rigidbody2D rb2D;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public Transform crateBottom;
    float groundRange = 0.1f;
    public bool isGrounded = false;

    [Header("Dragging")]
    public AudioSource[] dragSounds;
    public float minPitch = 0.95f;
    public float maxPitch = 1.05f;
    public float dragRepeatTime = 0.5f;

    private AudioSource currentDragSound;
    private float dragTimer = 0f;
    private Vector3 lastColliderPos;

    // Pulling crate
    GameObject thePlayer;
    FixedJoint2D fixedJoint;
    public bool isTouchingPlayer = false;
    public bool canPull = false;
    public Transform jointNode;

    [Header("UI")]
    public TMP_Text grabPromptText; // Assign this in Inspector

    void Start()
    {
        lastColliderPos = crateBottom.position;
        rb2D = GetComponent<Rigidbody2D>();

        // Freeze horizontal position initially
        rb2D.constraints = RigidbodyConstraints2D.FreezePositionX;

        // Hide the prompt at start
        if (grabPromptText != null)
            grabPromptText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Handle grabbing/releasing
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isTouchingPlayer && fixedJoint == null && thePlayer != null)
            {
                // Grab the crate
                jointNode.position = thePlayer.transform.position;
                jointNode.parent = thePlayer.transform;
                gameObject.transform.parent = jointNode;

                fixedJoint = jointNode.gameObject.AddComponent<FixedJoint2D>();
                fixedJoint.connectedBody = thePlayer.GetComponent<Rigidbody2D>();
                fixedJoint.autoConfigureConnectedAnchor = false;
                
                canPull = true;

                // Hide the prompt while pulling
                if (grabPromptText != null)
                    grabPromptText.gameObject.SetActive(false);
            }
            else if (fixedJoint != null)
            {
                // Release the crate
                Destroy(fixedJoint);
                fixedJoint = null;
                canPull = false;
                gameObject.transform.parent = null;
                jointNode.parent = gameObject.transform;

                // Forget the player until they trigger again
                thePlayer = null;
                isTouchingPlayer = false;

                // Freeze horizontal movement again
                rb2D.constraints = RigidbodyConstraints2D.FreezePositionX;

                // Prompt will show again only when player re-enters the trigger
            }
        }

        // Hide the prompt while actually pushing/moving
        if (canPull && grabPromptText != null)
        {
            grabPromptText.gameObject.SetActive(false);
        }

        // Update grounded state
        IsGroundedCheck();

        // Check horizontal movement by collider position
        float deltaX = Mathf.Abs(crateBottom.position.x - lastColliderPos.x);

        // Play drag sound if crate is moving while grounded
        if (isGrounded && deltaX > 0.001f)
        {
            PlayDragSound();
        }
        else
        {
            StopDragSound();
        }

        lastColliderPos = crateBottom.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = true;
            thePlayer = other.gameObject;

            // Unfreeze horizontal movement while touching
            rb2D.constraints &= ~RigidbodyConstraints2D.FreezePositionX;

            // Show the prompt
            if (grabPromptText != null && !canPull)
            {
                grabPromptText.gameObject.SetActive(true);
                grabPromptText.text = "Press [E] to pull";
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = false;

            // Only freeze horizontal if not grabbing
            if (fixedJoint == null)
                rb2D.constraints = RigidbodyConstraints2D.FreezePositionX;

            // Hide the prompt
            if (grabPromptText != null)
                grabPromptText.gameObject.SetActive(false);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        IsGroundedCheck();
    }

    public bool IsGroundedCheck()
    {
        Collider2D groundCheck = Physics2D.OverlapCircle(crateBottom.position, groundRange, groundLayer);
        isGrounded = (groundCheck != null);
        return isGrounded;
    }

    void PlayDragSound()
    {
        dragTimer -= Time.deltaTime;
        if (dragTimer <= 0f && dragSounds.Length > 0)
        {
            int index = Random.Range(0, dragSounds.Length);
            currentDragSound = dragSounds[index];
            currentDragSound.pitch = Random.Range(minPitch, maxPitch);
            currentDragSound.Play();
            dragTimer = dragRepeatTime;
        }
    }

    void StopDragSound()
    {
        if (currentDragSound != null && currentDragSound.isPlaying)
            currentDragSound.Stop();
    }

    void OnDrawGizmosSelected()
    {
        if (crateBottom != null)
            Gizmos.DrawWireSphere(crateBottom.position, groundRange);
    }
}
