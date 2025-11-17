using UnityEngine;
using System.Collections;

public class TeleporterZone : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform teleportDestination;
    public bool requireCompletion = false;
    public float teleportDelay = 0.2f;

    [Header("Visual Effects")]
    [Tooltip("Partículas que se reproducen al teletransportar (se instancian temporalmente).")]
    public GameObject teleportEffect;

    private bool isTeleporting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isTeleporting) return;
        if (!other.CompareTag("Player")) return;
        if (teleportDestination == null)
        {
            Debug.LogWarning("TeleporterZone: no hay teleportDestination asignado en " + name);
            return;
        }

        // Si requiere completar minijuegos antes (por ejemplo, pirámide)
        if (requireCompletion && !GameProgress.AreAllMiniGamesDone())
        {
            if (UIMessageManager.Instance != null)
                UIMessageManager.Instance.ShowMessage("TE FALTA UN MINIJUEGO PARA ACCEDER A LA PIRÁMIDE");
            Debug.Log("LA PIRÁMIDE AÚN ESTÁ BLOQUEADA");
            return;
        }

        // Teletransportar con efecto visual
        StartCoroutine(SafeTeleportRoutine(other.gameObject));
    }

    private IEnumerator SafeTeleportRoutine(GameObject player)
    {
        isTeleporting = true;

        CharacterController cc = player.GetComponent<CharacterController>();
        PlayerMovement movement = player.GetComponent<PlayerMovement>();

        if (cc != null) cc.enabled = false;

        // 🔥 Instanciar efecto de partículas en el punto de entrada
        if (teleportEffect != null)
        {
            var entryFX = Instantiate(teleportEffect, player.transform.position, Quaternion.identity);
            Destroy(entryFX, 3f);
        }

        // Teletransportar
        player.transform.position = teleportDestination.position + Vector3.up * 0.5f;

        // 🔥 Instanciar efecto de partículas en el destino
        if (teleportEffect != null)
        {
            var exitFX = Instantiate(teleportEffect, teleportDestination.position, Quaternion.identity);
            Destroy(exitFX, 3f);
        }

        yield return new WaitForSeconds(teleportDelay);

        if (cc != null)
        {
            cc.enabled = true;
            cc.Move(Vector3.zero);
        }

        if (movement != null)
            movement.OnRespawn();

        Debug.Log($"✅ Teletransportado a {teleportDestination.name}");
        isTeleporting = false;
    }
}
