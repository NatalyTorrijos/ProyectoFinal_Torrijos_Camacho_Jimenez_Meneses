using System.Collections;
using UnityEngine;

public class TeleportToHubHelper : MonoBehaviour
{
    public static void SafeTeleport(GameObject player)
    {
        if (player == null) return;
        var helper = player.GetComponent<TeleportToHubHelper>();
        if (helper == null)
            helper = player.AddComponent<TeleportToHubHelper>();

        helper.StartCoroutine(helper.SafeTeleportRoutine(player));
    }

    private IEnumerator SafeTeleportRoutine(GameObject player)
    {
        CharacterController cc = player.GetComponent<CharacterController>();
        var movement = player.GetComponent<PlayerMovement>();

        if (cc != null) cc.enabled = false;

        // Teletransportar al hub principal
        GameProgress.TeleportToHub(player);
        player.transform.position += Vector3.up * 0.5f;

        // Esperar un momento para que Unity actualice físicas
        yield return new WaitForSeconds(0.2f);

        if (cc != null)
        {
            cc.enabled = true;
            cc.Move(Vector3.zero);
        }

        if (movement != null)
            movement.OnRespawn();

        Debug.Log("✅ Teletransporte al Hub completado correctamente.");
    }
}
