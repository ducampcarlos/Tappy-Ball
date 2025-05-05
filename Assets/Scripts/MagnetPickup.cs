using UnityEngine;

/// <summary>
/// Pickup that triggers the magnet effect when collected by the player.
/// </summary>
[RequireComponent(typeof(Collider))]
public class MagnetPickup : MonoBehaviour
{
    [SerializeField] private float magnetDuration = 5f;
    [SerializeField] private float magnetRadius = 3f;
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
        MagnetManager.Instance.StartMagnet(magnetDuration, magnetRadius);
        var player = other.GetComponent<Player>();
        if (player != null)
            player.ShowPowerUpLight(Color.magenta, magnetDuration);
        gameObject.SetActive(false);
        pool.ReturnPowerUp(gameObject);
    }
}