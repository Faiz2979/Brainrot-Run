using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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
    public bool isMuted => !toggleMusicActive; // Misalkan toggleMusicActive true berarti musik aktif
    public int Coins => coins;
    public float HighScore => highScore;
    public bool ToggleMusicActive => toggleMusicActive;

    // === Game UI ===
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject mainMenuUI; // Optional: jika ingin mengontrol UI Menu Utama dari GameManager
    [SerializeField] private GameObject gameUI; // Optional: jika ingin mengontrol UI Game dari GameManager

    // === Singleton Pattern ===
    [SerializeField]private AudioSource menuMusic; // Optional: jika ingin mengontrol audio dari GameManager
    [SerializeField]private AudioSource gameMusic; // Optional: jika ingin mengontrol audio dari GameManager

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Memuat ulang scene saat ini
        ScoreManager.Instance.ResetScore(); // Pastikan ScoreManager ada
        SetIsPlaying(true);
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        SetIsPlaying(false);
                    // Tambahkan tampilan Game Over UI jika perlu
        if (gameOverUI != null)
        {
            gameUI.SetActive(false); // Optional: menonaktifkan menu utama
            gameOverUI.SetActive(true);
        }
        // Simpan high score jika lebih tinggi dari yang ada
        if (coins > highScore)
        {
            SetHighScore(coins);
            Debug.Log("New High Score: " + highScore);
        }
    }



    public void ToggleMusic()
    {
        toggleMusicActive = !toggleMusicActive;

        if (toggleMusicActive)
        {
            // Aktifkan musik sesuai state permainan
            if (IsPlaying)
            {
                if (gameMusic != null)
                {
                    gameMusic.Play();
                }
                if (menuMusic != null)
                {
                    menuMusic.Stop();
                }
            }
            else
            {
                if (menuMusic != null)
                {
                    menuMusic.Play();
                }
                if (gameMusic != null)
                {
                    gameMusic.Stop();
                }
            }
        }
        else
        {
            // Matikan semua musik
            if (menuMusic != null)
            {
                menuMusic.Stop();
            }
            if (gameMusic != null)
            {
                gameMusic.Stop();
            }
        }
    }
}
