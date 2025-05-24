using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public List<GameObject> ObstaclePrefab; // prefab obstacle can be attach from folder of prefabs
    public BoundaryObstacleSpawner BoundaryObstacleSpawner; // BoundaryObstacleSpawner is GameObject
    private Transform obstacleSpawner; // obstacleSpawner is GameObject
    public Vector2 SpawnerPosition = new Vector2(12f, -4f); // for set position of spawner
    public float Speed = 5f; // for set speed of obstacle

    [Header("Interval Acak")]
    public float minSpawnInterval = 2f;
    public float maxSpawnInterval = 4f;

    public float DistanceXDivider = -22f; // for set the BoundaryObstacleSpawner of x position

    private bool isSetupReady = false; // for check the set up
    private float timer; // timer
    private float currentInterval; // interval saat ini

    private void Awake()
    {
        obstacleSpawner = GetComponent<Transform>();
        // just confirm if obstacle is ready
        if (ObstaclePrefab == null || obstacleSpawner == null)
        {
            isSetupReady = false;
            Debug.LogWarning("Obstacle not ready");
            return;
        }
        else
        {
            isSetupReady = true;
        }
    }

    private void Start()
    {
        if (!isSetupReady) return;
        transform.position = SpawnerPosition;
        currentInterval = Random.Range(minSpawnInterval, maxSpawnInterval); // inisialisasi awal
    }

    private void Update()
    {
        if (!isSetupReady || !GameManager.Instance.IsPlaying) return;

        transform.position = SpawnerPosition;

        timer += Time.deltaTime;
        if (timer > currentInterval)
        {
            if (ObstaclePrefab != null && ObstaclePrefab.Count > 0)
            {
                int randomIndex = Random.Range(0, ObstaclePrefab.Count);
                GameObject go = Instantiate(ObstaclePrefab[randomIndex], transform.position, Quaternion.identity);
                Obstacle obstacle = go.GetComponent<Obstacle>();
                obstacle.SetSpeed(Speed);
            }

            timer = 0f;
            currentInterval = Random.Range(minSpawnInterval, maxSpawnInterval); // interval berikutnya
        }

        // for set the x position of boundary obstacle
        BoundaryObstacleSpawner.SetXPosition(DistanceXDivider);
    }
}
