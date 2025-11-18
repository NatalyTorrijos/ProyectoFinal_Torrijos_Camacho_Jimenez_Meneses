using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("----- PLAYER -----")]
    public Transform player;

    [Header("----- AI SETTINGS -----")]
    public float detectDistance = 18f;
    public float attackDistance = 2.4f;
    public float moveSpeed = 5.2f;       
    public float rotateSpeed = 10f;      
    public float attackCooldown = 0.8f; 

    [Header("----- HEALTH SYSTEM -----")]
    public float maxHealth = 200f;
    public float currentHealth;
    public bool isDead = false;

    Animator anim;
    float nextAttackTime = 0f;

    int idleIndexMemo = 1;

    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        idleIndexMemo = Random.Range(1, 4);
        anim.SetInteger("idleIndex", idleIndexMemo);
    }

    void Update()
    {
        if (isDead) return;
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

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

        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotateSpeed * Time.deltaTime);

        anim.SetBool("isRunning", false);

        if (Time.time < nextAttackTime) return;

        nextAttackTime = Time.time + attackCooldown;

        anim.SetInteger("attackIndex", Random.Range(1, 4));
        anim.SetTrigger("Attack");
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
        isDead = true;
        anim.SetBool("isRunning", false);
        anim.SetTrigger("Die");
    }
}
