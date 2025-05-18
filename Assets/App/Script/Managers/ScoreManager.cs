using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private float timer;
    private int score;
    public int Score => score;

    private bool isPlaying = false;
    [Tooltip("Interval in seconds to increase score")]
    [SerializeField] private float interval = 0.1f; // setiap 0.1 detik naik skor 1
    private float intervalTimer = 0f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (!isPlaying) return;

        intervalTimer += Time.deltaTime;

        while (intervalTimer >= interval)
        {
            intervalTimer -= interval;
            score += 1; // total +10 per detik karena 0.1s * 10 = 1s
        }
    }

    public void ResetScore()
    {
        score = 0;
        intervalTimer = 0f;
    }

    public void StartScoring()
    {
        isPlaying = true;
        ResetScore();
    }

    public void StopScoring()
    {
        isPlaying = false;
    }
}
