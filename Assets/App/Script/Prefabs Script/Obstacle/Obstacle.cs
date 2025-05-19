using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // just confirm if rb is ready
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody2D not attachment");
            return;
        }
    }

    private void FixedUpdate()
    {
        if (speed <= 0 || !GameManager.Instance.IsPlaying) return;
        // for move the obstacle to left side
        Vector2 targetPos = speed * Time.fixedDeltaTime * Vector2.left;
        rb.MovePosition(rb.position + targetPos);
    }

    public void SetSpeed(float Speed)
    {
        // the Speed is from the ObstacleSpawner
        speed = Speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // for destroy the obstacle if touch a BoundaryObstacleSpawner
        if (collision.collider.CompareTag("BoundaryObstacleSpawner"))
        {
            Destroy(gameObject);
        }
        
        else if (collision.collider.CompareTag("Player"))
        {
            // for destroy the obstacle if touch a Player
            Destroy(gameObject);
            Debug.Log("Obstacle Collide with Player");
            // for stop the game
            // GameManager.Instance.SetIsPlaying(false);
        }
    }
}
