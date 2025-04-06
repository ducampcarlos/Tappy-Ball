using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    int score;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] ObstacleSpawner obstacleSpawner;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartGame();
    }

    public void ScoreUp()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void StartGame()
    {
        score = 0;
        scoreText.text = score.ToString();
        obstacleSpawner.StartSpawn();
    }

    public void StopGame()
    {
        obstacleSpawner.StopSpawning();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
