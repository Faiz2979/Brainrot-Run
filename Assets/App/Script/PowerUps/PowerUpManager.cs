using System.Collections;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;
    
    [Header("PowerUp Settings")]
    public float powerUpDuration = 5f;
    public AudioClip powerUpSound;
    public AudioClip obstacleDestroySound;
    
    private AudioSource audioSource;
    private bool isPowerUpActive = false;
    private float powerUpTimer = 0f;
    
    // Event untuk UI
    public System.Action<bool> OnPowerUpStateChanged;
    public System.Action<float> OnPowerUpTimerUpdated;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Update()
    {
        if (isPowerUpActive)
        {
            powerUpTimer -= Time.deltaTime;
            OnPowerUpTimerUpdated?.Invoke(powerUpTimer);
            
            if (powerUpTimer <= 0)
            {
                DeactivatePowerUp();
            }
        }
    }
    
    public void ActivatePowerUp()
    {
        isPowerUpActive = true;
        powerUpTimer = powerUpDuration;
        
        if (powerUpSound != null)
            audioSource.PlayOneShot(powerUpSound);
        
        OnPowerUpStateChanged?.Invoke(true);
        
        Debug.Log("TUNG TUNG TUNG SAHUR ACTIVATED!");
    }
    
    public void DeactivatePowerUp()
    {
        isPowerUpActive = false;
        powerUpTimer = 0f;
        OnPowerUpStateChanged?.Invoke(false);
        Debug.Log("PowerUp Deactivated!");
    }
    
    public bool IsPowerUpActive()
    {
        return isPowerUpActive;
    }
    
    public void DestroyObstacle(GameObject obstacle)
    {
        if (isPowerUpActive)
        {
            if (obstacleDestroySound != null)
                audioSource.PlayOneShot(obstacleDestroySound);
            
            Destroy(obstacle);
        }
    }
}