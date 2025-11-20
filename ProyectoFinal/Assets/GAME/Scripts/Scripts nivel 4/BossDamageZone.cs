using UnityEngine;

public class BossDamageZone : MonoBehaviour
{
    public int damageToPlayer = 1;
    public float damageCooldown = 0.7f;

    bool canDamage = true;

    private void OnTriggerEnter(Collider other)
    {
        TryDamage(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryDamage(other);
    }

    void TryDamage(Collider other)
    {
        if (!canDamage) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            HealthPlayer player = other.GetComponent<HealthPlayer>();

            if (player != null)
            {
                player.TakeDamage(damageToPlayer);
                StartCoroutine(DamageDelay());
            }
        }
    }

    System.Collections.IEnumerator DamageDelay()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }
}
