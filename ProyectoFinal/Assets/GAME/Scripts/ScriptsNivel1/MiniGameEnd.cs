using UnityEngine;

/// <summary>
/// Zona final de cada minijuego (1 y 2). 
/// Al ser atravesada por el jugador:
/// 1. Marca el minijuego como completado en GameProgress (persistencia)
/// 2. Actualiza visualmente el estado de la pirámide en el Hub
/// 3. Muestra mensaje contextual según progreso global
/// 4. Teletransporta al jugador de vuelta al Hub de forma segura
/// 5. Reproduce efecto de partículas en el punto de respawn
/// 
/// Solo se activa una vez por jugador.
/// </summary>
public class MiniGameEnd : MonoBehaviour
{
    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Header("Configuración del Minijuego")]
    [Tooltip("Número del minijuego que finaliza al tocar este trigger (1 o 2)")]
    public int miniGameIndex = 1;

    [Header("Efectos Visuales")]
    [Tooltip("Prefab de partículas que se reproduce al reaparecer en el Hub")]
    public GameObject respawnEffect;

    // ===================================================================
    // ESTADO INTERNO
    // ===================================================================
    private bool activated = false;

    // ===================================================================
    // DETECCIÓN DE FINALIZACIÓN
    // ===================================================================
    /// <summary>
    /// Se ejecuta cuando el jugador entra en el trigger final del minijuego.
    /// Solo responde al jugador y una única vez.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;
        if (!other.CompareTag("Player")) return;

        activated = true;

        // 1. Registrar progreso persistente
        GameProgress.CompleteMiniGame(miniGameIndex);

        // 2. Actualizar estado visual de la pirámide en el Hub
        var progress = FindObjectOfType<GameProgress>();
        if (progress != null)
            progress.UpdatePyramidState();

        // 3. Feedback contextual al jugador
        if (UIMessageManager.Instance != null)
        {
            if (GameProgress.AreAllMiniGamesDone())
            {
                UIMessageManager.Instance.ShowPriority("¡HAS DESBLOQUEADO LA PIRÁMIDE FINAL!", 4f);
            }
            else
            {
                UIMessageManager.Instance.ShowPriority($"MINIJUEGO {miniGameIndex} COMPLETADO\nVE AL SIGUIENTE", 3f);
            }
        }

        // 4. Teletransportar al jugador al Hub de forma segura
        TeleportToHubHelper.SafeTeleport(other.gameObject);

        // 5. Efecto visual de respawn en el Hub
        if (respawnEffect != null && GameProgress.staticSpawnHub != null)
        {
            Vector3 spawnPos = GameProgress.staticSpawnHub.position + Vector3.up * 0.5f;
            GameObject fx = Instantiate(respawnEffect, spawnPos, Quaternion.identity);
            Destroy(fx, 3f);
        }

        Debug.Log($"Minijuego {miniGameIndex} completado. Jugador devuelto al Hub.");
    }
}