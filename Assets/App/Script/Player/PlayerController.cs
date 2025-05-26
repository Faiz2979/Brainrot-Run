using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 7f;

     [Header("Slide Settings")]
    public float slideForce = 10f;
    public float slideDuration = 1f;
    public float slideColliderHeight = 0.5f; // collider height when sliding

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private Collider2D playerCollider;
    private float originalColliderHeight;
    private Vector2 originalColliderOffset;

    private int jumpCount = 0;
    private int maxJumpCount = 2;

    private bool jumpQueued = false;
    private bool downQueued = false;
     private bool slideQueued = false;
    
    private bool isSliding = false;
    private float slideTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
         
         // saving the size of the real collider
        if (playerCollider is BoxCollider2D boxCollider)
        {
            originalColliderHeight = boxCollider.size.y;
            originalColliderOffset = boxCollider.offset;
        }
        else if (playerCollider is CapsuleCollider2D capsuleCollider)
        {
            originalColliderHeight = capsuleCollider.size.y;
            originalColliderOffset = capsuleCollider.offset;
        }
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && jumpCount < maxJumpCount)
        {
            jumpQueued = true;
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (isGrounded && !isSliding)
            {
                slideQueued = true;
            }
            else
            {
                downQueued = true;
            }
              }

        // Update slide timer
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f)
            {
                StopSlide();
            }
        }
    }

     void FixedUpdate()
    {
        if (!GameManager.Instance.IsPlaying) return;

        if (jumpQueued && !isSliding)
        {
            Jump();
            jumpCount++;
            jumpQueued = false;
        }

        if (slideQueued)
        {
            StartSlide();
            slideQueued = false;
        }

        if (downQueued && !isSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, -jumpForce * 1.5f);
            downQueued = false;
        }

        // Maintain slide velocity
        if (isSliding)
        {
            rb.velocity = new Vector2(slideForce, rb.velocity.y);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false;
    }

    void StartSlide()
    {
        if (!isGrounded) return;
        
        Debug.Log("Slide Started");
        isSliding = true;
        slideTimer = slideDuration;
        
        // modify collider height
        ModifyCollider(slideColliderHeight);
        
        // Set horizontal velocity for sliding
        rb.velocity = new Vector2(slideForce, rb.velocity.y);
    }

    void StopSlide()
    {
        Debug.Log("Slide Stopped");
        isSliding = false;
        
        // when sliding stopped go back to original collider
        ModifyCollider(originalColliderHeight);
    }

    void ModifyCollider(float newHeight)
    {
        if (playerCollider is BoxCollider2D boxCollider)
        {
            Vector2 newSize = boxCollider.size;
            newSize.y = newHeight;
            boxCollider.size = newSize;
            
            // Adjust offset so the box collider keep touching the ground
            Vector2 newOffset = originalColliderOffset;
            newOffset.y = originalColliderOffset.y - (originalColliderHeight - newHeight) / 2f;
            boxCollider.offset = newOffset;
        }
        else if (playerCollider is CapsuleCollider2D capsuleCollider)
        {
            Vector2 newSize = capsuleCollider.size;
            newSize.y = newHeight;
            capsuleCollider.size = newSize;
            
            // same as the code above but for capsule collider
            Vector2 newOffset = originalColliderOffset;
            newOffset.y = originalColliderOffset.y - (originalColliderHeight - newHeight) / 2f;
            capsuleCollider.offset = newOffset;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            isGrounded = false;
            
            // Stop slide if player no longer touching ground
            if (isSliding)
            {
                StopSlide();
            }
        }
    }

    // Public method for checking slide status
    public bool IsSliding()
    {
        return isSliding;
    }
}