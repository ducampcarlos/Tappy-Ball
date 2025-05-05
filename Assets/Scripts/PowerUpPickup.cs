using UnityEngine;

/// <summary>
/// Handles pickup of a power-up: triggers speed modifier, enables invincibility, and returns to pool.
/// </summary>
public class PowerUpPickup : MonoBehaviour
{
    [SerializeField] private float totalDuration = 10f;
    [SerializeField] private float speedUpMultiplier = 2f;
    [SerializeField] private AudioClip pickUpSound;

    private PowerUpPool pool;

    private void Start()
    {
        pool = FindFirstObjectByType<PowerUpPool>();
    }

    private void Update()
    {
        float globalSpeed = GameSpeedManager.Instance.SpeedMultiplier;
        transform.Translate(Vector3.left * 5f * Time.deltaTime * globalSpeed, Space.World);

        if (transform.position.x < -15f)
        {
            gameObject.SetActive(false);
            pool.ReturnPowerUp(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Trigger global smooth power-up
        GameSpeedManager.Instance.StartPowerUp(totalDuration, speedUpMultiplier);

        AudioManager.Instance.PlaySFX(pickUpSound, 0.5f);
        // Enable player invincibility against obstacles
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            player.EnablePassThrough(totalDuration);
            player.ShowPowerUpLight(Color.cyan, totalDuration);
        }

        // Deactivate and return to pool
        gameObject.SetActive(false);
        pool.ReturnPowerUp(gameObject);
    }
}
