using UnityEngine;
using TMPro;

/// <summary>
/// Tracks and displays the player's score.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score;

    private void OnEnable()
    {
        EventManager.OnGameStart += ResetScore;
        EventManager.OnScore += IncrementScore;
    }

    private void OnDisable()
    {
        EventManager.OnGameStart -= ResetScore;
        EventManager.OnScore -= IncrementScore;
    }

    private void ResetScore()
    {
        score = 0;
        UpdateUI();
    }

    private void IncrementScore()
    {
        score++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }
}