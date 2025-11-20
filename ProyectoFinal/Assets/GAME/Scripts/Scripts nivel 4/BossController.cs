using UnityEngine;

public class BossController : MonoBehaviour
{
    // referencia al jugador
    [Header("----- PLAYER -----")]
    public Transform player;

    // configuraciones de la ia
    [Header("----- AI SETTINGS -----")]
    public float detectDistance = 18f;          // distancia para empezar a perseguir
    public float attackDistance = 2.4f;         // distancia para iniciar ataque
    public float moveSpeed = 5.2f;              // velocidad de movimiento
    public float rotateSpeed = 10f;             // velocidad de giro
    public float attackCooldown = 0.8f;         // tiempo minimo entre ataques
    public float attackAngleTolerance = 35f;    // tolerancia de angulo para atacar (evita que se quede quieto)

    // sistema de vida
    [Header("----- HEALTH SYSTEM -----")]
    public float maxHealth = 200f;
    public float currentHealth;
    public bool isDead = false;

    Animator anim;
    float nextAttackTime = 0f;

    int idleIndexMemo = 1; // guarda un idle inicial

    // sonidos
    public AudioSource audioSource;
    public AudioClip[] attackSounds;
    public AudioClip hitSound;
    public AudioClip deathSound;

    void Start()
    {
        // inicializacion basica
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        // elegir idle aleatorio y aplicarlo
        idleIndexMemo = Random.Range(1, 4);
        anim.SetInteger("idleIndex", idleIndexMemo);

        // asegurar que exista un audiosource
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        // si esta muerto o no hay jugador, no hace nada
        if (isDead) return;
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // decidir estado segun distancia al jugador
        if (distance > detectDistance)
        {
            Idle(); // fuera de rango de deteccion
        }
        else if (distance > attackDistance)
        {
            RunToPlayer(); // perseguir
        }
        else
        {
            TryAttack(); // intentar atacar
        }
    }

    void RunToPlayer()
    {
        // calcular direccion horizontal hacia el jugador
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;

        // mover hacia el jugador
        transform.position += dir * moveSpeed * Time.deltaTime;

        // girar suavemente hacia la direccion
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotateSpeed * Time.deltaTime);

        anim.SetBool("isRunning", true); // activar animacion correr
    }

    void TryAttack()
    {
        // direccion hacia el jugador (plana)
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;

        // calcular angulo entre adelante del boss y la direccion al jugador
        float angle = Vector3.Angle(transform.forward, dir);

        // si no esta lo suficientemente alineado, seguir moviendose para corregir
        if (angle > attackAngleTolerance)
        {
            RunToPlayer();
            return;
        }

        // girar suavemente antes de atacar
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotateSpeed * Time.deltaTime);

        anim.SetBool("isRunning", false);

        // verificar cooldown
        if (Time.time < nextAttackTime) return;

        nextAttackTime = Time.time + attackCooldown;

        // seleccionar ataque aleatorio y disparar trigger
        anim.SetInteger("attackIndex", Random.Range(1, 4));
        anim.SetTrigger("Attack");

        // reproducir sonido de ataque si hay clips
        if (attackSounds.Length > 0)
        {
            AudioClip clip = attackSounds[Random.Range(0, attackSounds.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    void Idle()
    {
        // animacion idle fija
        anim.SetBool("isRunning", false);
        anim.SetInteger("idleIndex", idleIndexMemo);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        // reproducir sonido de hit si existe
        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);

        // si la vida llega a cero, morir, sino animar hit
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            anim.SetTrigger("Hit");
        }
    }

    void Die()
    {
        // marcar estado muerto y reproducir anim/musica de muerte
        isDead = true;
        anim.SetBool("isRunning", false);
        anim.SetTrigger("Die");

        if (deathSound != null)
            audioSource.PlayOneShot(deathSound);
    }
}
