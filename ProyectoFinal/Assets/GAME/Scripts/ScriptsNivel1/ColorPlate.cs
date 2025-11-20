using UnityEngine;

/// <summary>
/// Placa de presión de color que detecta bloques específicos.
/// Se encarga de:
/// • Detectar cuando un bloque entra o sale de su área de activación
/// • Verificar si el bloque tiene el color requerido para la solución
/// • Notificar al PuzzleManager sobre cambios en su estado (correcta/incorrecta)
/// • Mantener el estado de si tiene el bloque correcto colocado
/// • Distinguir entre bloques correctos e incorrectos mediante su componente BlockID
/// 
/// Requiere un Collider configurado como Trigger para detectar los bloques.
/// </summary>
[RequireComponent(typeof(Collider))]
public class ColorPlate : MonoBehaviour
{
    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Header("Color requerido")]
    [Tooltip("Color del bloque que debe colocarse en esta placa para considerarla correcta")]
    public BlockID.BlockColor requiredColor = BlockID.BlockColor.Red;

    // ===================================================================
    // ESTADO INTERNO
    // ===================================================================
    private PuzzleManager manager;
    private bool isCorrect = false;

    // ===================================================================
    // INICIALIZACIÓN
    // ===================================================================
    private void Start()
    {
        // Buscar el PuzzleManager en la escena
        manager = FindObjectOfType<PuzzleManager>();

        if (manager == null)
            Debug.LogError($"ColorPlate ({name}): No se encontró PuzzleManager en la escena.");
    }

    // ===================================================================
    // DETECCIÓN DE BLOQUES ENTRANTES
    // ===================================================================
    /// <summary>
    /// Se ejecuta cuando un objeto entra en el área de la placa.
    /// Verifica si es un bloque con el color correcto y notifica al manager.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // Buscar el componente BlockID (puede estar en el objeto o en su padre)
        BlockID block = other.GetComponentInParent<BlockID>();
        if (block == null) return;

        // Verificar si el bloque tiene el color requerido
        if (block.color == requiredColor)
        {
            if (!isCorrect)
            {
                isCorrect = true;
                Debug.Log($"ColorPlate {name}: bloque CORRECTO ({block.name}).");
                manager?.PlateStateChanged(this, true);
                // Opcional: efecto visual aquí (partículas, luz, sonido, etc.)
            }
        }
        else
        {
            // Bloque incorrecto detectado
            Debug.Log($"ColorPlate {name}: bloque incorrecto ({block.name}) color = {block.color} (se espera {requiredColor}).");
            manager?.PlateStateChanged(this, false);
        }
    }

    // ===================================================================
    // DETECCIÓN DE BLOQUES SALIENTES
    // ===================================================================
    /// <summary>
    /// Se ejecuta cuando un objeto sale del área de la placa.
    /// Actualiza el estado y notifica al manager si era el bloque correcto.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        // Buscar el componente BlockID (puede estar en el objeto o en su padre)
        BlockID block = other.GetComponentInParent<BlockID>();
        if (block == null) return;

        // Si sale el bloque correcto, marcar la placa como incorrecta
        if (block.color == requiredColor)
        {
            if (isCorrect)
            {
                isCorrect = false;
                Debug.Log($"ColorPlate {name}: bloque CORRECTO salio ({block.name}).");
                manager?.PlateStateChanged(this, false);
            }
        }
        else
        {
            // Si sale un bloque incorrecto, asegurar que la placa esté marcada como incorrecta
            manager?.PlateStateChanged(this, false);
        }
    }
}