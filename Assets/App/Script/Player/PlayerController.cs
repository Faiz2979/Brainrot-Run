using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    [SerializeField] private Animator anim;
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
            downQueued = true;
        }

        AnimationController();
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.IsPlaying) return;

        if (jumpQueued)
        {
            Jump();
            jumpCount++;
            jumpQueued = false;
        }

        if (downQueued)
        {
            rb.velocity = new Vector2(rb.velocity.x, -jumpForce * 1.5f);
            downQueued = false;
        }
    }

 private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coins"))
        {
            CoinsManager.Instance.AddCoins(1); // Gunakan singleton
            Destroy(other.gameObject, 0.05f); // Delay kecil untuk memastikan trigger selesai
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

void AnimationController()
{
    float verticalVelocity = rb.velocity.y;

    // Reset semua dulu
    anim.SetBool("isRunning", false);
    anim.SetBool("isJumping", false);
    anim.SetBool("isFalling", false);

    if (isGrounded && GameManager.Instance.IsPlaying){
        anim.SetBool("isRunning", true);

        // Mainkan landSound jika belum dimainkan
        if (!isPlayingLandSound)
        {
            audioSource.clip = landSound;
            audioSource.loop = true;
            audioSource.Play();
            isPlayingLandSound = true;
        }
    }
    else
    {
        if (isPlayingLandSound)
        {
            audioSource.Stop();
            isPlayingLandSound = false;
        }

        if (verticalVelocity > 0.1f)
        {
            anim.SetBool("isJumping", true);
        }
        else if (verticalVelocity < -0.1f)
        {
            anim.SetBool("isFalling", true);
        }
    }
}

}
