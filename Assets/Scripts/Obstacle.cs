using UnityEngine;

/// <summary>
/// Moves obstacle left and optionally oscillates vertically.
/// Uses GameSpeedManager.SpeedMultiplier.
/// </summary>
public class Obstacle : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    // Oscillation parameters
    private bool isOscillating;
    private float minY, maxY, range, verticalSpeed;
    private float timeOffset;

    private ObstaclePool pool;
    private float offscreenX = -15f;

    private void Start()
    {
        pool = FindFirstObjectByType<ObstaclePool>();
    }

    public void ConfigureOscillation(bool enabled, float minY, float maxY, float speed)
    {
        isOscillating = enabled;
        this.minY = minY;
        this.maxY = maxY;
        this.range = maxY - minY;
        this.verticalSpeed = speed;
        timeOffset = Random.value * 10f;
    }

    private void Update()
    {
        float globalSpeed = GameSpeedManager.Instance.SpeedMultiplier;

        // Horizontal movement
        float newX = transform.position.x - moveSpeed * Time.deltaTime * globalSpeed;

        // Vertical oscillation
        float newY = transform.position.y;
        if (isOscillating)
        {
            float t = (Time.time + timeOffset) * verticalSpeed;
            newY = Mathf.PingPong(t, range) + minY;
        }

        transform.position = new Vector3(newX, newY, 0f);

        // Return to pool if off-screen
        if (newX < offscreenX)
            pool.ReturnObstacle(gameObject);
    }
}