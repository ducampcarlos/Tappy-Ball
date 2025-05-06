using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles user input and raises jump events.
/// </summary>
[DefaultExecutionOrder(-100)]
public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance { get; private set; }
    public event Action OnJumpPressed;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR || UNITY_WEBGL
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            OnJumpPressed?.Invoke();
#endif
#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBGL
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            OnJumpPressed?.Invoke();
#endif
    }
}