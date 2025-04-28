using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages game start, game over, and scene reload.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        EventManager.OnGameOver += RestartGame;
    }

    private void OnDisable()
    {
        EventManager.OnGameOver -= RestartGame;
    }

    private void Start()
    {
        StartGame();
    }

    /// <summary>
    /// Triggers game start events.
    /// </summary>
    public void StartGame()
    {
        EventManager.OnGameStart?.Invoke();
    }

    /// <summary>
    /// Restarts the current scene on game over.
    /// </summary>
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}