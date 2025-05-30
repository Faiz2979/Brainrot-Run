using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Vector2 obstaclePosition; // obstacle is GameObject
    private Rigidbody2D rb;
    private float speed;
    
    [Header("🎯 Attack Animation Settings")]
    [SerializeField] private float attackAnimationDelay = 0.1f; // Delay sebelum destroy obstacle

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // just confirm if rb is ready
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody2D not attachment");
            return;
        }
    }

    private void FixedUpdate()
    {
        if (speed <= 0 || !GameManager.Instance.IsPlaying) return;
        // for move the obstacle to left side
        Vector2 targetPos = speed * Time.fixedDeltaTime * Vector2.left;
        rb.MovePosition(rb.position + targetPos);
    }

    public void SetSpeed(float Speed)
    {
        // the Speed is from the ObstacleSpawner
        speed = Speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // for destroy the obstacle if touch a BoundaryObstacleSpawner
        if (collision.collider.CompareTag("BoundaryObstacleSpawner"))
        {
            Destroy(gameObject);
        }
        
        else if (collision.collider.CompareTag("Player"))
        {
            // ** POWERUP LOGIC - Cek apakah PowerUp aktif **
            if (PowerUpManager.Instance != null && PowerUpManager.Instance.IsPowerUpActive())
            {
                Debug.Log("🥁 TUNG TUNG TUNG SAHUR COLLISION DETECTED!");
                
                // *** TRIGGER ATTACK ANIMATION SEGERA ***
                bool attackTriggered = TriggerPlayerAttackAnimation(collision.collider);
                
                if (attackTriggered)
                {
                    // Mulai coroutine untuk destroy obstacle dengan delay
                    StartCoroutine(DestroyObstacleWithDelay(collision.collider));
                }
                else
                {
                    // Fallback: langsung destroy jika attack animation gagal
                    Debug.LogWarning("⚠️ Attack animation failed, destroying obstacle immediately");
                    DestroyObstacleImmediately();
                }
                
                return; // Keluar tanpa game over
            }
            
            // ** LOGIC ASLI - Game Over **
            Destroy(gameObject);
            Debug.Log("Player Die");
            GameManager.Instance.GameOver(); // Call GameManager to handle game over
        }
    }
    
    /// <summary>
    /// Coroutine untuk destroy obstacle dengan delay (memberikan waktu attack animation)
    /// </summary>
    private IEnumerator DestroyObstacleWithDelay(Collider2D playerCollider)
    {
        Debug.Log($"⏳ Waiting {attackAnimationDelay}s for attack animation...");
        
        // Tunggu sebentar untuk attack animation
        yield return new WaitForSeconds(attackAnimationDelay);
        
        // Destroy obstacle menggunakan PowerUpManager
        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.DestroyObstacle(gameObject);
            Debug.Log("💥 OBSTACLE DESTROYED BY TUNG TUNG TUNG SAHUR!");
        }
        else
        {
            // Fallback destroy
            Destroy(gameObject);
            Debug.Log("💥 Obstacle destroyed (fallback)");
        }
    }
    
    /// <summary>
    /// Destroy obstacle langsung tanpa delay
    /// </summary>
    private void DestroyObstacleImmediately()
    {
        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.DestroyObstacle(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Trigger attack animation pada player saat PowerUp aktif
    /// </summary>
    private bool TriggerPlayerAttackAnimation(Collider2D playerCollider)
    {
        try
        {
            Debug.Log("🎯 Attempting to trigger attack animation...");
            
            // Method 1: Melalui PlayerAnimationPowerUp (RECOMMENDED & PRIMARY)
            PlayerAnimationPowerUp playerAnimPowerUp = playerCollider.GetComponent<PlayerAnimationPowerUp>();
            if (playerAnimPowerUp != null)
            {
                // Pastikan PowerUp benar-benar aktif
                if (playerAnimPowerUp.IsPowerUpActive())
                {
                    playerAnimPowerUp.TriggerAttackAnimation();
                    Debug.Log("✅ Attack animation triggered via PlayerAnimationPowerUp");
                    return true;
                }
                else
                {
                    Debug.LogWarning("⚠️ PlayerAnimationPowerUp found but PowerUp is not active!");
                }
            }
            else
            {
                Debug.LogWarning("⚠️ PlayerAnimationPowerUp component not found on player!");
            }

            // Method 2: Fallback ke Animator langsung (SECONDARY)
            Animator playerAnimator = playerCollider.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                // Cek apakah parameter isAttacking ada
                bool hasAttackParameter = false;
                foreach (AnimatorControllerParameter param in playerAnimator.parameters)
                {
                    if (param.name == "isAttacking" && param.type == AnimatorControllerParameterType.Trigger)
                    {
                        hasAttackParameter = true;
                        break;
                    }
                }
                
                if (hasAttackParameter)
                {
                    playerAnimator.SetTrigger("isAttacking");
                    Debug.Log("✅ Attack animation triggered via Animator directly");
                    return true;
                }
                else
                {
                    Debug.LogWarning("⚠️ Animator found but no 'isAttacking' trigger parameter");
                }
            }
            else
            {
                Debug.LogWarning("⚠️ Animator component not found on player!");
            }

            // Method 3: Debug info untuk troubleshooting
            Debug.LogError("❌ NO SUITABLE METHOD FOUND TO TRIGGER ATTACK ANIMATION!");
            Debug.LogError("Make sure Player has:");
            Debug.LogError("1. PlayerAnimationPowerUp component");
            Debug.LogError("2. Animator component with 'isAttacking' trigger");
            Debug.LogError("3. PowerUp is properly activated");
            
            return false;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ ERROR triggering attack animation: {e.Message}");
            Debug.LogError($"Stack trace: {e.StackTrace}");
            return false;
        }
    }
    
    #region Debug Methods
    [ContextMenu("🧪 Test Attack Animation")]
    public void TestAttackAnimation()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Debug.Log("🧪 Testing attack animation on found player...");
            TriggerPlayerAttackAnimation(player.GetComponent<Collider2D>());
        }
        else
        {
            Debug.LogError("❌ No player found with 'Player' tag!");
        }
    }
    
    [ContextMenu("🔍 Debug Player Components")]
    public void DebugPlayerComponents()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Debug.Log("=== 🔍 PLAYER COMPONENTS DEBUG ===");
            
            // Check all components
            Component[] components = player.GetComponents<Component>();
            foreach (Component comp in components)
            {
                Debug.Log($"• {comp.GetType().Name}");
            }
            
            // Specific checks
            PlayerAnimationPowerUp powerUp = player.GetComponent<PlayerAnimationPowerUp>();
            Debug.Log($"PlayerAnimationPowerUp: {(powerUp != null ? "✅ Found" : "❌ Missing")}");
            if (powerUp != null)
            {
                Debug.Log($"PowerUp Active: {powerUp.IsPowerUpActive()}");
            }
            
            Animator anim = player.GetComponent<Animator>();
            Debug.Log($"Animator: {(anim != null ? "✅ Found" : "❌ Missing")}");
            
            Debug.Log("=== END DEBUG ===");
        }
        else
        {
            Debug.LogError("❌ No player found!");
        }
    }
    #endregion
}