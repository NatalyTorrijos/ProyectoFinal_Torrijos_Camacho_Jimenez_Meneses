using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 1f;
    public float moveSpeed = 2f;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        
        // ----------------------------ATAQUE
        
        if (dist <= attackRange)
        {
            animator.SetBool("RunBool", false);
            animator.SetBool("IdleBool", false);

            animator.SetTrigger("AttackTrigger");
            return;
        }


        // ----------------------------PERSEGUIR

        if (dist <= detectionRange)
        {
            
            Vector3 lookDir = player.position - transform.position;
            lookDir.y = 0;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(lookDir),
                10f * Time.deltaTime
            );

            
            Vector3 direccion = (player.position - transform.position).normalized;
            Vector3 destino = player.position - direccion * 0.6f;

            transform.position = Vector3.MoveTowards(
                transform.position,
                destino,
                moveSpeed * Time.deltaTime
            );

            animator.SetBool("RunBool", true);
            animator.SetBool("IdleBool", false);
        }


        // ----------------------------IDLE

        else
        {
            animator.SetBool("RunBool", false);
            animator.SetBool("IdleBool", true);
        }
    }
}
