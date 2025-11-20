using System.Collections;
using UnityEngine;

/// <summary>
/// Clase auxiliar estática que permite teletransportar al jugador al Hub de forma segura 
/// desde cualquier punto del código sin necesidad de tener una referencia previa al componente.
/// Se añade automáticamente al jugador si no existe y ejecuta una corrutina que:
/// • Desactiva temporalmente el CharacterController
/// • Usa GameProgress para el teletransporte
/// • Aplica pequeña elevación para evitar quedar atrapado
/// • Reactiva controles tras breve espera
/// </summary>
public class TeleportToHubHelper : MonoBehaviour
{
    // ===================================================================
    // MÉTODO ESTÁTICO PÚBLICO - LLAMADA PRINCIPAL
    // ===================================================================
    /// <summary>
    /// Teletransporta al jugador al Hub principal de forma segura.
    /// Si el componente no existe en el jugador, se añade automáticamente.
    /// </summary>
    /// <param name="player">GameObject del jugador (debe tener CharacterController y PlayerMovement)</param>
    public static void SafeTeleport(GameObject player)
    {
        if (player == null) return;

        // Añadir el helper si no existe (patrón "lazy add")
        var helper = player.GetComponent<TeleportToHubHelper>();
        if (helper == null)
            helper = player.AddComponent<TeleportToHubHelper>();

        // Iniciar corrutina segura
        helper.StartCoroutine(helper.SafeTeleportRoutine(player));
    }

    // ===================================================================
    // CORRUTINA PRIVADA - PROCESO SEGURO DE TELETRANSPORTE
    // ===================================================================
    /// <summary>
    /// Ejecuta el teletransporte con todas las precauciones físicas necesarias.
    /// Evita bugs comunes como quedarse atascado en el suelo o pérdida de control.
    /// </summary>
    private IEnumerator SafeTeleportRoutine(GameObject player)
    {
        // Obtener componentes necesarios
        CharacterController cc = player.GetComponent<CharacterController>();
        var movement = player.GetComponent<PlayerMovement>();

        // Desactivar CharacterController para evitar conflictos durante el movimiento instantáneo
        if (cc != null)
            cc.enabled = false;

        // Teletransportar al punto guardado en GameProgress (Hub)
        GameProgress.TeleportToHub(player);

        // Pequeña elevación para evitar quedar dentro del suelo
        player.transform.position += Vector3.up * 0.5f;

        // Esperar un frame para que Unity procese el cambio de posición
        yield return new WaitForSeconds(0.2f);

        // Reactivar CharacterController y estabilizar
        if (cc != null)
        {
            cc.enabled = true;
            cc.Move(Vector3.zero);  // Reset interno de física
        }

        // Notificar al sistema de movimiento que hubo respawn
        if (movement != null)
            movement.OnRespawn();

        Debug.Log("Teletransporte al Hub completado correctamente.");
    }
}