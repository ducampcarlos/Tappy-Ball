using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public bool IsPressed { get; private set; }
    Rigidbody rb;
    [SerializeField] float jumpForce = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        IsPressed = false;

#if UNITY_ANDROID || UNITY_EDITOR
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            IsPressed = true;
        }
#endif
#if UNITY_STANDALONE || UNITY_EDITOR
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            IsPressed = true;
        }
#endif

        if (IsPressed)
        {
            rb.linearVelocity = Vector3.up * jumpForce;
        }
    }
}
