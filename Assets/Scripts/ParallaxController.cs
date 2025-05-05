using UnityEngine;

/// <summary>
/// Simulates infinite parallax by moving repeating background layers to the left,
/// restarting their position when offscreen, respecting game start and speed power-ups.
/// </summary>
public class ParallaxController : MonoBehaviour
{
    [System.Serializable]
    public class Layer
    {
        [Tooltip("Parent transform that contains two child sprites side by side.")]
        public Transform container;

        [Range(0f, 1f)]
        [Tooltip("Relative speed factor: 0 = static, 1 = full base scroll speed.")]
        public float parallaxFactor = 0.5f;

        [Tooltip("Width of one sprite tile in world units (auto-detected if zero).")]
        public float tileWidth = 0f;
    }

    [Header("Parallax Settings")]
    [Tooltip("Base scroll speed in world units per second.")]
    [SerializeField] private float baseScrollSpeed = 2f;

    [Tooltip("Layers from back (0) to front (last). Each layer.container should have two children/sprites.")]
    [SerializeField] private Layer[] layers;

    private bool isRunning;

    private void OnEnable()
    {
        EventManager.OnGameStart += OnGameStart;
    }

    private void OnDisable()
    {
        EventManager.OnGameStart -= OnGameStart;
    }

    private void Start()
    {
        isRunning = false;

        // Auto-detect tileWidth if not set
        foreach (var layer in layers)
        {
            if (layer.container != null && layer.tileWidth <= 0f)
            {
                var sr = layer.container.GetComponentInChildren<SpriteRenderer>();
                if (sr != null)
                {
                    // Use bounds.x for world-space width
                    layer.tileWidth = sr.bounds.size.x;
                }
                else
                {
                    Debug.LogWarning($"ParallaxController: No SpriteRenderer found in layer {layer.container.name} to auto-detect tileWidth.");
                }
            }
        }
    }

    private void OnGameStart()
    {
        isRunning = true;
    }

    private void LateUpdate()
    {
        if (!isRunning) return;

        float speedMult = GameSpeedManager.Instance.SpeedMultiplier;
        float delta = baseScrollSpeed * speedMult * Time.deltaTime;

        foreach (var layer in layers)
        {
            float moveX = delta * layer.parallaxFactor;
            // Move the container left
            layer.container.position += Vector3.left * moveX;

            // If container has scrolled past one tile width, wrap it
            if (layer.container.position.x <= -layer.tileWidth)
            {
                layer.container.position += Vector3.right * layer.tileWidth;
            }
        }
    }
}
