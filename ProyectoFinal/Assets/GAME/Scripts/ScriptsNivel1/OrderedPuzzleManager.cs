using UnityEngine;

/// <summary>
/// Gestiona el puzle de pilares con orden estricto y orientación correcta.
/// Requisitos para avanzar:
/// 1. El jugador debe interactuar con los pilares en el orden exacto definido por el array
/// 2. Cada pilar debe estar rotado al ángulo correcto (con tolerancia configurable)
/// 
/// Al completar todo el puzle:
/// • Se activan plataforma giratoria y portal final
/// • Se marca el Minijuego 2 como completado en GameProgress (persistencia)
/// </summary>
public class OrderedPuzzleManager : MonoBehaviour
{
    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Header("Pilares (en orden correcto de activación)")]
    [Tooltip("Array ordenado de los 4 pilares. El índice 0 debe ser el primero en activarse")]
    public PillarInteract[] pillars;

    [Header("Recompensas al completar el puzle")]
    [Tooltip("Plataforma giratoria que se activa al resolver el puzle")]
    public GameObject rotatingPlatform;

    [Tooltip("Portal final que permite salir del minijuego")]
    public GameObject endPortal;

    // ===================================================================
    // ESTADO DEL PUZLE
    // ===================================================================
    private int currentIndex = 0;   // Índice del próximo pilar que debe activarse

    // ===================================================================
    // INICIALIZACIÓN
    // ===================================================================
    private void Start()
    {
        // Ocultar recompensas hasta completar el puzle
        if (rotatingPlatform != null)
            rotatingPlatform.SetActive(false);

        if (endPortal != null)
            endPortal.SetActive(false);

        // Asignar referencia de este manager a cada pilar con su índice correspondiente
        for (int i = 0; i < pillars.Length; i++)
        {
            if (pillars[i] != null)
                pillars[i].SetPuzzleManager(this, i);
        }
    }

    // ===================================================================
    // INTENTO DE ACTIVACIÓN DE UN PILAR
    // ===================================================================
    /// <summary>
    /// Llamado por PillarInteract cuando el jugador pulsa E cerca de un pilar.
    /// Evalúa si es el pilar correcto en el orden y si está bien orientado.
    /// </summary>
    /// <param name="index">Índice del pilar que el jugador intentó activar</param>
    public void TryActivatePillar(int index)
    {
        // Caso 1: No es el pilar que toca en el orden
        if (index != currentIndex)
        {
            UIMessageManager.Instance.ShowPriority("INTENTA CON OTRO PILAR PRIMERO", 2f);
            return;
        }

        // Caso 2: Es el pilar correcto, pero está mal rotado
        if (!pillars[index].CheckAngleCorrect())
        {
            UIMessageManager.Instance.ShowPriority("LA ROTACIÓN NO ES LA CORRECTA", 2f);
            return;
        }

        // Caso 3: Todo correcto → avanzar
        pillars[index].MarkCorrect();   // Feedback visual permanente
        currentIndex++;                 // Pasar al siguiente pilar

        // Completar el puzle si ya se activaron todos
        if (currentIndex >= pillars.Length)
        {
            PuzzleCompleted();
        }
    }

    // ===================================================================
    // COMPLETAR EL PUZLE
    // ===================================================================
    /// <summary>
    /// Se ejecuta al activar correctamente todos los pilares en orden.
    /// Activa las recompensas y registra el minijuego como completado.
    /// </summary>
    private void PuzzleCompleted()
    {
        UIMessageManager.Instance.ShowPriority("PUZZLE COMPLETADO!", 2f);

        if (rotatingPlatform != null)
            rotatingPlatform.SetActive(true);

        if (endPortal != null)
            endPortal.SetActive(true);

        // Registrar progreso persistente: Minijuego 2 completado
        GameProgress.CompleteMiniGame(2);
    }
}