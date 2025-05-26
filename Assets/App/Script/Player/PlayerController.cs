using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private bool isGrounded = false;

    private int jumpCount = 0;
    private int maxJumpCount = 2;

    private bool jumpQueued = false;
    private bool downQueued = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Tangkap input sekali saja per frame
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && jumpCount < maxJumpCount)
        {
            jumpQueued = true;
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            downQueued = true;
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.IsPlaying) return;

        if (jumpQueued)
        {
            Jump();
            jumpCount++;
            jumpQueued = false; // reset agar tidak lompat dua kali
        }

        if (downQueued)
        {
            rb.velocity = new Vector2(rb.velocity.x, -jumpForce * 1.5f);
            downQueued = false;
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }
}
