using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private ObstaclePool pool;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float minY = -3f;
    [SerializeField] private float maxY = 5f;

    [Header("Oscillation Settings")]
    [SerializeField] private float oscillationStartDelay = 30f;   // segundos desde OnGameStart
    [SerializeField] private float oscillationChance = .2f;   // 20% de probabilidad
    [SerializeField] private float verticalSpeed = 2f;    // velocidad de oscilación

    private float gameStartTime;
    private Coroutine spawnCoroutine;

    private void OnEnable()
    {
        EventManager.OnGameStart += OnGameStart;
        EventManager.OnGameOver += StopSpawning;
    }

    private void OnDisable()
    {
        EventManager.OnGameStart -= OnGameStart;
        EventManager.OnGameOver -= StopSpawning;
    }

    private void OnGameStart()
    {
        gameStartTime = Time.time;
        StartSpawning();
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            var go = pool.GetObstacle();
            // Posición X,Y de spawn aleatorio
            float ySpawn = Random.Range(minY, maxY);
            go.transform.position = new Vector3(transform.position.x, ySpawn, 0f);

            // Decidir si este obstáculo tendrá oscilación
            bool willOscillate = Time.time - gameStartTime > oscillationStartDelay
                                 && Random.value < oscillationChance;

            var obs = go.GetComponent<Obstacle>();
            obs.ConfigureOscillation(
                enabled: willOscillate,
                minY: minY,
                maxY: maxY,
                speed: verticalSpeed
            );

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
