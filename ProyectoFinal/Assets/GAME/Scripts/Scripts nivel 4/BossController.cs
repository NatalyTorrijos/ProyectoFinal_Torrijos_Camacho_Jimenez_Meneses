using UnityEngine;

public class BossController : MonoBehaviour
{
    // referencia al jugador
    [Header("----- PLAYER -----")]
    public Transform player;

    // configuraciones de la ia
    [Header("----- AI SETTINGS -----")]
    public float detectDistance = 18f;
    public float attackDistance = 2.4f;
    public float moveSpeed = 5.2f;
    public float rotateSpeed = 10f;
    public float attackCooldown = 0.8f;
    public float attackAngleTolerance = 35f;

    // sistema de vida
    [Header("----- HEALTH SYSTEM -----")]
    public float maxHealth = 200f;
    public float currentHealth;
    public bool isDead = false;

    Animator anim;
    float nextAttackTime = 0f;

    int idleIndexMemo = 1;

    // sonidos
    public AudioSource audioSource;
    public AudioClip[] attackSounds;
    public AudioClip hitSound;
    public AudioClip deathSound;

    // 🎵 SOLO UNA RANURA DE AUDIOCLIP PARA LA MUSICA
    public AudioClip bossMusic;

    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        idleIndexMemo = Random.Range(1, 4);
        anim.SetInteger("idleIndex", idleIndexMemo);

        // asegurar audiosource
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // reproducir musica del boss usando el MISMO audio source
        if (bossMusic != null)
        {
            audioSource.clip = bossMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void Update()
    {
        if (isDead) return;
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > detectDistance)
            Idle();
        else if (distance > attackDistance)
            RunToPlayer();
        else
            TryAttack();
    }

    void RunToPlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;

        transform.position += dir * moveSpeed * Time.deltaTime;

        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotateSpeed * Time.deltaTime);

        anim.SetBool("isRunning", true);
    }

    void TryAttack()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;

        float angle = Vector3.Angle(transform.forward, dir);

        if (angle > attackAngleTolerance)
        {
            RunToPlayer();
            return;
        }

        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotateSpeed * Time.deltaTime);

        anim.SetBool("isRunning", false);

        if (Time.time < nextAttackTime) return;
        nextAttackTime = Time.time + attackCooldown;

        anim.SetInteger("attackIndex", Random.Range(1, 4));
        anim.SetTrigger("Attack");

        if (attackSounds.Length > 0)
        {
            AudioClip clip = attackSounds[Random.Range(0, attackSounds.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    void Idle()
    {
        anim.SetBool("isRunning", false);
        anim.SetInteger("idleIndex", idleIndexMemo);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);

        if (currentHealth <= 0)
            Die();
        else
            anim.SetTrigger("Hit");
    }

    void Die()
    {
        isDead = true;
        anim.SetBool("isRunning", false);
        anim.SetTrigger("Die");

        if (deathSound != null)
            audioSource.PlayOneShot(deathSound);

        // 🔇 APAGAR LA MUSICA DE LA MISMA RANURA
        audioSource.Stop();
    }
}
