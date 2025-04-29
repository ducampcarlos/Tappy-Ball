using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    // Parámetros de oscilación
    private bool isOscillating;
    private float minY, maxY, range, verticalSpeed;
    private float timeOffset;

    private ObstaclePool pool;
    private float offscreenX = -15f;

    private void Start()
    {
        pool = FindFirstObjectByType<ObstaclePool>();
    }

    /// <summary>
    /// Llama el spawner cada vez que lo sacas del pool
    /// </summary>
    public void ConfigureOscillation(bool enabled, float minY, float maxY, float speed)
    {
        isOscillating = enabled;
        this.minY = minY;
        this.maxY = maxY;
        this.range = maxY - minY;
        this.verticalSpeed = speed;
        timeOffset = Random.value * 10f; // desfase aleatorio para no sincronizar todos
    }

    private void Update()
    {
        // 1) Movimiento horizontal constante
        float newX = transform.position.x - moveSpeed * Time.deltaTime;

        // 2) Si debe oscilar, calcula nueva Y con PingPong; si no, mantiene Y inicial
        float newY = transform.position.y;
        if (isOscillating)
        {
            float t = (Time.time + timeOffset) * verticalSpeed;
            newY = Mathf.PingPong(t, range) + minY;
        }

        transform.position = new Vector3(newX, newY, 0f);

        // 3) Devolver al pool si sale de cámara
        if (newX < offscreenX)
            pool.ReturnObstacle(gameObject);
    }
}
