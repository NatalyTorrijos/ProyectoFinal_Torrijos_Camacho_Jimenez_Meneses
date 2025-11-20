using UnityEngine;

/// <summary>
/// Gestiona el puzle de memoria basado en secuencia de pisadas (Simon Says).
/// El jugador debe pisar las baldosas en el orden exacto definido en el array correctOrder.
/// Al completar la secuencia correcta se activan el portal final y la plataforma de salida.
/// Si falla, se reinicia el puzle con feedback visual y mensaje en pantalla.
/// </summary>
public class MemoryPuzzleManager : MonoBehaviour
{
    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Header("Secuencia Correcta")]
    [Tooltip("Orden exacto en que deben pisarse las baldosas. Ejemplo: {2, 5, 1, 3}")]
    public int[] correctOrder;                              // Secuencia que el jugador debe replicar

    [Header("Objetos que se activan al completar el puzle")]
    [Tooltip("Portal que aparece al completar correctamente la secuencia")]
    public GameObject finalPortal;

    [Tooltip("Plataforma o puente que se hace visible al completar el puzle")]
    public GameObject finalPlatform;

    // ===================================================================
    // ESTADO INTERNO DEL PUZLE
    // ===================================================================
    private int currentProgress = 0;                        // Índice actual en la secuencia (cuántas baldosas correctas lleva)

    // ===================================================================
    // INICIALIZACIÓN
    // ===================================================================
    /// <summary>
    /// Se ejecuta al cargar la escena. Oculta los objetos de recompensa hasta que
    /// el jugador complete correctamente la secuencia.
    /// </summary>
    private void Start()
    {
        if (finalPortal != null)
            finalPortal.SetActive(false);

        if (finalPlatform != null)
            finalPlatform.SetActive(false);
    }

    // ===================================================================
    // VERIFICACIÓN DE BALDOSA PISADA
    // ===================================================================
    /// <summary>
    /// Llamado por cada MemoryTile cuando el jugador la pisa.
    /// Compara el ID de la baldosa con el siguiente valor esperado en la secuencia.
    /// </summary>
    /// <param name="id">ID único de la baldosa pisada</param>
    /// <param name="tile">Referencia al componente MemoryTile para feedback visual</param>
    public void CheckTile(int id, MemoryTile tile)
    {
        // CASO CORRECTO: coincide con el siguiente paso de la secuencia
        if (id == correctOrder[currentProgress])
        {
            tile.FlashCorrect();            // Feedback visual verde
            currentProgress++;              // Avanzar en la secuencia

            // Completar toda la secuencia
            if (currentProgress >= correctOrder.Length)
            {
                PuzzleCompleted();
            }
        }
        // CASO INCORRECTO: orden equivocado
        else
        {
            tile.FlashWrong();              // Feedback visual rojo
            ResetPuzzle();                  // Reiniciar progreso
        }
    }

    // ===================================================================
    // REINICIAR EL PUZLE
    // ===================================================================
    /// <summary>
    /// Se ejecuta cuando el jugador falla la secuencia.
    /// Reinicia el progreso y notifica al jugador con mensaje prioritario.
    /// </summary>
    private void ResetPuzzle()
    {
        currentProgress = 0;
        UIMessageManager.Instance.ShowPriority("SECUENCIA INCORRECTA, ¡INTENTA OTRO ORDEN!", 2f);
    }

    // ===================================================================
    // COMPLETAR EL PUZLE
    // ===================================================================
    /// <summary>
    /// Se ejecuta al pisar toda la secuencia correctamente.
    /// Activa los objetos de recompensa y muestra mensaje de victoria.
    /// </summary>
    private void PuzzleCompleted()
    {
        UIMessageManager.Instance.ShowPriority("¡¡MINIJUEGO COMPLETADO!!", 2f);

        if (finalPortal != null)
            finalPortal.SetActive(true);

        if (finalPlatform != null)
            finalPlatform.SetActive(true);

        // Aquí podrías llamar a GameProgress.CompleteMiniGame(x) si este fuera un minijuego numerado
    }
}