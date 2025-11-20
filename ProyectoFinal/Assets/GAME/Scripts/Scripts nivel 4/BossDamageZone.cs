using UnityEngine;

public class BossDamageZone : MonoBehaviour
{
    public int damageToPlayer = 1;
    // cuanto daño hace el boss al jugador

    public float damageCooldown = 0.7f;
    // tiempo entre daños repetidos

    bool canDamage = true;
    // para evitar daño constante cada frame :)

    private void OnTriggerEnter(Collider other)
    {
        // si entra en contacto, intenta hacer daño
        TryDamage(other);
    }

    private void OnTriggerStay(Collider other)
    {
        // mantiene el daño si sigue dentro :p
        TryDamage(other);
    }

    void TryDamage(Collider other)
    {
        if (!canDamage) return;

        // verifica que el objeto sea el jugador
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            HealthPlayer player = other.GetComponent<HealthPlayer>();

            if (player != null)
            {
                // aplica daño
                player.TakeDamage(damageToPlayer);

                // inicia cooldown del daño
                StartCoroutine(DamageDelay());
            }
        }
    }

    System.Collections.IEnumerator DamageDelay()
    {
        // pausa entre daños
        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }
}
