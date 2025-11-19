using UnityEngine;

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
