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

    [Header("Efectos visuales")]
    [Tooltip("Prefab de partículas que se reproducen al reaparecer en el Hub")]
    public GameObject respawnEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        StartCoroutine(HandleRespawnRoutine(other.gameObject));
    }

    private IEnumerator HandleRespawnRoutine(GameObject player)
    {
        Debug.Log("💀 Jugador tocó el trigger de respawn. Iniciando teletransporte...");

        CharacterController cc = player.GetComponent<CharacterController>();
        var playerMovement = player.GetComponent<PlayerMovement>();

        if (cc != null) cc.enabled = false;

        // 1️⃣ Teletransportar al Hub
        GameProgress.TeleportToHub(player);

        // 2️⃣ Ajustar posición un poco por encima del suelo
        player.transform.position += Vector3.up * respawnYOffset;

        // 🌀 Instanciar partículas de respawn
        if (respawnEffect != null)
        {
            GameObject fx = Instantiate(respawnEffect, player.transform.position, Quaternion.identity);
            Destroy(fx, 3f);
        }

        // 3️⃣ Esperar un pequeño delay
        yield return new WaitForSeconds(respawnDelay);

        if (cc != null)
        {
            cc.enabled = true;
            cc.Move(Vector3.zero);
        }

        if (playerMovement != null)
            playerMovement.OnRespawn();

        Debug.Log("✨ Respawn completado. Jugador puede moverse nuevamente.");
    }
}
