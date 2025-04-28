using UnityEngine;

/// <summary>
/// Applies jump physics and triggers game events on collisions.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        rb.linearVelocity = Vector3.up * jumpForce;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pipe"))
            EventManager.OnGameOver?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ScoreCheck"))
            EventManager.OnScore?.Invoke();
    }
}