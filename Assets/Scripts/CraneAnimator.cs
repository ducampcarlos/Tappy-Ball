using UnityEngine;

/// <summary>
/// Adds gentle bobbing and rocking to a child model (origami crane) using LeanTween,
/// without moving the parent transform. 
/// For side-view: vertical bob and tilt around Z axis (beak up/down).
/// </summary>
public class CraneAnimator : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Child transform of the crane mesh to animate visually.")]
    [SerializeField] private Transform modelTransform;

    [Header("Bob Settings")]
    [Tooltip("Vertical bob amount relative to initial local position.")]
    [SerializeField] private float bobAmplitude = 0.2f;
    [Tooltip("Seconds it takes to complete one bob cycle (up and down).")]
    [SerializeField] private float bobDuration = 1.5f;

    [Header("Tilt Settings")]
    [Tooltip("Tilt angle around local Z axis (degrees) for beak up/down.")]
    [SerializeField] private float tiltAngle = 10f;
    [Tooltip("Seconds it takes to complete one tilt cycle (down/up).")]
    [SerializeField] private float tiltDuration = 2f;

    private Vector3 initialLocalPos;
    private Vector3 initialLocalEuler;

    private void Start()
    {
        if (modelTransform == null)
        {
            Debug.LogWarning("CraneAnimator: modelTransform not assigned. Disabling animator.");
            enabled = false;
            return;
        }

        // Cache starting local position and rotation
        initialLocalPos = modelTransform.localPosition;
        initialLocalEuler = modelTransform.localEulerAngles;

        // Bob: vertical ping-pong on local Y
        Vector3 bobUp = initialLocalPos + Vector3.up * bobAmplitude;
        LeanTween.moveLocal(modelTransform.gameObject, bobUp, bobDuration * 0.5f)
            .setEaseInOutSine()
            .setLoopPingPong();

        // Tilt: rotate around local Z to tilt beak up and down
        Vector3 tiltTarget = initialLocalEuler + new Vector3(0f, 0f, tiltAngle);
        LeanTween.rotateLocal(modelTransform.gameObject, tiltTarget, tiltDuration * 0.5f)
            .setEaseInOutSine()
            .setLoopPingPong();
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.PingPong(Time.time * 25, 30f) - 15f);
    }
}
