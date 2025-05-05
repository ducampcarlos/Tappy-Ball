using UnityEngine;

/// <summary>
/// Adds a pendulum-like swing to a power-up model using LeanTween,
/// preserving the initial rotation as the pivot and rotating only.
/// </summary>
public class PowerUpAnimator : MonoBehaviour
{
    [Tooltip("Child transform of the power-up model to animate.")]
    [SerializeField] private Transform modelTransform;

    [Header("Swing Settings")]
    [Tooltip("Maximum swing angle (degrees) from the initial rotation.")]
    [SerializeField] private float swingAngle = 15f;
    [Tooltip("Time in seconds for one swing half-cycle (to one side).")]
    [SerializeField] private float swingDuration = 1f;

    private Quaternion initialRotation;

    private void Start()
    {
        if (modelTransform == null)
        {
            Debug.LogWarning("PowerUpAnimator: modelTransform not assigned. Disabling animator.");
            enabled = false;
            return;
        }

        // Cache the initial local rotation
        initialRotation = modelTransform.localRotation;

        // Calculate target rotations
        Vector3 axis = Vector3.forward; // swing around Z axis
        Quaternion targetPlus = initialRotation * Quaternion.Euler(axis * swingAngle);
        Quaternion targetMinus = initialRotation * Quaternion.Euler(axis * -swingAngle);

        // Start swinging with LeanTween
        LeanTween.rotateLocal(modelTransform.gameObject, targetPlus.eulerAngles, swingDuration)
            .setEaseInOutSine()
            .setLoopPingPong()
            .setOnComplete(() => {
                // Loop back to minus
                LeanTween.rotateLocal(modelTransform.gameObject, targetMinus.eulerAngles, swingDuration)
                    .setEaseInOutSine()
                    .setLoopPingPong();
            });
    }
}
