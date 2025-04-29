using System.Collections;
using UnityEngine;

/// <summary>
/// Spawns power-ups occasionally based on chance.
/// </summary>
public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private PowerUpPool pool;
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField, Range(0f, 1f)] private float spawnChance = 0.3f;
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
            yield return new WaitForSeconds(spawnInterval);
            if (Random.value <= spawnChance)
            {
                var pu = pool.GetPowerUp();
                float y = Random.Range(minY, maxY);
                pu.transform.position = new Vector3(transform.position.x, y, 0f);
            }
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