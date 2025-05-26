using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    // === Game State Data ===
    private bool isPlaying;
    private int coins;
    private float highScore;
    private bool toggleMusicActive;

    // === Getter untuk global akses ===
    public bool IsPlaying => isPlaying;
    public int Coins => coins;
    public float HighScore => highScore;
    public bool ToggleMusicActive => toggleMusicActive;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: if you want it to persist
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // === Game State Setter ===
    public void SetIsPlaying(bool value) { isPlaying = value; }
    public void SetCoins(int value) { coins = value; }
    public void SetHighScore(float value) { highScore = value; }
    public void SetToggleMusicActive(bool value) { toggleMusicActive = value; }

    // === Game Flow ===
    public void PlayGame()
    {
        Debug.Log("Play Game");
        SetIsPlaying(true);
    }

    public void OpenShop()
    {
        Debug.Log("Open Shop");
        // Tambahkan logika membuka shop UI
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void PauseGame()
    {
        Debug.Log("Pause Game");
        SetIsPlaying(false);
    }

    public void ResumeGame()
    {
        Debug.Log("Resume Game");
        SetIsPlaying(true);
    }

    public void RestartGame()
    {
        Debug.Log("Restart Game");
        SetIsPlaying(false);
        ScoreManager.Instance.ResetScore(); // Pastikan ScoreManager ada
        SetIsPlaying(true);
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        SetIsPlaying(false);
        // Tambahkan tampilan Game Over UI jika perlu
    }

    public void toMenu(){
        Debug.Log("Back to Menu");
        SetIsPlaying(false);
        // Tambahkan logika untuk kembali ke menu utama
    }
}
