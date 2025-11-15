using UnityEngine;

public class BossAI : MonoBehaviour
{
    public Transform player;
    public float detectDistance = 18f;
    public float attackDistance = 2.4f;
    public float moveSpeed = 4f;
    public float rotateSpeed = 6f;
    public float attackCooldown = 1.2f;

    Animator anim;
    float nextAttackTime = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
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
        anim.SetInteger("attackIndex", Random.Range(1, 4)); // 1..3
        anim.SetTrigger("Attack");
    }

    void Idle()
    {
        anim.SetBool("isRunning", false);
        anim.SetInteger("idleIndex", Random.Range(1, 4));
    }
}
