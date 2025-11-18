using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackDamage = 20f;
    public float attackRange = 2f;
    public float attackCooldown = 0.6f;

    [Header("References")]
    public Transform attackPoint;   // mano del jugador
    public LayerMask bossLayer;     // layer del boss

    private Animator anim;
    private float nextAttackTime = 0f;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Time.time < nextAttackTime)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            PerformAttack();
        }
    }

    void PerformAttack()
    {
        nextAttackTime = Time.time + attackCooldown;

        if (anim != null)
            anim.SetTrigger("Attack");

        Debug.Log("🗡 Player intentó atacar");

        Collider[] detected = Physics.OverlapSphere(attackPoint.position, attackRange, bossLayer);

        foreach (Collider c in detected)
        {
            var boss = c.GetComponent<BossStats>();
            if (boss != null)
            {
                boss.TakeDamage(attackDamage);
                Debug.Log("💥 Boss recibió daño");
            }
        }
    }

    // Solo para visualizar en escena
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
