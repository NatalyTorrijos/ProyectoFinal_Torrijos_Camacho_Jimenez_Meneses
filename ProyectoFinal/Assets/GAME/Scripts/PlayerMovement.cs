using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 720f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private float gravity = -9.81f;

    [Header("References")]
    public Camera mainCamera;

    private CharacterController controller;
    private Animator anim;

    //Para poder escalar 
    public bool canMove = true;   

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

 
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            jumpPressed = true;
    }

    private void Update()
    {
        // Detectar suelo
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

            if (anim != null)
                anim.SetBool("isJumping", false);
        }

      
        if (!canMove)
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
            return;
        }

        
        Vector3 moveDir = Vector3.zero;

        if (mainCamera != null)
        {
            Vector3 forward = mainCamera.transform.forward;
            forward.y = 0f;
            forward.Normalize();

            Vector3 right = mainCamera.transform.right;
            right.y = 0f;
            right.Normalize();

            moveDir = right * moveInput.x + forward * moveInput.y;
        }

       
        if (moveDir.sqrMagnitude > 0.05f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

      
        controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

        // SALTO
        if (jumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpPressed = false;

            if (anim != null)
                anim.SetBool("isJumping", true);
        }

        // Gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    
        float animSpeed = new Vector3(moveDir.x, 0, moveDir.z).magnitude;
        anim.SetFloat("Speed", animSpeed);
    }


    public void OnRespawn()
    {
        moveInput = Vector2.zero;
        velocity = Vector3.zero;
        jumpPressed = false;

        if (anim != null)
        {
            anim.SetFloat("Speed", 0f);
            anim.SetBool("isJumping", false);
        }

        controller.Move(Vector3.zero);
    }
}
