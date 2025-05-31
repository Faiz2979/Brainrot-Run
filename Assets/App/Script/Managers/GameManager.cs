using System.Collections;
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

    // === Global Accessors ===
    public bool IsPlaying => isPlaying;
    public bool isMuted => !toggleMusicActive;
    public int Coins => coins;
    public float HighScore => highScore;
    public bool ToggleMusicActive => toggleMusicActive;

    // === UI References ===
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject gameUI;

    // === AudioSources (NO AudioClips) ===
    [SerializeField] private AudioSource menuMusicSource;
    [SerializeField] private AudioSource gameMusicSource;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Setup music source defaults
            if (menuMusicSource != null)
            {
                menuMusicSource.loop = true;
                menuMusicSource.playOnAwake = false;
            }

            if (gameMusicSource != null)
            {
                gameMusicSource.loop = true;
                gameMusicSource.playOnAwake = false;
            }

            PlayMenuMusic();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // === Game State Setters ===
    public void SetIsPlaying(bool value)
    {
        isPlaying = value;

        if (value)
        {
            PlayGameMusic();
        }
        else
        {
            PlayMenuMusic();
        }
    }

    public void SetCoins(int value) => coins = value;
    public void SetHighScore(float value) => highScore = value;
    public void SetToggleMusicActive(bool value)
    {
        toggleMusicActive = value;

        if (menuMusicSource != null) menuMusicSource.mute = !value;
        if (gameMusicSource != null) gameMusicSource.mute = !value;
    }

    // === Game Flow ===
    public void PlayGame()
    {
        Debug.Log("Play Game");
        SetIsPlaying(true);
    }

    public void OpenShop()
    {
        Debug.Log("Open Shop");
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }
        SetIsPlaying(true);
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        SetIsPlaying(false);

        if (gameOverUI != null)
        {
            if (gameUI != null) gameUI.SetActive(false);
            gameOverUI.SetActive(true);
        }

        if (coins > highScore)
        {
            SetHighScore(coins);
            Debug.Log("New High Score: " + highScore);
        }
    }

    // === Music Control ===
    private void PlayMenuMusic()
    {
        if (menuMusicSource != null && !menuMusicSource.isPlaying)
            menuMusicSource.Play();

        if (gameMusicSource != null && gameMusicSource.isPlaying)
            gameMusicSource.Stop();
    }

    private void PlayGameMusic()
    {
        if (gameMusicSource != null && !gameMusicSource.isPlaying)
            gameMusicSource.Play();

        if (menuMusicSource != null && menuMusicSource.isPlaying)
            menuMusicSource.Stop();
    }
}
