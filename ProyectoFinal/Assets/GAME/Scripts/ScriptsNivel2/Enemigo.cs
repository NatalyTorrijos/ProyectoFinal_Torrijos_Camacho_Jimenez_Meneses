using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int maxHealth = 3;
    public float damage = 1f;
    public float attackCooldown = 1f;

    [Header("Movimiento")]
    public float detectionRange = 8f;
    public float speed = 2f;
    public Transform player;

    private int currentHealth;
    private float lastAttackTime;

    private Animator anim;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDead) return;
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // SI EL PLAYER ESTÁ EN RANGO → SE MUEVE HACIA ÉL
        if (distance < detectionRange && distance > 1.5f)
        {
            anim.SetFloat("Speed", 1f); // Run animation

            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );

            transform.LookAt(player.position);
        }
        else
        {
            anim.SetFloat("Speed", 0f); // Idle animation
        }
    }

    // ---------------------------------------------------------
    // RECIBIR DAÑO DEL PLAYER (TU TAKE DAMAGE)
    // ---------------------------------------------------------
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        anim.SetTrigger("Hit"); // Animación Reaction

        if (currentHealth <= 0)
            Die();
    }

    // ---------------------------------------------------------
    // MUERTE ANIMADA
    // ---------------------------------------------------------
    void Die()
    {
        isDead = true;
        anim.SetTrigger("Die");
        anim.SetFloat("Speed", 0f);

        Destroy(gameObject, 3f); // Espera la animación antes de destruir
    }

    // ---------------------------------------------------------
    // ATAQUE AL PLAYER (LO QUE YA TENÍAS)
    // ---------------------------------------------------------
    private void OnTriggerStay(Collider other)
    {
        if (isDead) return;
        if (!other.CompareTag("Player")) return;

        if (Time.time < lastAttackTime + attackCooldown)
            return;

        // Animación de ataque
        anim.SetTrigger("Attack");

        HealthPlayer hp = other.GetComponent<HealthPlayer>();
        if (hp != null)
        {
            hp.TakeDamage((int)damage);
        }

        lastAttackTime = Time.time;
    }
}
