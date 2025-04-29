using System.Collections;
using UnityEngine;

/// <summary>
/// Controls a global speed multiplier for all moving objects, allowing power-up effects with smooth transitions.
/// </summary>
public class GameSpeedManager : MonoBehaviour
{
    public static GameSpeedManager Instance { get; private set; }

    // Current global multiplier
    public float SpeedMultiplier { get; private set; } = 1f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Starts a timed power-up: smooth ramp-up, hold, and ramp-down.
    /// </summary>
    /// <param name="totalDuration">Total duration of the power-up in seconds.</param>
    /// <param name="targetMultiplier">Peak speed multiplier.</param>
    public void StartPowerUp(float totalDuration, float targetMultiplier)
    {
        StopAllCoroutines();
        StartCoroutine(PowerUpRoutine(totalDuration, targetMultiplier));
    }

    private IEnumerator PowerUpRoutine(float totalDuration, float targetMultiplier)
    {
        float holdDuration = totalDuration * 0.75f;
        float transitionTime = (totalDuration - holdDuration) * 0.5f;
        float timer = 0f;

        // Phase 1: ramp up from 1 to targetMultiplier
        while (timer < transitionTime)
        {
            SpeedMultiplier = Mathf.Lerp(1f, targetMultiplier, timer / transitionTime);
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure exact peak
        SpeedMultiplier = targetMultiplier;

        // Phase 2: hold at targetMultiplier
        yield return new WaitForSeconds(holdDuration);

        // Phase 3: ramp down from targetMultiplier back to 1
        timer = 0f;
        while (timer < transitionTime)
        {
            SpeedMultiplier = Mathf.Lerp(targetMultiplier, 1f, timer / transitionTime);
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure reset
        SpeedMultiplier = 1f;
    }
}