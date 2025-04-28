using UnityEngine;

/// <summary>
/// Moves collectable left and returns to pool on off-screen.
/// </summary>
public class Collectable : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private CollectablePool pool;

    private void Start()
    {
        pool = FindFirstObjectByType<CollectablePool>();
    }

    private void Update()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
        if (transform.position.x < -15f)
            pool.ReturnCollectable(gameObject);
    }
}