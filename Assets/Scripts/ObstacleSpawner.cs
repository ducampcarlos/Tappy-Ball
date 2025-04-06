using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] float spawnInterval = 2f;
    [SerializeField] float spawnRangePositiveY = 5f;
    [SerializeField] float spawnRangeNegativeY = -3f;

    Coroutine spawnCoroutine;

    void Spawn()
    {
        Instantiate(obstaclePrefab, new Vector3(transform.position.x, Random.Range(spawnRangeNegativeY, spawnRangePositiveY), 0), Quaternion.identity);
    }

    IEnumerator StartSpawning()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void StartSpawn()
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(StartSpawning());
        }

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
