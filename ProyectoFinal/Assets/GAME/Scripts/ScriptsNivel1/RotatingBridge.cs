using UnityEngine;

/// <summary>
/// Puente o plataforma giratoria que se activa al completar un puzle (normalmente el minijuego 1).
/// Una vez activado:
/// • Permanece visible y activo
/// • Gira constantemente alrededor del eje Y
/// • Opcionalmente puede avanzar en una dirección mientras gira
/// 
/// Se usa como recompensa visual y funcional para permitir el paso del jugador.
/// </summary>
public class RotatingBridge : MonoBehaviour
{
    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Header("Rotación")]
    [Tooltip("Velocidad de rotación en grados por segundo (positivo = horario, negativo = antihorario)")]
    public float rotationSpeed = 40f;

    [Header("Movimiento Lineal Opcional")]
    [Tooltip("Si está activado, el puente además se desplaza mientras gira")]
    public bool moveForward = false;

    [Tooltip("Velocidad de desplazamiento en unidades por segundo")]
    public float moveSpeed = 2f;

    [Tooltip("Dirección normalizada del movimiento (se normaliza automáticamente)")]
    public Vector3 moveDirection = Vector3.forward;

    // ===================================================================
    // ESTADO DE LA PLATAFORMA
    // ===================================================================
    private bool active = false;

    // ===================================================================
    // ACTUALIZACIÓN CADA FRAME
    // ===================================================================
    /// <summary>
    /// Solo se ejecuta cuando el puente ha sido activado por el RotationPuzzleManager.
    /// Aplica rotación constante y, si está habilitado, movimiento lineal.
    /// </summary>
    private void Update()
    {
        if (!active) return;

        // Rotación continua alrededor del eje Y (en espacio mundial)
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

        // Movimiento lineal opcional en la dirección configurada
        if (moveForward)
        {
            Vector3 normalizedDirection = moveDirection.normalized;
            transform.Translate(normalizedDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    // ===================================================================
    // ACTIVACIÓN PÚBLICA (LLAMADA DESDE EL MANAGER)
    // ===================================================================
    /// <summary>
    /// Método público llamado por RotationPuzzleManager al completar el puzle.
    /// Activa el GameObject (si estaba desactivado) y comienza la rotación/movimiento.
    /// </summary>
    public void ActivateBridge()
    {
        active = true;
        gameObject.SetActive(true);  // Asegura que esté visible y activo
    }
}
