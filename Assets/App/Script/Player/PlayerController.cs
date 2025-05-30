// PlayerController.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [Header("Audio Settings")]
    public AudioClip jumpSound;
    public AudioClip landSound;

    private AudioSource audioSource;
    private bool isPlayingLandSound = false;

    [Header("Slide Settings")]
    public float slideDuration = 0.5f;
    private bool isSliding = false;
    private float slideTimer = 0f;

    public CoinsManager cm;
    public TMP_Text coinText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && jumpCount < maxJumpCount)
        {
            jumpQueued = true;
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (isGrounded)
            {
                isSliding = true;
                slideTimer = slideDuration;
            }
            else
            {
                downQueued = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isSliding)
            {
                isSliding = false;
            }
            else if (isGrounded)
            {
                isSliding = true;
                slideTimer = slideDuration;
            }
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.IsPlaying) return;

        if (jumpQueued)
        {
            if (isSliding)
            {
                isSliding = false;
            }
            Jump();
            jumpCount++;
            jumpQueued = false;
        }

        if (downQueued)
        {
            rb.velocity = new Vector2(rb.velocity.x, -jumpForce * 1.5f);
            downQueued = false;
        }

        if (isSliding)
        {
            slideTimer -= Time.fixedDeltaTime;
            if (slideTimer <= 0)
            {
                isSliding = false;
            }
        }

        HandleAudio();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coins"))
        {
            CoinsManager.Instance.AddCoins(1);
            Destroy(other.gameObject, 0.05f);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        audioSource.PlayOneShot(jumpSound);
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
        }
    }

    void HandleAudio()
    {
        if (isGrounded && GameManager.Instance.IsPlaying)
        {
            if (isSliding)
            {
                if (isPlayingLandSound)
                {
                    audioSource.Stop();
                    isPlayingLandSound = false;
                }
            }
            else
            {
                if (!isPlayingLandSound)
                {
                    audioSource.clip = landSound;
                    audioSource.loop = true;
                    audioSource.Play();
                    isPlayingLandSound = true;
                }
            }
        }
        else
        {
            if (isPlayingLandSound)
            {
                audioSource.Stop();
                isPlayingLandSound = false;
            }
        }
    }

    public bool IsGrounded => isGrounded;
    public bool IsSliding => isSliding;
    public float VerticalVelocity => rb.velocity.y;
}