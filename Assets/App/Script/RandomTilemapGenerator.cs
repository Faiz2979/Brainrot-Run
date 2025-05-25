using System.Collections.Generic;
using UnityEngine;

public class RandomTilemapGenerator : MonoBehaviour
{
    [Header("Prefab Terrain Tanpa Obstacle")]
    public List<GameObject> tilePrefabsTanpaObstacle;

    [Header("Prefab Terrain Dengan Obstacle")]
    public List<GameObject> tilePrefabsDenganObstacle;

    [Header("Starter Terrain di Scene")]
    public List<GameObject> starterTerrains; // Drag manual dari scene

    [Header("Pengaturan Spawn")]
    public Vector3 spawnPosition = new Vector3(10f, 0f, 0f);
    public float spawnInterval = 2f;

    [Range(0f, 1f)] public float chanceTerrainDenganObstacle = 0.5f;

    [Header("Jarak Antar Terrain")]
    public float gapBetweenTerrains = 0f; // Tambahan gap antar terrain

    [Header("Gerakan Terrain")]
    public float scrollSpeed = 2f;

    [Header("Parent Grid (opsional)")]
    public Transform parent;

    private List<GameObject> activeTerrains = new List<GameObject>();
    private float timer;

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

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnRandomTerrain();
            timer = spawnInterval;
        }

        ScrollTerrains();
        CleanupTerrains();
    }

    void SpawnRandomTerrain()
    {
        bool pakaiObstacle = Random.value < chanceTerrainDenganObstacle;
        List<GameObject> listDipilih = pakaiObstacle ? tilePrefabsDenganObstacle : tilePrefabsTanpaObstacle;

        if (listDipilih == null || listDipilih.Count == 0) return;

        GameObject prefab = listDipilih[Random.Range(0, listDipilih.Count)];

        // Tambahkan gap
        spawnPosition.x += gapBetweenTerrains;

        Vector3 spawnPos = new Vector3(spawnPosition.x, 0f, spawnPosition.z);
        GameObject terrain = Instantiate(prefab, spawnPos, Quaternion.identity, parent);
        activeTerrains.Add(terrain);

        // Update posisi spawn untuk terrain berikutnya (digeser berdasarkan lebar prefab + gap)
        float width = GetPrefabWidth(prefab);
        spawnPosition.x += width;
    }

    float GetPrefabWidth(GameObject prefab)
    {
        Renderer rend = prefab.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            return rend.bounds.size.x;
        }
        return 5f; // fallback default width jika tidak ada renderer
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
