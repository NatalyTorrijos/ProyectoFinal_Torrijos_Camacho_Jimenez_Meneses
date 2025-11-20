using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("----- PLAYER -----")]
    public Transform player;
    // referencia al jugador para saber donde esta

    [Header("----- AI SETTINGS -----")]
    public float detectDistance = 18f;
    // distancia desde la cual el boss empieza a detectar al jugador :)

    public float attackDistance = 2.4f;
    // distancia minima para comenzar ataques

    public float moveSpeed = 5.2f;
    // velocidad de movimiento del boss

    public float rotateSpeed = 10f;
    // velocidad para girar hacia el jugador :3

    public float attackCooldown = 0.8f;
    // tiempo minimo entre ataques

    public float attackAngleTolerance = 35f;
    // angulo maximo para considerar que mira al jugador

    [Header("----- HEALTH SYSTEM -----")]
    public float maxHealth = 200f;
    public float currentHealth;
    public bool isDead = false;
    // sistema basico de vida

    Animator anim;
    float nextAttackTime = 0f;
    int idleIndexMemo = 1;

    // sonidos
    public AudioSource audioSource;
    public AudioClip[] attackSounds;
    public AudioClip hitSound;
    public AudioClip deathSound;

    void Start()
    {
        // inicializa la animacion y vida
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        // idle inicial aleatorio
        idleIndexMemo = Random.Range(1, 4);
        anim.SetInteger("idleIndex", idleIndexMemo);

        // crea un audiosource si no existe :p
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (isDead) return;
        // si esta muerto no hace nada

        if (player == null) return;
        // si no hay jugador no puede actuar :D

        float distance = Vector3.Distance(transform.position, player.position);

        // comportamiento segun distancia
        if (distance > detectDistance)
        {
            Idle();
        }
        else if (distance > attackDistance)
        {
            RunToPlayer();
        }
        else
        {
            TryAttack();
        }
    }

    void RunToPlayer()
    {
        // calcula direccion hacia el jugador
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;

        // avanza hacia el jugador
        transform.position += dir * moveSpeed * Time.deltaTime;

        // gira suavemente hacia el jugador
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotateSpeed * Time.deltaTime);

        anim.SetBool("isRunning", true);
    }

    void TryAttack()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;

        float angle = Vector3.Angle(transform.forward, dir);
        // calcula si el boss esta mirando lo suficiente al jugador

        if (angle > attackAngleTolerance)
        {
            // si esta muy torcido, sigue acomodandose
            RunToPlayer();
            return;
        }

        // giro final suave
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotateSpeed * Time.deltaTime);

        anim.SetBool("isRunning", false);

        // cooldown
        if (Time.time < nextAttackTime) return;

        nextAttackTime = Time.time + attackCooldown;

        // elige animacion aleatoria
        anim.SetInteger("attackIndex", Random.Range(1, 4));
        anim.SetTrigger("Attack");

        // sonido de ataque
        if (attackSounds.Length > 0)
        {
            AudioClip clip = attackSounds[Random.Range(0, attackSounds.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    void Idle()
    {
        // idle sin correr
        anim.SetBool("isRunning", false);
        anim.SetInteger("idleIndex", idleIndexMemo);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        // resta vida
        currentHealth -= amount;

        // sonido hit
        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            anim.SetTrigger("Hit");
            // animacion de recibir golpe :3
        }
    }

    void Die()
    {
        // marca muerte
        isDead = true;

        anim.SetBool("isRunning", false);
        anim.SetTrigger("Die");

        // sonido muerte
        if (deathSound != null)
            audioSource.PlayOneShot(deathSound);
    }
}
