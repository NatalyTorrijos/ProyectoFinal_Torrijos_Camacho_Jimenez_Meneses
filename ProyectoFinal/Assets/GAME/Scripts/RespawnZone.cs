using UnityEngine;
using System.Collections;

public class RespawnTrigger : MonoBehaviour
{
    [Header("Etiqueta del jugador")]
    public string playerTag = "Player";

    [Tooltip("Altura adicional para reaparecer sobre el suelo (evita quedar atrapado)")]
    public float respawnYOffset = 0.5f;

    [Tooltip("Pequeño delay para reactivar movimiento (en segundos)")]
    public float respawnDelay = 0.2f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        // Inicia la rutina de respawn
        StartCoroutine(HandleRespawnRoutine(other.gameObject));
    }

    private IEnumerator HandleRespawnRoutine(GameObject player)
    {
        Debug.Log("💀 Jugador tocó el trigger de respawn. Iniciando teletransporte...");

        CharacterController cc = player.GetComponent<CharacterController>();
        var playerMovement = player.GetComponent<PlayerMovement>();

        // 1️⃣ Desactivar el CharacterController antes de mover al jugador
        if (cc != null) cc.enabled = false;

        // 2️⃣ Teletransportar al hub
        GameProgress.TeleportToHub(player);

        // 3️⃣ Ajustar posición un poco por encima del suelo para evitar solapamiento
        player.transform.position += Vector3.up * respawnYOffset;

        // 4️⃣ Esperar un pequeño delay para que Unity actualice colisiones
        yield return new WaitForSeconds(respawnDelay);

        // 5️⃣ Reactivar el CharacterController y forzar un pequeño movimiento nulo
        if (cc != null)
        {
            cc.enabled = true;
            cc.Move(Vector3.zero);
        }

        // 6️⃣ Reiniciar estados del movimiento (resetea inputs, gravedad, animaciones)
        if (playerMovement != null)
        {
            playerMovement.OnRespawn();
        }

        Debug.Log("✨ Respawn completado. Jugador puede moverse nuevamente.");
    }
}
