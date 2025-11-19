using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

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

    public bool canMove = true;

    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isGrounded;
    private bool jumpPressed;

    // ===========================================================
    // 🔥 SISTEMA DE VIDA DEL PLAYER
    // ===========================================================
    [Header("----- PLAYER HEALTH -----")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("----- UI -----")]
    public Image healthFill;
    public CanvasGroup damageFlash;
    public TextMeshProUGUI hpText;

    bool isDead = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        if (mainCamera == null)
            mainCamera = Camera.main;

        currentHealth = maxHealth;

        if (healthFill != null)
            healthFill.fillAmount = 1f;

        if (hpText != null)
            hpText.text = maxHealth.ToString();
    }

    // ===========================
    // INPUT SYSTEM
    // ===========================
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
        if (isDead) return;

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
            anim.SetFloat("Speed", 0f);
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

        if (jumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpPressed = false;

            if (anim != null)
                anim.SetBool("isJumping", true);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        float animSpeed = new Vector3(moveDir.x, 0, moveDir.z).magnitude;
        anim.SetFloat("Speed", animSpeed);
    }

    // ===========================================================
    // 🔥 EMPUJE DE BLOQUES
    // ===========================================================
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Plataforma p = hit.collider.GetComponent<Plataforma>();

        if (p != null)
        {
            p.TocarPlataforma();
        }

        PushableBlock pushable = hit.collider.GetComponent<PushableBlock>();
        if (pushable == null) return;

        if (moveInput.sqrMagnitude < 0.1f) return;

        Vector3 camForward = mainCamera.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = mainCamera.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 pushDir = (camRight * moveInput.x + camForward * moveInput.y).normalized;
        pushDir.y = 0f;

        pushable.Push(pushDir);
    }

    // ===========================================================
    // RECIBIR DAÑO DEL JEFE
    // ===========================================================
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        if (healthFill != null)
            healthFill.fillAmount = currentHealth / maxHealth;

        if (hpText != null)
            hpText.text = currentHealth.ToString();

        if (damageFlash != null)
            StartCoroutine(FlashDamage());

        if (anim != null)
            anim.SetTrigger("Hit");

        if (currentHealth <= 0)
            Die();
    }

    System.Collections.IEnumerator FlashDamage()
    {
        damageFlash.alpha = 1f;
        yield return new WaitForSeconds(0.15f);
        damageFlash.alpha = 0f;
    }

    void Die()
    {
        isDead = true;
        canMove = false;

        if (anim != null)
        {
            anim.SetFloat("Speed", 0f);
            anim.SetTrigger("Die");
        }
    }

    // ===========================
    // RESPAWN
    // ===========================
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
