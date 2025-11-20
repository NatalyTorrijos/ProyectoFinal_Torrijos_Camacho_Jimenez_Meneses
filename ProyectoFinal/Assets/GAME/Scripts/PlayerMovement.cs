using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // ajustes basicos de movimiento
    // controla la velocidad del jugador y la rotacion del modelo
    [Header("movement settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 720f;

    // ajustes del salto
    // jumpheight controla que tan alto salta
    // gravity añade fuerza hacia abajo
    [Header("jump settings")]
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private float gravity = -9.81f;

    // referencia a la camara principal
    [Header("references")]
    public Camera mainCamera;

    private CharacterController controller;
    private Animator anim;

    // para bloquear el movimiento cuando el jugador muere
    public bool canMove = true;

    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isGrounded;
    private bool jumpPressed;

    // sistema de vida del jugador
    // se uso un sistema simple sin ui como pediste :D
    [Header("player health")]
    public float maxHealth = 100f;
    public float currentHealth;

    private bool isDead = false;

    // sistema de ataque del jugador basado en colisiones
    [Header("attack system")]
    public Transform punchPoint;
    public float punchRange = 1.2f;
    public int punchDamage = 1;
    public LayerMask enemyLayer;
    public LayerMask bossLayer; // capa especial para el jefe

    [HideInInspector] public bool isPunching = false;

    // sonidos del jugador
    [Header("sounds")]
    public AudioSource audioSource;
    public AudioClip attackSound;
    public AudioClip hitSound;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        // si no hay camara asignada, toma la principal
        if (mainCamera == null)
            mainCamera = Camera.main;

        // inicia la vida del jugador al maximo
        currentHealth = maxHealth;

        // si no existe un audio source en el objeto, se añade uno :)
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    // recibe input de movimiento
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    // cuando se presiona el boton de saltar
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            jumpPressed = true;
    }

    // cuando se presiona el boton de atacar
    public void OnPunch(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            anim.SetBool("isPunching", true);
            isPunching = true;

            if (attackSound != null)
                audioSource.PlayOneShot(attackSound);

            PunchAttack();
        }

        if (ctx.canceled)
        {
            anim.SetBool("isPunching", false);
            isPunching = false;
        }
    }

    private void Update()
    {
        if (isDead) return;

        // comprueba si el jugador toca el piso
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            // evita que el jugador siga cayendo
            velocity.y = -2f;

            if (anim != null)
                anim.SetBool("isJumping", false);
        }

        // si no puede moverse, solo aplica gravedad
        if (!canMove)
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
            anim.SetFloat("speed", 0f);
            return;
        }

        // direccion del movimiento basado en la camara
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

        // rotacion del jugador hacia la direccion de movimiento
        if (moveDir.sqrMagnitude > 0.05f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        // mueve al jugador
        controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

        // si salto y esta en el piso, aplica fuerza
        if (jumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpPressed = false;

            if (anim != null)
                anim.SetBool("isJumping", true);
        }

        // aplica gravedad cada frame
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // actualiza la animacion del movimiento
        float animSpeed = new Vector3(moveDir.x, 0, moveDir.z).magnitude;
        anim.SetFloat("speed", animSpeed);
    }

    // se ejecuta cuando el character controller golpea algo
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // plataformas que se activan al tocarlas
        Plataforma p = hit.collider.GetComponent<Plataforma>();
        if (p != null)
            p.TocarPlataforma();

        // bloques empujables
        PushableBlock pushable = hit.collider.GetComponent<PushableBlock>();
        if (pushable == null) return;

        // si no hay movimiento, no empuja
        if (moveInput.sqrMagnitude < 0.1f) return;

        Vector3 camForward = mainCamera.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = mainCamera.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        // direccion de empuje :)
        Vector3 pushDir = (camRight * moveInput.x + camForward * moveInput.y).normalized;
        pushDir.y = 0f;

        pushable.Push(pushDir);
    }

    // resta vida al jugador
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        // reproduce sonido de golpe
        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);

        // animacion de golpe
        if (anim != null)
            anim.SetTrigger("hit");

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        canMove = false;

        if (anim != null)
        {
            anim.SetFloat("speed", 0f);
            anim.SetTrigger("die");
        }
    }

    // resetea ciertos valores al reaparecer
    public void OnRespawn()
    {
        moveInput = Vector2.zero;
        velocity = Vector3.zero;
        jumpPressed = false;

        if (anim != null)
        {
            anim.SetFloat("speed", 0f);
            anim.SetBool("isJumping", false);
        }

        controller.Move(Vector3.zero);
    }

    // ataque real, detecta enemigos y jefes dentro del rango
    public void DoPunch()
    {
        if (punchPoint == null) return;

        Collider[] hits = Physics.OverlapSphere(punchPoint.position, punchRange, enemyLayer | bossLayer);

        foreach (Collider hit in hits)
        {
            // enemigo normal
            Enemigo enemy = hit.GetComponent<Enemigo>();
            if (enemy == null)
                enemy = hit.GetComponentInParent<Enemigo>();

            if (enemy != null)
            {
                enemy.TakeDamage(punchDamage);
                if (hitSound != null) audioSource.PlayOneShot(hitSound);
                continue;
            }

            // jefe
            BossHealth boss = hit.GetComponent<BossHealth>();
            if (boss == null)
                boss = hit.GetComponentInParent<BossHealth>();

            if (boss != null)
            {
                boss.TakeDamage(punchDamage);
                if (hitSound != null) audioSource.PlayOneShot(hitSound);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (punchPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(punchPoint.position, punchRange);
        }
    }

    // ataque alterno que se usa desde el input
    void PunchAttack()
    {
        Collider[] enemies = Physics.OverlapSphere(punchPoint.position, punchRange, enemyLayer | bossLayer);

        foreach (Collider enemy in enemies)
        {
            Enemigo e = enemy.GetComponent<Enemigo>();
            if (e != null)
            {
                e.TakeDamage(punchDamage);
                if (hitSound != null) audioSource.PlayOneShot(hitSound);
                continue;
            }

            BossHealth boss = enemy.GetComponent<BossHealth>();
            if (boss == null)
                boss = enemy.GetComponentInParent<BossHealth>();

            if (boss != null)
            {
                boss.TakeDamage(punchDamage);
                if (hitSound != null) audioSource.PlayOneShot(hitSound);
            }
        }
    }
}
