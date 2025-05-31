using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject powerUpPrefab;
    public float spawnInterval = 15f;
    public float spawnChance = 0.3f;
    
    [Header("Spawn Position")]
    public float spawnX = 12f;
    public float minY = -2f;
    public float maxY = 3f;
    
    private float spawnTimer = 0f;
    
    void Update()
    {
        // Hanya spawn jika game berjalan
        if (!GameManager.Instance.IsPlaying) return; // Uncomment jika ada GameManager
        
        spawnTimer += Time.deltaTime;
        
        if (spawnTimer >= spawnInterval)
        {
            if (Random.Range(0f, 1f) <= spawnChance)
            {
                SpawnPowerUp();
            }
            spawnTimer = 0f;
        }
    }
    
    void SpawnPowerUp()
    {
        float spawnY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);
        
        Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
    }
}