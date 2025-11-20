
using UnityEngine;

public class BossStats : MonoBehaviour
{
    // vida maxima del boss
    public float maxHealth = 200f;

    // vida actual del boss
    public float currentHealth;

    void Start()
    {
        // al iniciar, la vida actual se establece igual a la vida maxima :)
        currentHealth = maxHealth;
    }

    // este metodo se llama cuando el boss recibe dano
    public void TakeDamage(float damage)
    {
        // se reduce la vida con el dano recibido
        currentHealth -= damage;

        // mensaje para revisar por consola :p
        Debug.Log("boss vida actual: " + currentHealth);

        // si la vida llega a cero o menos, se llama al metodo de muerte
        if (currentHealth <= 0)
            Die();
    }

    // este metodo se ejecuta cuando el boss muere
    void Die()
    {
        // mensaje en consola confirmando la muerte del boss
        Debug.Log("boss murio");

        // destruye el objeto despues de 2 segundos para permitir animaciones o sonidos
        Destroy(gameObject, 2f);
    }
}
