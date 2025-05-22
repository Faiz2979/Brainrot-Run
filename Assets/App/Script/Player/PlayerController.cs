using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 7f;

    // private bool isPlaying () => GameManager.Instance.IsPlaying;
    private Rigidbody2D rb;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Check for jump input and only allow jumping if grounded
        if (Input.GetKey("space") && isGrounded && GameManager.Instance.IsPlaying)
        {
            Debug.Log("Jump");
            Jump();
        }
    }

    void Jump()
    {
        
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // If we hit something tagged "Ground", we become grounded
        if (col.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
