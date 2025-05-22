using System.Collections.Generic;
using UnityEngine;

public class RandomTilemapGenerator : MonoBehaviour
{
    [Header("Starter Terrain (tidak diacak)")]
    public GameObject[] starterPrefabs;

    [Header("Prefab Acak")]
    public GameObject[] terrainPrefabs;

    [Header("Pengaturan Gerak")]
    public float scrollSpeed = 2f;

    [Header("Awal dan Spawning")]
    public int initialPrefabs = 5;
    public float spacing = 1f;

    [Header("Parent (opsional)")]
    public Transform parent;

    private List<GameObject> activeBlocks = new List<GameObject>();
    private float nextSpawnX = 0f;
    private int starterIndex = 0;

    void Start()
    {
        for (int i = 0; i < initialPrefabs; i++)
        {
            SpawnNextBlock();
        }
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying) return;

        ScrollBlocks();
        RecycleBlocks();
    }

void SpawnNextBlock()
{
    GameObject prefab;

    // 1. Ambil prefab starter atau random
    if (starterIndex < starterPrefabs.Length)
    {
        prefab = starterPrefabs[starterIndex];
        starterIndex++;
    }
    else
    {
        if (terrainPrefabs.Length == 0) return;
        prefab = terrainPrefabs[Random.Range(0, terrainPrefabs.Length)];
    }

    // 2. Instansiasi prefab sementara di posisi (0, 0) dulu agar bisa ukur lebar
    GameObject tempBlock = Instantiate(prefab, Vector3.zero, Quaternion.identity);
    float prefabWidth = GetPrefabWidth(tempBlock);

    // 3. Pindahkan prefab ke posisi yang sesuai
    Vector3 spawnPosition = new Vector3(nextSpawnX, 0f, 0f);
    tempBlock.transform.position = spawnPosition;

    // 4. Set parent jika ada
    if (parent != null)
        tempBlock.transform.parent = parent;

    // 5. Tambahkan ke list dan perbarui next spawn X
    activeBlocks.Add(tempBlock);
    nextSpawnX = spawnPosition.x + prefabWidth + spacing;
}


    float GetPrefabWidth(GameObject obj)
    {
        var renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return 1f;

        Bounds bounds = renderers[0].bounds;
        foreach (var r in renderers)
        {
            bounds.Encapsulate(r.bounds);
        }

        return bounds.size.x;
    }

    void ScrollBlocks()
    {
        foreach (var block in activeBlocks)
        {
            if (block != null)
                block.transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
        }
    }

    void RecycleBlocks()
    {
        if (activeBlocks.Count == 0) return;

        GameObject first = activeBlocks[0];
        float blockRightEdge = first.transform.position.x + GetPrefabWidth(first);

        if (blockRightEdge < -20f)
        {
            Destroy(first);
            activeBlocks.RemoveAt(0);
            SpawnNextBlock();
        }
    }
}
