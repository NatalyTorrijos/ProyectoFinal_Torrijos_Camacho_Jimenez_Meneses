using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [Header("Configuración de movimiento")]
    public Vector3 direction = Vector3.up;   // Dirección del movimiento (up, right, forward...)
    public float distance = 2f;              // Cuánto se moverá desde su punto inicial
    public float speed = 2f;                 // Velocidad del movimiento

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // guarda posición original
    }

    void Update()
    {
        // Movimiento tipo ping-pong (ida y vuelta)
        float offset = Mathf.Sin(Time.time * speed) * distance;
        transform.position = startPos + direction.normalized * offset;
    }

    // Dibuja en el editor una guía visual del movimiento
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + direction.normalized * distance);
        Gizmos.DrawWireSphere(transform.position + direction.normalized * distance, 0.2f);
        Gizmos.DrawWireSphere(transform.position - direction.normalized * distance, 0.2f);
    }
}
