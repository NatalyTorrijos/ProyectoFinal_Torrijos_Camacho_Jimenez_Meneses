using UnityEngine;

public class PlayerPunchHitbox : MonoBehaviour
{
    [Header("CONFIG")]
    public float radius = 0.6f;          // Tamaño del área del golpe
    public int damage = 1;               // Daño que causa el golpe
    public LayerMask enemyLayer;         // Layer que identifica a los enemigos

    [Header("REFERENCES")]
    public PlayerMovement player;        // Referencia al script del jugador

    private void OnTriggerEnter(Collider other)
    {
        // ❌ Si el Player no está golpeando → cancelar
        if (player == null || !player.isPunching) return;

        // ❌ Si el objeto NO pertenece al enemyLayer → cancelar
        if (((1 << other.gameObject.layer) & enemyLayer.value) == 0) return;

        // ✔ Intentar obtener el script Enemigo
        Enemigo enemy = other.GetComponent<Enemigo>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log("🔥 Player golpeó al enemigo y le hizo daño!");
        }
    }

    // Dibuja en la escena el área del golpe
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
