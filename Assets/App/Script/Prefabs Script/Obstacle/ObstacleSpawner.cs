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
    private bool isSetupReady = false; // for check the set up
    private float timer; // timer
    public float SpawnInterval = 3f; // for tell the spawner when obstacle will spawn
    public float DistanceXDivider = -22f; // for set the BoundaryObstacleSpawner of x position

    private void Awake()
    {
        obstacleSpawner = GetComponent<Transform>();
        // just confirm if obstacle is ready
        if (ObstaclePrefab == null || obstacleSpawner == null)
        {
            isSetupReady = false;
            Debug.LogWarning("obstacle not ready");
            return;
        }
        else
        {
            isSetupReady = !isSetupReady;
        }
    }

    private void Start()
    {
        if (!isSetupReady) return;
        transform.position = SpawnerPosition;
    }

    private void Update()
    {
        if (!isSetupReady || !GameManager.Instance.IsPlaying) return;
        transform.position = SpawnerPosition;
        // for set the obstacle when it spawn
        timer += Time.deltaTime;
        if (timer > SpawnInterval)
        {
            if (ObstaclePrefab != null && ObstaclePrefab.Count > 0)
            {
                int randomIndex = Random.Range(0, ObstaclePrefab.Count);
                GameObject go = Instantiate(ObstaclePrefab[randomIndex], transform.position, Quaternion.identity);
                Obstacle obstacle = go.GetComponent<Obstacle>();
                obstacle.SetSpeed(Speed);
            }

            timer -= SpawnInterval;
        }
        // for set the x position of boundary obstacle
        BoundaryObstacleSpawner.SetXPosition(DistanceXDivider);
    }
}
