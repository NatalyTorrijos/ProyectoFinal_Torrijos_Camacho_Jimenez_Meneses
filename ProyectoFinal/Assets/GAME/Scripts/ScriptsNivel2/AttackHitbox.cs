using UnityEngine;

/// <summary>
/// En este script lo que se hace es ver o validar la hitbox de la mano del enemigo toca al player, para asi quitarle vida.
/// </summary>

public class AttackHitbox : MonoBehaviour
{
    public float damage = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthPlayer hp = other.GetComponent<HealthPlayer>();
            if (hp != null)
            {
                hp.TakeDamage((int)damage);
            }
        }
    }
}
