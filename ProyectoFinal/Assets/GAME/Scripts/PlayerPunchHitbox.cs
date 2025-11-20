using UnityEngine;

public class PlayerPunchHitbox : MonoBehaviour
{
    [Header("CONFIG")]
    public float radius = 0.6f;          // Radio del golpe
    public int damage = 1;               // Daño del golpe
    public LayerMask enemyLayer;         // Layer de enemigos

    [Header("REFERENCES")]
    public PlayerMovement player;        // Referencia al Player

    private void OnTriggerEnter(Collider other)
    {
        // Si el player no está golpeando, cancelar
        if (player == null || !player.isPunching) return;

        // Si no está en el layer de enemigos, cancelar
        if (((1 << other.gameObject.layer) & enemyLayer.value) == 0) return;

        // Intentar obtener componente Enemy
        Enemigo enemy = other.GetComponent<Enemigo>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log("🔥 Player golpeó al enemigo y le hizo daño!");
        }
    }

    // ✔ Esto dibuja el área del golpe para debug
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
