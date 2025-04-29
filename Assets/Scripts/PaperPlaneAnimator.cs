using UnityEngine;

/// <summary>
/// Adjusts the pitch and turbulence of a paper airplane model based on Rigidbody linearVelocity,
/// preserving the model’s initial local rotation as a base and applying a smooth delta on top.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PaperPlaneAnimator : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Child transform of the airplane mesh to pitch.")]
    [SerializeField] private Transform modelTransform;

    [Header("Pitch Settings")]
    [Tooltip("Maximum pitch angle (degrees) when climbing or diving.")]
    [SerializeField] private float maxPitchAngle = 30f;
    [Tooltip("How quickly the model interpolates toward the target pitch.")]
    [SerializeField] private float pitchSmoothSpeed = 5f;
    [Tooltip("Max vertical speed to normalize the pitch input.")]
    [SerializeField] private float maxVerticalSpeed = 5f;

    [Header("Turbulence Settings")]
    [Tooltip("Max random turbulence angle added to pitch.")]
    [SerializeField] private float turbulenceAmplitude = 3f;
    [Tooltip("Frequency of the Perlin-noise turbulence.")]
    [SerializeField] private float turbulenceFrequency = 1f;

    private Rigidbody rb;
    private Quaternion baseRotation;
    private float currentPitch;
    private float noiseOffset;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (modelTransform == null)
            modelTransform = transform;

        // Capture the model's original local rotation
        baseRotation = modelTransform.localRotation;

        // Random phase so multiple planes don't shake in unison
        noiseOffset = Random.value * 100f;
    }

    private void LateUpdate()
    {
        if (modelTransform == null) return;

        // 1) Read vertical speed from linearVelocity.y
        float vY = rb.linearVelocity.y;

        // 2) Normalize to [-1..1]
        float ratio = Mathf.Clamp(vY / maxVerticalSpeed, -1f, 1f);

        // 3) Compute target pitch delta (around 0)
        float targetPitch = maxPitchAngle * ratio;

        // 4) Smoothly interpolate our currentPitch toward that target
        currentPitch = Mathf.Lerp(currentPitch, targetPitch, Time.deltaTime * pitchSmoothSpeed);

        // 5) Add Perlin noise–based turbulence
        float noise = (Mathf.PerlinNoise(Time.time * turbulenceFrequency, noiseOffset) - 0.5f)
                      * 2f * turbulenceAmplitude;

        // 6) Build a small rotation around the X axis
        Quaternion deltaRot = Quaternion.Euler(currentPitch + noise, 0f, 0f);

        // 7) Apply it on top of the original baseRotation
        modelTransform.localRotation = baseRotation * deltaRot;
    }
}
