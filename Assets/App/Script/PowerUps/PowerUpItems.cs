using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    
    void Update()
    {
        if(!GameManager.Instance.IsPlaying) return; // Pastikan game sedang berjalan
        // Bergerak ke kiri seperti obstacle
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        
        // Destroy jika keluar layar
        if (transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Aktivasi powerup
            PowerUpManager.Instance.ActivatePowerUp();
            
            // Destroy item
            Destroy(gameObject);
        }
    }
}