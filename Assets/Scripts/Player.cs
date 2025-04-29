using UnityEngine;

/// <summary>
/// Applies jump physics and triggers game events on collisions.
/// Waits for first input to start the game.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;
    private Rigidbody rb;
    private bool gameStarted;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gameStarted = false;
        rb.isKinematic = true; // Disable physics until game starts
    }

    private void OnEnable()
    {
        InputHandler.Instance.OnJumpPressed += HandleJump;
    }

    private void OnDisable()
    {
        if (InputHandler.Instance != null)
            InputHandler.Instance.OnJumpPressed -= HandleJump;
    }

    private void HandleJump()
    {
        if (!gameStarted)
        {
            GameManager.Instance.StartGame();
            gameStarted = true;
            rb.isKinematic = false; // Enable physics when game starts
        }
        rb.linearVelocity = Vector3.up * jumpForce;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pipe"))
            EventManager.OnGameOver?.Invoke();
        else if (other.CompareTag("ScoreCheck"))
            EventManager.OnScore?.Invoke();
        else if (other.CompareTag("Collectable"))
        {
            EventManager.OnCollect?.Invoke();
            other.gameObject.SetActive(false);
        }
    }
}