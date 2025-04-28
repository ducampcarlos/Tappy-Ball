using System.Collections;
using UnityEngine;

/// <summary>
/// Spawns collectables at random intervals and heights.
/// </summary>
public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] private CollectablePool pool;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float minY = -2f;
    [SerializeField] private float maxY = 4f;
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
            var collectable = pool.GetCollectable();
            collectable.transform.position = new Vector3(transform.position.x, Random.Range(minY, maxY), 0f);
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