using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public Transform player;            // Referencia al jugador
    public float speed = 3f;            // Velocidad de seguimiento
    public float distanceToFollow = 10f; // Distancia para empezar a seguir
    public int damage = 1;              // Daño al jugador
    public float timeBetweenHits = 1f;  // Tiempo entre golpes

    private float hitTimer = 0f;

    void Update()
    {
        if (player == null) return;

        // Calcula la distancia al jugador
        float distance = Vector3.Distance(transform.position, player.position);

        // Si está cerca, lo sigue
        if (distance <= distanceToFollow)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;
        }

        // Timer para controlar los golpes
        hitTimer += Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && hitTimer >= timeBetweenHits)
        {
            HealthPlayer hp = other.GetComponent<HealthPlayer>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
                hitTimer = 0f; // Reinicia el temporizador
            }
        }
    }
}
