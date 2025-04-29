using UnityEngine;

/// <summary>
/// Moves collectable left, signals miss if it passes off-screen, and returns to pool.
/// </summary>
public class Collectable : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private CollectablePool pool;
    private float offscreenX = -15f;

    private void Start()
    {
        pool = FindFirstObjectByType<CollectablePool>();
    }

    private void Update()
    {
        transform.Rotate(0, 0, -100 * Time.deltaTime);
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
        if (transform.position.x < offscreenX)
        {
            // Signal a missed collectable
            EventManager.OnCollectableMiss?.Invoke();
            pool.ReturnCollectable(gameObject);
        }
    }
}