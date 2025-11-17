using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;
    public float distance = 6f;
    public float height = 3f;
    public float rotationSmoothTime = 0.1f;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 120f;
    public Vector2 pitchLimits = new Vector2(-30f, 60f);

    private float yaw;
    private float pitch;
    private Vector3 currentRotation;
    private Vector3 rotationSmoothVelocity;

    private PlayerInput playerInput;
    private InputAction lookAction;

    private void Start()
    {
        if (target == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                target = playerObj.transform;
        }

        playerInput = FindAnyObjectByType<PlayerInput>();
        if (playerInput != null)
        {
            if (playerInput.actions.FindAction("Look") != null)
                lookAction = playerInput.actions.FindAction("Look");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (!target) return;

        Vector2 lookInput = Vector2.zero;

        if (lookAction != null)
        {
            lookInput = lookAction.ReadValue<Vector2>();
        }
        else if (Mouse.current != null)
        {
            lookInput.x = Mouse.current.delta.x.ReadValue();
            lookInput.y = Mouse.current.delta.y.ReadValue();
        }

        yaw += lookInput.x * mouseSensitivity * Time.deltaTime;
        pitch -= lookInput.y * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);

        Vector3 targetRotation = new Vector3(pitch, yaw);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);

        transform.eulerAngles = currentRotation;

        Vector3 focusPoint = target.position + Vector3.up * height;
        Vector3 targetPosition = focusPoint - transform.forward * distance;
        transform.position = targetPosition;

        transform.LookAt(focusPoint);
    }
}
