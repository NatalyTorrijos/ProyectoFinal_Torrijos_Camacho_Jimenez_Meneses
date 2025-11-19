using UnityEngine;

public class RotationPuzzleManager : MonoBehaviour
{
    [Header("Piezas rotatorias del puzzle")]
    public RotatingPiece[] pieces;

    [Header("Efectos al completar")]
    public Light puzzleLight;
    public AudioSource completeSound;

    [Header("Plataforma que aparece al completar el puzzle")]
    public RotatingBridge rotatingBridge;

    [Header("Teleporter final del minijuego")]
    public TeleporterZone exitTeleporter;

    private bool puzzleCompleted = false;

    // Llamado cada vez que una pieza rota
    public void CheckPuzzleState()
    {
        if (puzzleCompleted) return;

        // Verificar si todas las piezas están correctas
        foreach (var p in pieces)
        {
            if (!p.IsCorrect())
                return;
        }

        // Si todas están correctas → completar puzzle
        PuzzleCompleted();

    }

    private void PuzzleCompleted()
    {
        puzzleCompleted = true;

        Debug.Log("✨ Puzzle COMPLETADO: Activando puente y salida.");

        // Efectos visuales/sonoros
        if (puzzleLight != null)
            puzzleLight.enabled = true;

        if (completeSound != null)
            completeSound.Play();

        // Activar PUENTE
        if (rotatingBridge != null)
            rotatingBridge.ActivateBridge();

        // Desbloquear teleporter final
        if (exitTeleporter != null)
            exitTeleporter.requireCompletion = false;

        // Registrar minijuego como completado
      
    }
}
