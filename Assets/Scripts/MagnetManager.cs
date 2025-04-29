using System.Collections;
using UnityEngine;

/// <summary>
/// Manages activation and duration of the magnet power-up, broadcasting start/end events.
/// </summary>
public class MagnetManager : MonoBehaviour
{
    public static MagnetManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Activates magnet for `duration` seconds with given `radius`.
    /// </summary>
    public void StartMagnet(float duration, float radius)
    {
        StopAllCoroutines();
        StartCoroutine(MagnetRoutine(duration, radius));
    }

    private IEnumerator MagnetRoutine(float duration, float radius)
    {
        EventManager.OnMagnetStart?.Invoke(radius);
        yield return new WaitForSeconds(duration);
        EventManager.OnMagnetEnd?.Invoke();
    }
}