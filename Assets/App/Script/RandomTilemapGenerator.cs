using System.Collections.Generic;
using UnityEngine;

public class RandomTilemapGenerator : MonoBehaviour
{
    [Header("Prefab Terrain Acak")]
    public List<GameObject> tilePrefabs;

    [Header("Starter Terrain di Scene")]
    public List<GameObject> starterTerrains; // Drag manual dari scene

    [Header("Pengaturan Spawn")]
    public Vector3 spawnPosition = new Vector3(10f, 0f, 0f);
    public float spawnInterval = 2f;

    [Header("Gerakan Terrain")]
    public float scrollSpeed = 2f;

    [Header("Parent Grid (opsional)")]
    public Transform parent; // Tambahkan ini

    private List<GameObject> activeTerrains = new List<GameObject>();
    private float timer;

    private bool starterBolehBergerak = false;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying) return;

        
            foreach (var starter in starterTerrains)
            {
                if (starter != null)
                    starter.transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
            }
        

        // 2. Timer untuk spawn terrain baru
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnRandomTerrain();
            timer = spawnInterval;
        }

        // 3. Gerakkan dan bersihkan terrain yang di-spawn
        ScrollTerrains();
        CleanupTerrains();
    }

    // Panggil fungsi ini misalnya dari OnTriggerEnter player untuk mengaktifkan scroll starter terrain
    public void AktifkanStarterTerrain()
    {
        starterBolehBergerak = true;
    }

    void SpawnRandomTerrain()
    {
        if (tilePrefabs == null || tilePrefabs.Count == 0) return;

        GameObject prefab = tilePrefabs[Random.Range(0, tilePrefabs.Count)];
        Vector3 spawnPos = new Vector3(spawnPosition.x, 0f, spawnPosition.z);
        GameObject terrain = Instantiate(prefab, spawnPos, Quaternion.identity, parent); // Gunakan parent di sini
        activeTerrains.Add(terrain);
    }

    void ScrollTerrains()
    {
        foreach (var terrain in activeTerrains)
        {
            if (terrain != null)
                terrain.transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
        }
    }

    void CleanupTerrains()
    {
        for (int i = activeTerrains.Count - 1; i >= 0; i--)
        {
            if (activeTerrains[i].transform.position.x < -30f)
            {
                Destroy(activeTerrains[i]);
                activeTerrains.RemoveAt(i);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(spawnPosition, new Vector3(1f, 0.1f, 0.1f));
    }
}
