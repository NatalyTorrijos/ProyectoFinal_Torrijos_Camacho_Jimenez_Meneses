using UnityEngine;

/// <summary>
/// Gestor central del puzzle de placas de colores.
/// Se encarga de:
/// • Mantener un registro de todas las placas de color del puzzle
/// • Monitorear el estado de cada placa (correcta o incorrecta)
/// • Verificar si todas las placas están en estado correcto simultáneamente
/// • Activar el objeto final (puente/portal) cuando el puzzle esté completo
/// • Desactivar el objeto final si alguna placa deja de estar correcta
/// 
/// Las placas individuales notifican a este manager cuando su estado cambia.
/// </summary>
public class PuzzleManager : MonoBehaviour
{
    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Header("Placas (assign in inspector)")]
    [Tooltip("Array de todas las placas de color que forman parte del puzzle")]
    public ColorPlate[] plates;

    [Header("Objeto final a activar (puente/portal)")]
    [Tooltip("GameObject que se activará cuando todas las placas estén correctas")]
    public GameObject finalBridge;

    // ===================================================================
    // ESTADO INTERNO DEL PUZZLE
    // ===================================================================
    private bool[] plateStates;

    // ===================================================================
    // INICIALIZACIÓN
    // ===================================================================
    private void Start()
    {
        // Validar que hay placas asignadas
        if (plates == null || plates.Length == 0)
            Debug.LogError("PuzzleManager: no hay plates asignadas.");

        // Inicializar array de estados (todas comienzan en false)
        plateStates = new bool[plates != null ? plates.Length : 0];

        // Iniciar con el objeto final desactivado
        if (finalBridge != null)
            finalBridge.SetActive(false);
    }

    // ===================================================================
    // NOTIFICACIÓN DE CAMBIO DE ESTADO
    // ===================================================================
    /// <summary>
    /// Método llamado por las placas individuales cuando su estado cambia.
    /// Actualiza el estado interno y verifica si el puzzle está completo.
    /// </summary>
    /// <param name="plate">La placa que notifica el cambio</param>
    /// <param name="correct">True si la placa está en estado correcto, False si no</param>
    public void PlateStateChanged(ColorPlate plate, bool correct)
    {
        // Buscar el índice de la placa en el array
        int idx = System.Array.IndexOf(plates, plate);

        if (idx < 0)
        {
            Debug.LogWarning("PuzzleManager: PlateStateChanged recibió una plate no registrada.");
            return;
        }

        // Actualizar el estado de la placa
        plateStates[idx] = correct;
        Debug.Log($"PuzzleManager: plate '{plate.name}' changed -> {correct}");

        // Verificar si el puzzle está completo
        CheckPuzzleState();
    }

    // ===================================================================
    // VERIFICACIÓN DEL ESTADO DEL PUZZLE
    // ===================================================================
    /// <summary>
    /// Verifica si todas las placas están en estado correcto.
    /// Activa o desactiva el objeto final según corresponda.
    /// </summary>
    private void CheckPuzzleState()
    {
        if (plateStates == null || plateStates.Length == 0) return;

        // Verificar si hay alguna placa incorrecta
        foreach (bool p in plateStates)
        {
            if (!p)
            {
                // Hay una placa incompleta: asegurarse de que el puente esté apagado
                if (finalBridge != null && finalBridge.activeSelf)
                    finalBridge.SetActive(false);
                return;
            }
        }

        // Todas las placas están correctas → activar el objeto final
        Debug.Log("PuzzleManager: Todas las placas correctas -> activar finalBridge");
        if (finalBridge != null)
            finalBridge.SetActive(true);
    }
}