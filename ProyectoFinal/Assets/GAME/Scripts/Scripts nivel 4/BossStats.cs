using UnityEngine;

public class BossStats : MonoBehaviour
{
    public float maxHealth = 200f;
    public float currentHealth;
    // vida basica del boss

    void Start()
    {
        currentHealth = maxHealth;
        // inicia la salud al maximo :)
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        Debug.Log("boss vida actual: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("boss murio :p");

        // destruye el boss despues de un tiempo
        Destroy(gameObject, 2f);
    }
}
