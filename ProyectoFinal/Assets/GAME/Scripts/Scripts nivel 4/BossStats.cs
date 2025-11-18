using UnityEngine;

public class BossStats : MonoBehaviour
{
    public float maxHealth = 200f;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        Debug.Log("🔥 Boss vida actual: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log(" Boss murió");
        // aquí puedes llamar animación de muerte
        // GetComponent<Animator>().SetTrigger("Die");
        Destroy(gameObject, 2f); // opcional
    }
}
