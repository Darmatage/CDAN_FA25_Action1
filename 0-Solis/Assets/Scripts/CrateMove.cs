using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateMove : MonoBehaviour {

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public Transform crateBottom;
    float groundRange = 0.1f;
    public bool isGrounded = false;

    [Header("Dragging")]
    public AudioSource[] dragSounds;
    public float minPitch = 0.95f;
    public float maxPitch = 1.05f;
    public float dragRepeatTime = 0.5f; // delay between sounds

    private AudioSource currentDragSound;
    private float dragTimer = 0f;
    private Vector3 lastColliderPos;

    //For pulling crate, create a joint:
    GameObject thePlayer;
    FixedJoint2D fixedJoint;
    public bool isTouchingPlayer = false;
    public bool canPull = false;
    public Transform jointNode;

    void Start() {
        lastColliderPos = crateBottom.position;
    }

    void Update() {
    // Handle pulling/releasing
    if (Input.GetKeyDown("e")) {
        if (isTouchingPlayer && fixedJoint == null) {
            jointNode.position = thePlayer.transform.position;
            jointNode.parent = thePlayer.transform;
            gameObject.transform.parent = jointNode;

            fixedJoint = jointNode.gameObject.AddComponent<FixedJoint2D>();
            fixedJoint.connectedBody = thePlayer.GetComponent<Rigidbody2D>();
            fixedJoint.autoConfigureConnectedAnchor = false;
            canPull = true;
        }
        else if (fixedJoint != null) {
            Destroy(fixedJoint);
            thePlayer = null;
            canPull = false;
            gameObject.transform.parent = null;
            jointNode.parent = gameObject.transform;
        }
    }

    // Update grounded state
    IsGroundedCheck();

    // Check horizontal movement by collider position (any movement)
    float deltaX = Mathf.Abs(crateBottom.position.x - lastColliderPos.x);

    // Play drag sound if crate is moving while grounded
    if (isGrounded && deltaX > 0.001f) {
        PlayDragSound();
    } else {
        StopDragSound();
    }

    lastColliderPos = crateBottom.position; // store for next frame
}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Hit [E] to pull");
            isTouchingPlayer = true;
            thePlayer = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            isTouchingPlayer = false;
        }
    }

    void OnCollisionExit2D(Collision2D other) {
        if (IsGroundedCheck()) {
            //gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
    }

    public bool IsGroundedCheck() {
        Collider2D groundCheck = Physics2D.OverlapCircle(crateBottom.position, groundRange, groundLayer);
        isGrounded = (groundCheck != null);
        return isGrounded;
    }

    void PlayDragSound() {
        dragTimer -= Time.deltaTime;
        if (dragTimer <= 0f && dragSounds.Length > 0) {
            int index = Random.Range(0, dragSounds.Length);
            currentDragSound = dragSounds[index];
            currentDragSound.pitch = Random.Range(minPitch, maxPitch);
            currentDragSound.Play();
            dragTimer = dragRepeatTime;
        }
    }

    void StopDragSound() {
        if (currentDragSound != null && currentDragSound.isPlaying) {
            currentDragSound.Stop();
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(crateBottom.position, groundRange);
    }
}
