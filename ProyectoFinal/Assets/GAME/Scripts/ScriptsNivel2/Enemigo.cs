using UnityEngine;
using System.Collections;
/// <summary>
/// Aqui esta toda la logica del enemigo del nivel2, su vida, su ataque las animaciones, el perseguir al player cuando esta en un rango determinado
/// </summary>

public class Enemigo : MonoBehaviour
{
    [Header("Config enemigo")]
    public int maxHealth = 3;
    public float detectionRange = 8f;
    public float speed = 2f;

    [Header("Ataque")]
    public float attackCooldown = 1f;
    public GameObject attackHitbox;
    public float hitboxActiveTime = 0.2f;  
    public float damage = 1f;

    private int currentHealth;
    private float lastAttackTime;
    private Animator anim;
    private Transform player;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //-------------------------------------------Hitbox Apagada
        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }

    private void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        //---------------------------------------------------------------persigue al player si esta en el rango de deteccion
        if (distance < detectionRange && distance > 1.5f)
        {
            anim.SetFloat("Speed", 1f);

            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );

            transform.LookAt(player.position);
        }
        else
        {
            anim.SetFloat("Speed", 0f);
        }

        // -------------------------------si esta lo suficientemente cerca del player lo ataca
        if (distance <= 1.5f)
            TryAttack();
    }

    private void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        anim.SetTrigger("Attack");

        StartCoroutine(ActivateHitbox());

        lastAttackTime = Time.time;
    }

    private IEnumerator ActivateHitbox()
    {
        attackHitbox.SetActive(true);
        yield return new WaitForSeconds(hitboxActiveTime);
        attackHitbox.SetActive(false);
    }

    // ----------------Aqui se evalua lo que es la animacion de dead y hit, yaque al pegar el player se analiza si aun tiene vidas el enemigo, si no muere.
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        anim.SetTrigger("Hit");

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        anim.SetTrigger("Die");
        anim.SetFloat("Speed", 0f);
        Destroy(gameObject, 3f);
    }
}
