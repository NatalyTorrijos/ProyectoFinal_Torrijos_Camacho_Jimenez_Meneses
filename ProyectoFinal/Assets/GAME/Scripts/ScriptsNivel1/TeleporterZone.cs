using UnityEngine;
using System.Collections;

/// <summary>
/// Zona de teletransporte con condiciones de acceso y efectos visuales.
/// Permite teletransportar al jugador a un destino específico con:
/// • Bloqueo opcional si no se cumplen requisitos (ej: pirámide)
/// • Efectos de partículas en entrada y salida
/// • Seguridad física: desactiva temporalmente CharacterController para evitar bugs
/// • Feedback claro al jugador cuando el acceso está bloqueado
/// </summary>
public class TeleporterZone : MonoBehaviour
{
    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Header("Configuración del Teletransporte")]
    [Tooltip("Destino al que será teletransportado el jugador")]
    public Transform teleportDestination;

    [Tooltip("Si está activado, solo permite el teletransporte al completar minijuegos 1 y 2 (usado en la pirámide)")]
    public bool requireCompletion = false;

    [Tooltip("Pequeño retraso tras el teletransporte para estabilizar física")]
    public float teleportDelay = 0.2f;

    [Header("Efectos Visuales")]
    [Tooltip("Prefab de partículas que se reproduce al entrar y al salir del teletransporte")]
    public GameObject teleportEffect;

    // ===================================================================
    // ESTADO INTERNO
    // ===================================================================
    private bool isTeleporting = false;         // Evita teletransportes múltiples simultáneos

    // ===================================================================
    // DETECCIÓN DE ENTRADA DEL JUGADOR
    // ===================================================================
    /// <summary>
    /// Se activa cuando el jugador entra en el trigger del teletransporte.
    /// Verifica condiciones y lanza la corrutina segura de teletransporte.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // Prevenir activación múltiple
        if (isTeleporting) return;
        if (!other.CompareTag("Player")) return;

        // Validar que haya destino asignado
        if (teleportDestination == null)
        {
            Debug.LogWarning("TeleporterZone: no hay teleportDestination asignado en " + name);
            return;
        }

        // Bloqueo condicional: solo para la pirámide
        if (requireCompletion && !GameProgress.AreMiniGamesForPyramidDone())
        {
            UIMessageManager.Instance?.ShowMessage("TE FALTA COMPLETAR UN MINIJUEGO.\nVE A LOS OTROS EDIFICIOS");
            Debug.Log("Acceso bloqueado a la pirámide");
            return;
        }

        // Iniciar teletransporte seguro
        StartCoroutine(SafeTeleportRoutine(other.gameObject));
    }

    // ===================================================================
    // CORRUTINA DE TELETRANSPORTE SEGURO
    // ===================================================================
    /// <summary>
    /// Realiza el teletransporte con seguridad física:
    /// - Desactiva CharacterController
    /// - Reproduce efectos visuales
    /// - Mueve al jugador
    /// - Reactiva controles tras breve delay
    /// </summary>
    private IEnumerator SafeTeleportRoutine(GameObject player)
    {
        isTeleporting = true;

        CharacterController cc = player.GetComponent<CharacterController>();
        PlayerMovement movement = player.GetComponent<PlayerMovement>();

        // Desactivar CharacterController para evitar conflictos físicos
        if (cc != null) cc.enabled = false;

        // Efecto de entrada
        if (teleportEffect != null)
        {
            var entryFX = Instantiate(teleportEffect, player.transform.position, Quaternion.identity);
            Destroy(entryFX, 3f);
        }

        // Teletransportar con pequeña elevación para evitar quedar atrapado
        player.transform.position = teleportDestination.position + Vector3.up * 0.5f;

        // Efecto de salida
        if (teleportEffect != null)
        {
            var exitFX = Instantiate(teleportEffect, teleportDestination.position, Quaternion.identity);
            Destroy(exitFX, 3f);
        }

        // Pequeña espera para estabilizar física
        yield return new WaitForSeconds(teleportDelay);

        // Reactivar controles
        if (cc != null)
        {
            cc.enabled = true;
            cc.Move(Vector3.zero);
        }

        if (movement != null)
            movement.OnRespawn();

        Debug.Log($"Teletransportado correctamente a {teleportDestination.name}");
        isTeleporting = false;
    }
}