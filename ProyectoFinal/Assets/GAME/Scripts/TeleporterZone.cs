using UnityEngine;

public class TeleporterZone : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform teleportDestination; // a dónde irá el jugador
    public bool requireCompletion = false; // si este portal solo se activa al completar los minijuegos

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Si necesita que se completen los minijuegos antes
            if (requireCompletion && !GameProgress.AreAllMiniGamesDone())
            {
                Debug.Log("? La pirámide aún está bloqueada.");
                return;
            }

            // Teletransportar jugador
            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false; // desactivar antes de mover
                other.transform.position = teleportDestination.position;
                cc.enabled = true;
            }

            Debug.Log($"? Teletransportado a {teleportDestination.name}");
        }
    }
}
