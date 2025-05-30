using UnityEngine;
using System.Collections;

public class CoinSpawner : MonoBehaviour
{
    [Header("Coin Prefab")]
    public GameObject coinPrefab;

    [Header("Spawn Timing")]
    [Tooltip("Minimum time between spawns")]
    public float minSpawnInterval = 1f;
    [Tooltip("Maximum time between spawns")]
    public float maxSpawnInterval = 3f;

    [Header("Spawn Area")]
    [Tooltip("Distance from screen edge")]
    public float xSpawnOffset = 1f;
    [Tooltip("Vertical spawn range")]
    public float yMin = -2f;
    public float yMax = 2f;

    private float screenRightEdge;

    private void Start()
    {
        CalculateScreenBounds();
        StartCoroutine(SpawnLoop());
    }

    private void CalculateScreenBounds()
    {
        // Hitung tepi kanan layar dalam world space
        screenRightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
            SpawnCoin();
        }
    }

    private void SpawnCoin()
    {
        // Hitung posisi spawn dengan offset dari tepi layar
        float spawnX = screenRightEdge + xSpawnOffset;
        float spawnY = Random.Range(yMin, yMax);

        Vector2 spawnPosition = new Vector2(spawnX, spawnY);
        Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
    }

    // Visualisasi area spawn di Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        float rightEdge = Application.isPlaying ? screenRightEdge : Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        Vector3 center = new Vector3(rightEdge + xSpawnOffset, (yMin + yMax) / 2, 0);
        Vector3 size = new Vector3(0.1f, yMax - yMin, 0);
        Gizmos.DrawWireCube(center, size);
    }
}