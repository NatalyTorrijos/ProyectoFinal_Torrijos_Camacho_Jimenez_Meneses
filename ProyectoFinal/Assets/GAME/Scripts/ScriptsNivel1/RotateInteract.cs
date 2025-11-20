using UnityEngine;

/// <summary>
/// Componente de interacción por trigger para piezas del puzle de rotación (minijuego 1).
/// Permite al jugador rotar una RotatingPiece simplemente entrando en su trigger y pulsando E.
/// Características:
/// • Búsqueda automática de la pieza si no está asignada
/// • Solo permite interacción si la pieza aún no está correcta
/// • Muestra y limpia automáticamente el mensaje de ayuda al entrar/salir del trigger
/// • Usa OnTriggerStay para detectar la pulsación de tecla mientras el jugador permanece dentro
/// </summary>
[RequireComponent(typeof(Collider))]
public class RotateInteract : MonoBehaviour
{
    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Tooltip("Referencia directa a la pieza que será rotada al interactuar")]
    public RotatingPiece piece;

    [Tooltip("Si está activado, busca automáticamente la RotatingPiece en el mismo objeto o hijos si no está asignada")]
    public bool autoFindPiece = true;

    // ===================================================================
    // INICIALIZACIÓN AUTOMÁTICA
    // ===================================================================
    private void Awake()
    {
        // Búsqueda automática de la pieza si no está asignada manualmente
        if (piece == null && autoFindPiece)
        {
            // Primero en el mismo GameObject o padres
            piece = GetComponentInParent<RotatingPiece>();
            if (piece == null)
                piece = GetComponentInChildren<RotatingPiece>();
        }
    }

    // ===================================================================
    // DETECCIÓN CONTINUA MIENTRAS EL JUGADOR ESTÁ DENTRO DEL TRIGGER
    // ===================================================================
    /// <summary>
    /// Se ejecuta cada frame mientras el jugador permanece dentro del collider (debe ser trigger).
    /// Detecta la pulsación de E y rota la pieza asociada si es válido.
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        // Solo responder al jugador
        if (!other.CompareTag("Player")) return;
        if (piece == null) return;
        if (piece.IsCorrect()) return;  // No permitir interacción si ya está resuelta

        // Detectar pulsación de E
        if (Input.GetKeyDown(KeyCode.E))
        {
            piece.RotatePiece();        // Ejecutar rotación y validación
        }
    }

    // ===================================================================
    // MOSTRAR AYUDA AL ENTRAR EN EL ÁREA
    // ===================================================================
    /// <summary>
    /// Muestra mensaje de interacción cuando el jugador entra en el trigger.
    /// Solo si la pieza aún no está correcta.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (piece == null) return;
        if (piece.IsCorrect()) return;

        UIMessageManager.Instance.ShowMessage("Presiona E para girar");
    }

    // ===================================================================
    // LIMPIAR MENSAJE AL SALIR
    // ===================================================================
    /// <summary>
    /// Elimina el mensaje de ayuda cuando el jugador abandona el área de interacción.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        UIMessageManager.Instance.ClearMessage();
    }
}