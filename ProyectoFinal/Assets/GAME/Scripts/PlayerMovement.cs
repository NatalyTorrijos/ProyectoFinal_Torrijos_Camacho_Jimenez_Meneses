using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 720f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 2.0f;   // altura del salto
    [SerializeField] private float gravity = -9.81f;    // gravedad

    [Header("References")]
    public Camera mainCamera;

    private CharacterController controller;
    private Animator anim;

    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isGrounded;
    private bool jumpPressed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    // ✅ Input System callbacks
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) // solo cuando se presiona
            jumpPressed = true;
    }

    private void Update()
    {
        // Verificar si está tocando el suelo
        isGrounded = controller.isGrounded;

        // Reiniciar velocidad vertical si está en el suelo
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Calcular dirección de movimiento (relativa a cámara)
        Vector3 moveDir = Vector3.zero;
        if (mainCamera != null)
        {
            Vector3 camForward = mainCamera.transform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            Vector3 camRight = mainCamera.transform.right;
            camRight.y = 0f;
            camRight.Normalize();

            moveDir = camRight * moveInput.x + camForward * moveInput.y;
        }
        else
        {
            moveDir = new Vector3(moveInput.x, 0f, moveInput.y);
        }

        // Rotar hacia donde se mueve
        if (moveDir.sqrMagnitude > 0.05f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        // Mover horizontalmente
        controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

        // Salto
        if (jumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpPressed = false;
        }

        // Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // ============================================================
    // 📍 MÉTODO PARA REINICIAR ESTADOS TRAS UN RESPAWN
    // ============================================================
    public void OnRespawn()
    {
        // Reiniciar inputs y movimiento vertical
        moveInput = Vector2.zero;
        velocity = Vector3.zero;
        jumpPressed = false;

        // Reiniciar animaciones si hay un Animator
        if (anim != null)
        {
            anim.SetFloat("Speed", 0f);
            anim.SetBool("isJumping", false);
        }

        // Asegurar que el CharacterController esté en estado estable
        if (controller != null)
        {
            controller.Move(Vector3.zero);
        }

        Debug.Log("🔄 PlayerMovement reiniciado tras respawn.");
    }
}
