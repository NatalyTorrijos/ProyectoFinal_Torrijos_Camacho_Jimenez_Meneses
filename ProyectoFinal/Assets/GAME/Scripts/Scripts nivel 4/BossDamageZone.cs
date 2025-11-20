using UnityEngine;

public class BossDamageZone : MonoBehaviour
{
    public int damageToPlayer = 1;      // cuanto quita al player por contacto
    public float damageCooldown = 0.7f; // tiempo entre daños repetidos

    bool canDamage = true;             // evita aplicar daño cada frame

    private void OnTriggerEnter(Collider other)
    {
        // intentar causar daño al entrar
        TryDamage(other);
    }

    private void OnTriggerStay(Collider other)
    {
        // intentar causar daño mientras se mantiene dentro del trigger
        TryDamage(other);
    }

    void TryDamage(Collider other)
    {
        if (!canDamage) return;

        // solo dañar si el objeto pertenece a la layer "Player"
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            HealthPlayer player = other.GetComponent<HealthPlayer>();

            if (player != null)
            {
                // aplicar daño y activar cooldown
                player.TakeDamage(damageToPlayer);
                StartCoroutine(DamageDelay());
            }
        }
    }

    System.Collections.IEnumerator DamageDelay()
    {
        // bloquear daño por un tiempo breve
        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }
}
