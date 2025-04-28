using System.Collections;
using UnityEngine;

/// <summary>
/// Spawns obstacles at random heights using pooling.
/// </summary>
public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private ObstaclePool pool;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float minY = -3f;
    [SerializeField] private float maxY = 5f;
    private Coroutine spawnCoroutine;

    private void OnEnable()
    {
        EventManager.OnGameStart += StartSpawning;
        EventManager.OnGameOver += StopSpawning;
    }

    private void OnDisable()
    {
        EventManager.OnGameStart -= StartSpawning;
        EventManager.OnGameOver -= StopSpawning;
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            var obstacle = pool.GetObstacle();
            obstacle.transform.position = new Vector3(transform.position.x, Random.Range(minY, maxY), 0f);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void StartSpawning()
    {
        if (spawnCoroutine == null)
            spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }
}