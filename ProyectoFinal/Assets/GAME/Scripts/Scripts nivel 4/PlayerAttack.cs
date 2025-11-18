using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackDamage = 20f;
    public float attackRange = 2f;
    public float attackCooldown = 0.6f;

    public Transform attackPoint; 
    public LayerMask bossLayer;

    private float nextAttackTime = 0f;
    private Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Time.time < nextAttackTime) return;

        if (Input.GetMouseButtonDown(0))
        {
            DoAttack();
        }
    }

    void DoAttack()
    {
        nextAttackTime = Time.time + attackCooldown;

        if (anim != null)
            anim.SetTrigger("Attack");

        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, bossLayer);

        foreach (Collider hit in hits)
        {
            BossHealth bossHP = hit.GetComponentInParent<BossHealth>();

            if (bossHP != null)
            {
                bossHP.TakeDamage(attackDamage);
                Debug.Log("Le hiciste daño al Boss, nueva vida: " + bossHP.currentHealth);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
