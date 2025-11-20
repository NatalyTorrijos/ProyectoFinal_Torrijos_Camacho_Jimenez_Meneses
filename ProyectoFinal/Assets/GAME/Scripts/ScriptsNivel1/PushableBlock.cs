using UnityEngine;

/// <summary>
/// Bloque empujable con movimiento restringido a un eje específico.
/// Se encarga de:
/// • Permitir que el jugador empuje el bloque en una dirección específica
/// • Restringir el movimiento solo al eje configurado (X o Z)
/// • Mantener la física de gravedad activa para que el bloque pueda caer
/// • Aplicar frenado suave cuando no está siendo empujado
/// • Bloquear la rotación para mantener el bloque estable
/// 
/// Requiere un componente Rigidbody para funcionar correctamente.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PushableBlock : MonoBehaviour
{
    // ===================================================================
    // ENUMERACIÓN DE EJES
    // ===================================================================
    public enum Axis { X, Z }

    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Header("Configuración de movimiento")]
    [Tooltip("Eje en el que el bloque puede moverse (X horizontal o Z profundidad)")]
    public Axis moveAxis = Axis.X;

    [Tooltip("Velocidad a la que se desliza el bloque cuando es empujado")]
    public float slideSpeed = 2f;

    [Tooltip("Velocidad del frenado suave cuando deja de ser empujado (mayor = más rápido)")]
    public float stopSmoothness = 5f;

    // ===================================================================
    // ESTADO INTERNO
    // ===================================================================
    private Rigidbody rb;
    private bool beingPushed = false;
    private Vector3 pushDirection;

    // ===================================================================
    // INICIALIZACIÓN
    // ===================================================================
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Solo bloqueamos la rotación, pero NO la posición en Y (puede caer libremente)
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // ===================================================================
    // FÍSICA DE MOVIMIENTO
    // ===================================================================
    private void FixedUpdate()
    {
        if (beingPushed)
        {
            Vector3 dir = pushDirection.normalized;

            // Limitar dirección al eje permitido configurado
            if (moveAxis == Axis.X)
                dir = new Vector3(dir.x, 0, 0);
            else
                dir = new Vector3(0, 0, dir.z);

            // Mantener la velocidad actual en Y para que no afecte la caída por gravedad
            Vector3 newVel = new Vector3(dir.x * slideSpeed, rb.linearVelocity.y, dir.z * slideSpeed);
            rb.linearVelocity = newVel;

            beingPushed = false;
        }
        else
        {
            // Frenado horizontal suave, sin afectar el eje Y (gravedad)
            Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            Vector3 smoothed = Vector3.Lerp(horizontalVel, Vector3.zero, Time.fixedDeltaTime * stopSmoothness);
            rb.linearVelocity = new Vector3(smoothed.x, rb.linearVelocity.y, smoothed.z);
        }
    }

    // ===================================================================
    // MÉTODO PÚBLICO PARA EMPUJAR
    // ===================================================================
    /// <summary>
    /// Aplica una fuerza de empuje al bloque en la dirección especificada.
    /// Llamado externamente por el jugador u otros scripts.
    /// </summary>
    /// <param name="direction">Vector de dirección del empuje</param>
    public void Push(Vector3 direction)
    {
        pushDirection = direction;
        beingPushed = true;
    }
}