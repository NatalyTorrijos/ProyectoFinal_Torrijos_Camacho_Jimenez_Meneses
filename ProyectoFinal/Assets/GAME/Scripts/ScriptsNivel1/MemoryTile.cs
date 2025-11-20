using UnityEngine;

/// <summary>
/// Componente individual de cada baldosa en el puzle de memoria (secuencia de pisadas).
/// Cada losa tiene un ID único y responde al contacto del jugador enviando su ID al MemoryPuzzleManager.
/// Proporciona feedback visual inmediato (verde = correcto, rojo = incorrecto) y vuelve al color base.
/// </summary>
public class MemoryTile : MonoBehaviour
{
    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Header("Identificación y Apariencia")]
    [Tooltip("ID único de esta baldosa. Debe coincidir con un valor del array correctOrder en MemoryPuzzleManager")]
    public int tileID;                                      // Identificador único en la secuencia

    [Tooltip("Renderer de la baldosa para cambiar color dinámicamente")]
    public Renderer tileRenderer;

    [Header("Colores de Feedback")]
    [Tooltip("Color normal de la baldosa cuando está inactiva")]
    public Color baseColor = Color.gray;

    [Tooltip("Color que parpadea cuando el jugador pisa correctamente esta baldosa")]
    public Color correctColor = Color.green;

    [Tooltip("Color que parpadea cuando el jugador comete un error en esta baldosa")]
    public Color wrongColor = Color.red;

    // ===================================================================
    // REFERENCIA AL GESTOR DEL PUZLE
    // ===================================================================
    private MemoryPuzzleManager manager;                    // Referencia al controlador del puzle

    // ===================================================================
    // INICIALIZACIÓN
    // ===================================================================
    /// <summary>
    /// Se ejecuta al iniciar la escena:
    /// - Establece el color base de la baldosa
    /// - Busca automáticamente el MemoryPuzzleManager en la escena
    /// </summary>
    private void Start()
    {
        // Aplicar color base inicial
        if (tileRenderer != null)
            tileRenderer.material.color = baseColor;

        // Buscar el gestor del puzle (debe haber solo uno en la escena)
        manager = FindObjectOfType<MemoryPuzzleManager>();
        if (manager == null)
            Debug.LogError($"MemoryTile ({name}): No se encontró MemoryPuzzleManager en la escena.");
    }

    // ===================================================================
    // DETECCIÓN DE CONTACTO CON EL JUGADOR
    // ===================================================================
    /// <summary>
    /// Se activa cuando el jugador entra en el trigger de la baldosa.
    /// Solo responde al objeto con tag "Player" y notifica al gestor del puzle.
    /// </summary>
    /// <param name="other">Collider que entró en el trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        // Ignorar todo lo que no sea el jugador
        if (!other.CompareTag("Player")) return;

        // Enviar esta baldosa al gestor para validar la secuencia
        if (manager != null)
            manager.CheckTile(tileID, this);
    }

    // ===================================================================
    // EFECTOS VISUALES DE FEEDBACK
    // ===================================================================
    /// <summary>
    /// Llamado por el MemoryPuzzleManager cuando esta baldosa es correcta en la secuencia.
    /// Cambia temporalmente el color a verde.
    /// </summary>
    public void FlashCorrect()
    {
        if (tileRenderer != null)
            tileRenderer.material.color = correctColor;

        Invoke(nameof(ResetColor), 0.4f);  // Volver al color base tras breve delay
    }

    /// <summary>
    /// Llamado cuando el jugador falla al pisar esta baldosa.
    /// Muestra color rojo durante más tiempo para enfatizar el error.
    /// </summary>
    public void FlashWrong()
    {
        if (tileRenderer != null)
            tileRenderer.material.color = wrongColor;

        Invoke(nameof(ResetColor), 0.7f);  // Más tiempo visible para reforzar el error
    }

    /// <summary>
    /// Restaura el color original de la baldosa tras el feedback visual.
    /// </summary>
    private void ResetColor()
    {
        if (tileRenderer != null)
            tileRenderer.material.color = baseColor;
    }
}