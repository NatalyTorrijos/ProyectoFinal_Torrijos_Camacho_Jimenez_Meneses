using UnityEngine;
using System.Collections;

public class TeleporterZone : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform teleportDestination;
    public bool requireCompletion = false; // Para pirámide = true
    public float teleportDelay = 0.2f;

    [Header("Visual Effects")]
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

        // 🔥 Verificar SOLO minijuegos 1 y 2
        if (requireCompletion && !GameProgress.AreMiniGamesForPyramidDone())
        {
            if (UIMessageManager.Instance != null)
                UIMessageManager.Instance.ShowMessage("TE FALTA COMPLETAR UN MINIJUEGO.\n VE A LOS OTROS EDIFICIOS");

            Debug.Log("Acceso bloqueado a la pirámide");
            return;
        }

        StartCoroutine(SafeTeleportRoutine(other.gameObject));
    }

    private IEnumerator SafeTeleportRoutine(GameObject player)
    {
        isTeleporting = true;

        CharacterController cc = player.GetComponent<CharacterController>();
        PlayerMovement movement = player.GetComponent<PlayerMovement>();

        if (cc != null) cc.enabled = false;

        if (teleportEffect != null)
        {
            var entryFX = Instantiate(teleportEffect, player.transform.position, Quaternion.identity);
            Destroy(entryFX, 3f);
        }

        player.transform.position = teleportDestination.position + Vector3.up * 0.5f;

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

        Debug.Log($"Teletransportado a {teleportDestination.name}");
        isTeleporting = false;
    }
}
