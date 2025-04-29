using UnityEngine;
using TMPro;

/// <summary>
/// Tracks and displays the player's score and combo for collectables.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;

    private int score;
    private int comboCount;

    private void Awake()
    {
        ResetAll();
    }

    private void OnEnable()
    {
        EventManager.OnGameStart += ResetAll;
        EventManager.OnScore += OnScore;
        EventManager.OnCollect += OnCollect;
        EventManager.OnCollectableMiss += OnCollectableMiss;
    }

    private void OnDisable()
    {
        EventManager.OnGameStart -= ResetAll;
        EventManager.OnScore -= OnScore;
        EventManager.OnCollect -= OnCollect;
        EventManager.OnCollectableMiss -= OnCollectableMiss;
    }

    private void ResetAll()
    {
        score = 0;
        comboCount = 0;
        UpdateUI();
    }

    private void OnScore()
    {
        // Increment score for passing pipe; do not reset combo
        score++;
        UpdateUI();
    }

    private void OnCollect()
    {
        // Successful collectable pickup: increase combo and add combo bonus
        comboCount++;
        score += comboCount;
        UpdateUI();
    }

    private void OnCollectableMiss()
    {
        // Missed collectable: reset combo without affecting score
        comboCount = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
        if (comboText != null)
            comboText.text = comboCount > 1 ? $"Combo x{comboCount}" : string.Empty;
    }
}