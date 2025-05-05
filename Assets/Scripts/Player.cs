using System.Collections;
using UnityEngine;

/// <summary>
/// Applies jump physics and handles collisions, including power-up invincibility against obstacles.
/// Waits for first input to start the game.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;
    private Rigidbody rb;
    private bool gameStarted;

    // Power-up invincibility
    private bool canPassThrough = false;
    private Coroutine passThroughRoutine;

    [SerializeField] private AudioClip[] paperJump;
    [SerializeField] private AudioClip collectableSound;

    [Header("Power-Up Indicator")]
    [SerializeField] private Light powerUpLight;

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

        AudioManager.Instance.PlaySFX(paperJump[Random.Range(0, paperJump.Length)], 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pipe"))
        {
            if (!canPassThrough)
                EventManager.OnGameOver?.Invoke();
        }
        else if (other.CompareTag("Boundary"))
        {
            EventManager.OnGameOver?.Invoke();
        }
        else if (other.CompareTag("ScoreCheck"))
            EventManager.OnScore?.Invoke();
        else if (other.CompareTag("Collectable"))
        {
            EventManager.OnCollect?.Invoke();
            AudioManager.Instance.PlaySFX(collectableSound, 0.5f);
            other.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Enables temporary invincibility to pass through obstacles.
    /// </summary>
    public void EnablePassThrough(float duration)
    {
        if (passThroughRoutine != null)
            StopCoroutine(passThroughRoutine);
        passThroughRoutine = StartCoroutine(PassThroughRoutine(duration));
    }

    private IEnumerator PassThroughRoutine(float duration)
    {
        canPassThrough = true;
        yield return new WaitForSeconds(duration);
        canPassThrough = false;
    }

    // Contador de power-ups activos que piden la luz
    private int lightRequestCount = 0;

    /// <summary>
    /// Enciende la luz con el color dado durante `duration` segundos,
    /// y se apaga s�lo cuando **todos** los power-ups hayan terminado.
    /// </summary>
    public void ShowPowerUpLight(Color color, float duration)
    {
        if (powerUpLight == null) return;

        lightRequestCount++;
        powerUpLight.color = color;
        powerUpLight.enabled = true;
        StartCoroutine(PowerUpLightRoutine(duration));
    }

    private IEnumerator PowerUpLightRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        lightRequestCount--;
        if (lightRequestCount <= 0)
        {
            // No quedan requests, apagamos
            lightRequestCount = 0;
            powerUpLight.enabled = false;
        }
    }
}


