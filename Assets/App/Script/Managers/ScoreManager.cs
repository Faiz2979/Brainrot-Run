using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; } // sinleton ScoreManager
    private float timer; // timer based on second
    private int score; // this variable can be used only here
    public int Score => score; // this variable for global used only for method get

    private void Awake()
    {
        // for make sure that ScoreManager just have one in this scene
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        // score will increase 1 for every 1 second
        timer += Time.deltaTime;
        if (timer > 1f)
        {
            timer -= 1f;
            score++;
        }
    }

    public void ResetScore()
    {
        score = 0;
    }   
}
