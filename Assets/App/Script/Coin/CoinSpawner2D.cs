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

    [Header("Detection Settings")]
    public float checkRadius = 0.3f; // radius untuk pengecekan area
    public LayerMask collisionMask;  // Layer yang dianggap sebagai penghalang

    private float screenRightEdge;
    private int maxAttempts = 5; // batas percobaan spawn ulang

    private void Start()
    {
        CalculateScreenBounds();
        StartCoroutine(SpawnLoop());
    }

    private void CalculateScreenBounds()
    {
        screenRightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if(!GameManager.Instance.IsPlaying) // Pastikan game sedang berjalan
            {
                yield return null; // Tunggu frame berikutnya
                continue;
            }
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
            TrySpawnCoin();
        }
    }

    private void TrySpawnCoin()
    {
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            float spawnX = screenRightEdge + xSpawnOffset;
            float spawnY = Random.Range(yMin, yMax);
            Vector2 spawnPosition = new Vector2(spawnX, spawnY);

            // Gunakan OverlapCircle untuk cek area
            Collider2D hit = Physics2D.OverlapCircle(spawnPosition, checkRadius, collisionMask);
            if (hit == null)
            {
                Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
                return; // selesai jika berhasil spawn
            }
        }

        Debug.LogWarning("Gagal spawn coin setelah beberapa percobaan. Area mungkin penuh.");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        float rightEdge = Application.isPlaying ? screenRightEdge : Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        Vector3 center = new Vector3(rightEdge + xSpawnOffset, (yMin + yMax) / 2, 0);
        Vector3 size = new Vector3(0.1f, yMax - yMin, 0);
        Gizmos.DrawWireCube(center, size);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float testX = Application.isPlaying ? screenRightEdge + xSpawnOffset : Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + xSpawnOffset;
        Vector3 testCenter = new Vector3(testX, (yMin + yMax) / 2f, 0);
        Gizmos.DrawWireSphere(testCenter, checkRadius);
    }
}
