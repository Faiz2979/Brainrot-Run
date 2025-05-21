using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private bool isPlaying;
    private int coins;
    private float highScore;
    private bool toggleMusicActive;
    // add new data if needed
    public bool IsPlaying => isPlaying; // this variable can used for global
    public int Coins => coins; // this variable can used for global
    public float HighScore => highScore; // this variable can used for global
    public bool ToggleMusicActive => toggleMusicActive; // this variable can used for global
    // add new data if needed

    private void Awake()
    {
        // for make sure the GameManager is not duplicate
        if (Instance == null) Instance = this;
        else Destroy(Instance);
    }

    public void SetIsPlaying(bool SetIsPlaying) { this.isPlaying = SetIsPlaying; } // for set data, not for get data
    public void SetCoins(int SetCoins) { this.coins = SetCoins; } // for set data, not for get data
     public void SetHighScore(float SetHighScore) { this.highScore = SetHighScore; } // for set data, not for get data
    public void SetToggleMusicActive(bool SetToggleMusicActive) { this.toggleMusicActive = SetToggleMusicActive; } // for set data, not for get data
    // add new data if needed
}
