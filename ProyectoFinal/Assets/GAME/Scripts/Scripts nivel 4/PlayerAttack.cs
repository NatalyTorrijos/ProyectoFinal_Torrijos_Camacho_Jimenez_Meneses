//using UnityEngine;

//public class PlayerAttack : MonoBehaviour
//{
//    public float attackDamage = 20f;
//    public float attackRange = 2f;
//    public float attackCooldown = 0.6f;

//    public Transform attackPoint;
//    public LayerMask bossLayer;

//    private float nextAttackTime = 0f;
//    private Animator anim;

//    // 🎵 SONIDOS
//    public AudioSource audioSource;
//    public AudioClip attackSwoosh;
//    public AudioClip hitBossSound;

//    void Start()
//    {
//        anim = GetComponentInChildren<Animator>();

//        if (audioSource == null)
//            audioSource = gameObject.AddComponent<AudioSource>();
//    }

//    void Update()
//    {
//        if (Time.time < nextAttackTime) return;

//        if (Input.GetMouseButtonDown(0))
//        {
//            DoAttack();
//        }
//    }

//    //void DoAttack()
//    //{
//    //    nextAttackTime = Time.time + attackCooldown;

//    //    if (anim != null)
//    //        anim.SetTrigger("Attack");

//    //    // 🔊 reproduce sonido de swing
//    //    if (attackSwoosh != null)
//    //        audioSource.PlayOneShot(attackSwoosh);

//    //    Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, bossLayer);

//    //    foreach (Collider hit in hits)
//    //    {
//    //        BossHealth bossHP = hit.GetComponentInParent<BossHealth>();

//    //        if (bossHP != null)
//    //        {
//    //            bossHP.TakeDamage(attackDamage);

//    //            // 🔊 sonido de golpe si pega al boss
//    //            if (hitBossSound != null)
//    //                audioSource.PlayOneShot(hitBossSound);

//    //            Debug.Log("Le hiciste daño al Boss, nueva vida: " + bossHP.currentHealth);
//    //        }
//    //    }
//    //}

//    void OnDrawGizmosSelected()
//    {
//        if (attackPoint == null) return;

//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
//    }
//}
