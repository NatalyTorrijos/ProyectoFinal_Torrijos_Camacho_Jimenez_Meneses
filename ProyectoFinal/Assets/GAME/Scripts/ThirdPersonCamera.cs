using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;                 // Objetivo (el jugador)
    public float distance = 6f;              // Distancia de la cámara
    public float height = 3f;                // Altura de la cámara
    public float rotationSmoothTime = 0.1f;  // Suavidad del movimiento

    [Header("Mouse Settings")]
    public float mouseSensitivity = 120f;    // Sensibilidad de mouse
    public Vector2 pitchLimits = new Vector2(-30f, 60f);

    private float yaw;                       // Rotación horizontal
    private float pitch;                     // Rotación vertical
    private Vector3 currentRotation;
    private Vector3 rotationSmoothVelocity;

    private PlayerInput playerInput;
    private InputAction lookAction;

    private void Start()
    {
        // Si no hay objetivo, buscar al Player automáticamente
        if (target == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                target = playerObj.transform;
        }

        // Obtener sistema de inputs
        playerInput = FindAnyObjectByType<PlayerInput>();
        if (playerInput != null)
        {
            if (playerInput.actions.FindAction("Look") != null)
                lookAction = playerInput.actions.FindAction("Look");
        }

        // Bloquear y ocultar cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (!target) return;

        Vector2 lookInput = Vector2.zero;

        // Leer input de cámara
        if (lookAction != null)
        {
            lookInput = lookAction.ReadValue<Vector2>();
        }
        else if (Mouse.current != null)
        {
            lookInput.x = Mouse.current.delta.x.ReadValue();
            lookInput.y = Mouse.current.delta.y.ReadValue();
        }

        // Movimiento del mouse → rotación de cámara
        yaw += lookInput.x * mouseSensitivity * Time.deltaTime;
        pitch -= lookInput.y * mouseSensitivity * Time.deltaTime;

        // Limitar vertical
        pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);

        // Suavizado
        Vector3 targetRotation = new Vector3(pitch, yaw);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);

        transform.eulerAngles = currentRotation;

        // Posicionar cámara detrás del jugador
        Vector3 focusPoint = target.position + Vector3.up * height;
        Vector3 targetPosition = focusPoint - transform.forward * distance;

        transform.position = targetPosition;

        // Mirar hacia el jugador
        transform.LookAt(focusPoint);
    }
}
