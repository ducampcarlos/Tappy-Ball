using UnityEngine;

/// <summary>
/// Moves collectable left normally, but if magnet is active and within radius, it moves toward the player.
/// </summary>
public class Collectable : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private CollectablePool pool;
    private float offscreenX = -15f;

    // Magnet state
    private bool magnetActive;
    private float magnetRadius;
    private Transform playerTransform;

    private void Start()
    {
        pool = FindFirstObjectByType<CollectablePool>();
        playerTransform = FindFirstObjectByType<Player>().transform;
        EventManager.OnMagnetStart += OnMagnetStart;
        EventManager.OnMagnetEnd += OnMagnetEnd;
    }

    private void OnDestroy()
    {
        EventManager.OnMagnetStart -= OnMagnetStart;
        EventManager.OnMagnetEnd -= OnMagnetEnd;
    }

    private void OnMagnetStart(float radius)
    {
        magnetActive = true;
        magnetRadius = radius;
    }

    private void OnMagnetEnd()
    {
        magnetActive = false;
    }

    private void Update()
    {
        transform.Rotate(0, 90 * Time.deltaTime, 0); // Rotate collectable for visual effect

        float globalSpeed = GameSpeedManager.Instance.SpeedMultiplier;
        Vector3 pos = transform.position;

        if (magnetActive && Vector3.Distance(pos, playerTransform.position) <= magnetRadius)
        {
            // Move toward player with speed
            transform.position = Vector3.MoveTowards(
                pos,
                playerTransform.position,
                moveSpeed * globalSpeed * Time.deltaTime
            );
        }
        else
        {
            // Normal leftward movement
            transform.Translate(Vector3.left * moveSpeed * globalSpeed * Time.deltaTime, Space.World);
        }

        if (transform.position.x < offscreenX)
        {
            EventManager.OnCollectableMiss?.Invoke();
            pool.ReturnCollectable(gameObject);
        }
    }
}
