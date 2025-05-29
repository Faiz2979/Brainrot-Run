using UnityEngine;
using TMPro;

public class CoinsManager : MonoBehaviour
{
    public static CoinsManager Instance { get; private set; } // Singleton pattern

    [SerializeField] private int _totalCoins;
    [SerializeField] private TMP_Text _coinText;

    public int TotalCoins
    {
        get => _totalCoins;
        private set
        {
            _totalCoins = Mathf.Max(0, value); // Pastikan tidak negatif
            UpdateCoinUI();
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void AddCoins(int amount)
    {
        if (amount > 0)
            TotalCoins += amount;
    }

    public bool SpendCoins(int amount)
    {
        if (amount <= 0 || TotalCoins < amount)
            return false;

        TotalCoins -= amount;
        return true;
    }

    private void UpdateCoinUI()
    {
        if (_coinText != null)
            _coinText.text = $" {TotalCoins}";
    }

    // Untuk save/load (opsional)
    public void SaveCoins() => PlayerPrefs.SetInt("PlayerCoins", TotalCoins);
    public void LoadCoins() => TotalCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
}