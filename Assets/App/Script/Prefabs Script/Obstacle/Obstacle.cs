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
        if (speed <= 0) return;
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
    }
}
