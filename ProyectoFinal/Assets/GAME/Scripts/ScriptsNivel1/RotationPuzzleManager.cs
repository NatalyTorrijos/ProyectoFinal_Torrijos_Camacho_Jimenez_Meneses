using UnityEngine;

/// <summary>
/// Gestor principal del puzle de rotación (Minijuego 1).
/// Controla un conjunto de RotatingPiece y verifica cuando todas están correctamente orientadas.
/// Al completar el puzle:
/// • Activa efectos visuales y sonoros
/// • Despliega el puente giratorio (RotatingBridge)
/// • Desbloquea el teletransporte de salida
/// • Registra el minijuego como completado en GameProgress (persistencia)
/// </summary>
public class RotationPuzzleManager : MonoBehaviour
{
    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Header("Piezas del Puzzle")]
    [Tooltip("Todas las piezas rotatorias que forman parte del puzle. Deben estar en orden cualquiera")]
    public RotatingPiece[] pieces;

    [Header("Efectos al Completar")]
    [Tooltip("Luz que se enciende al resolver el puzle (feedback visual)")]
    public Light puzzleLight;

    [Tooltip("Sonido de victoria que se reproduce al completar")]
    public AudioSource completeSound;

    [Header("Recompensas del Puzzle")]
    [Tooltip("Puente giratorio que se activa al completar el puzle")]
    public RotatingBridge rotatingBridge;

    [Tooltip("Teleporter de salida del minijuego. Se desbloquea al completar")]
    public TeleporterZone exitTeleporter;

    // ===================================================================
    // ESTADO DEL PUZLE
    // ===================================================================
    private bool puzzleCompleted = false;

    // ===================================================================
    // MÉTODO PÚBLICO LLAMADO POR CADA PIEZA
    // ===================================================================
    /// <summary>
    /// Llamado automáticamente por cada RotatingPiece cuando se rota y posiblemente se corrige.
    /// Verifica si todas las piezas están en su posición correcta.
    /// </summary>
    public void CheckPuzzleState()
    {
        if (puzzleCompleted) return;

        // Comprobar que TODAS las piezas estén correctas
        foreach (var piece in pieces)
        {
            if (piece == null || !piece.IsCorrect())
                return; // Aún falta alguna pieza
        }

        // Si llega aquí → ¡todas están correctas!
        PuzzleCompleted();
    }

    // ===================================================================
    // COMPLETAR EL PUZLE
    // ===================================================================
    /// <summary>
    /// Se ejecuta una sola vez cuando el jugador resuelve completamente el puzle.
    /// Activa todas las recompensas y registra el progreso persistente.
    /// </summary>
    private void PuzzleCompleted()
    {
        puzzleCompleted = true;
        Debug.Log("Puzzle COMPLETADO: Activando puente y salida.");

        // Efectos visuales y sonoros
        if (puzzleLight != null)
            puzzleLight.enabled = true;

        if (completeSound != null)
            completeSound.Play();

        // Activar puente giratorio
        if (rotatingBridge != null)
            rotatingBridge.ActivateBridge();

        // Desbloquear teletransporte de salida
        if (exitTeleporter != null)
            exitTeleporter.requireCompletion = false;

        // REGISTRAR PROGRESO PERSISTENTE
        GameProgress.CompleteMiniGame(1);

        // Feedback final al jugador
        UIMessageManager.Instance.ShowPriority("MINIJUEGO 1 COMPLETADO!", 3f);
    }
}