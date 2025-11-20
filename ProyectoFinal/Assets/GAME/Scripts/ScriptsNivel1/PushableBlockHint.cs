using UnityEngine;

/// <summary>
/// Muestra un mensaje contextual (hint) al jugador cuando se acerca a un bloque empujable.
/// El texto del mensaje varía según el color del bloque (definido en el componente BlockID).
/// Mejora la experiencia de usuario indicando claramente qué hacer con cada bloque.
/// </summary>
public class PushableBlockHint : MonoBehaviour
{
    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Header("Configuración del Hint")]
    [Tooltip("Distancia máxima desde el jugador para que aparezca el mensaje (en unidades)")]
    public float hintDistance = 2f;

    // ===================================================================
    // REFERENCIAS INTERNAS
    // ===================================================================
    private Transform player;          // Transform del jugador (para calcular distancia)
    private BlockID blockID;           // Componente que contiene el color del bloque
    private bool playerInRange = false; // Evita mostrar el mensaje repetidamente

    // ===================================================================
    // INICIALIZACIÓN
    // ===================================================================
    /// <summary>
    /// Busca y guarda referencias necesarias al iniciar el objeto:
    /// - El jugador (por tag "Player")
    /// - El componente BlockID del mismo bloque
    /// </summary>
    private void Start()
    {
        // Obtener referencia al jugador mediante su etiqueta
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;

        // Obtener el componente BlockID que indica el color del bloque
        blockID = GetComponent<BlockID>();
        if (blockID == null)
        {
            Debug.LogWarning($"PushableBlockHint ({name}): No se encontró componente BlockID en este objeto.");
        }
    }

    // ===================================================================
    // DETECCIÓN DE PROXIMIDAD CADA FRAME
    // ===================================================================
    /// <summary>
    /// Comprueba cada frame si el jugador está dentro del rango definido.
    /// Solo muestra el mensaje la primera vez que entra en rango.
    /// </summary>
    private void Update()
    {
        // Si faltan referencias, no hacer nada
        if (player == null || blockID == null) return;

        // Calcular distancia actual al jugador
        float dist = Vector3.Distance(player.position, transform.position);
        bool nowInRange = dist <= hintDistance;

        // Detectar entrada en rango (de fuera → dentro)
        if (nowInRange && !playerInRange)
        {
            UIMessageManager.Instance.ShowHint(GetMessageForColor(blockID.color));
        }

        // Actualizar estado de rango para el próximo frame
        playerInRange = nowInRange;
    }

    // ===================================================================
    // GENERACIÓN DE MENSAJES SEGÚN COLOR
    // ===================================================================
    /// <summary>
    /// Devuelve un mensaje personalizado según el color del bloque.
    /// Ayuda al jugador a entender la mecánica sin necesidad de tutorial externo.
    /// </summary>
    /// <param name="color">Color actual del bloque (Red, Blue, Yellow, etc.)</param>
    /// <returns>Mensaje descriptivo para mostrar en pantalla</returns>
    private string GetMessageForColor(BlockID.BlockColor color)
    {
        switch (color)
        {
            case BlockID.BlockColor.Red:
                return "Lleva el bloque hacia el portal rojo";
            case BlockID.BlockColor.Blue:
                return "Lleva el bloque hacia el portal azul";
            case BlockID.BlockColor.Yellow:
                return "Empuja el bloque";
            default:
                return "Empuja este bloque";
        }
    }
}