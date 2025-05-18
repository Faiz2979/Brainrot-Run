using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryObstacleSpawner : MonoBehaviour
{
    private float xPosition;

    private void Start()
    {
        transform.localPosition = new Vector2(xPosition, 0);
    }

    private void Update()
    {
        transform.localPosition = new Vector2(xPosition, 0);
    }

    public void SetXPosition(float XPosition)
    {
        // the XPosition is from the ObstacleSpawner
        xPosition = XPosition;
    }
}
