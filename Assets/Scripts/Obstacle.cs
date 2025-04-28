using UnityEngine;

/// <summary>
/// Moves obstacle left and returns it to the pool when off-screen.
/// </summary>
public class Obstacle : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private ObstaclePool pool;

    private void Start()
    {
        pool = FindFirstObjectByType<ObstaclePool>();
    }

    private void Update()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
        if (transform.position.x < -15f)
            pool.ReturnObstacle(gameObject);
    }
}