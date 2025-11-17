using UnityEngine;

public class MiniGameEnd : MonoBehaviour
{
    [Header("Número del minijuego (1 o 2)")]
    [Tooltip("Indica qué minijuego es: 1 o 2")]
    public int miniGameIndex = 1;

    [Header("Efectos visuales")]
    [Tooltip("Prefab de partículas que se reproducen al volver al Hub")]
    public GameObject respawnEffect;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;
        if (!other.CompareTag("Player")) return;

        activated = true;

        // 1️⃣ Marca el minijuego como completado
        GameProgress.CompleteMiniGame(miniGameIndex);

        // 2️⃣ Actualiza la pirámide
        var progress = FindObjectOfType<GameProgress>();
        if (progress != null)
            progress.UpdatePyramidState();

        // 3️⃣ Mostrar mensaje adecuado
        if (UIMessageManager.Instance != null)
        {
            if (GameProgress.AreAllMiniGamesDone())
                UIMessageManager.Instance.ShowMessage("¡HAS ACTIVADO LA PIRÁMIDE PRINCIPAL!");
            else
                UIMessageManager.Instance.ShowMessage("MINIJUEGO COMPLETADO. VE A LA PIRAMIDE.");
        }

        // 4️⃣ Teletransportar al jugador al Hub
        TeleportToHubHelper.SafeTeleport(other.gameObject);

        // 🌀 Instanciar partículas de respawn en el Hub
        if (respawnEffect != null && GameProgress.staticSpawnHub != null)
        {
            Vector3 spawnPos = GameProgress.staticSpawnHub.position + Vector3.up * 0.5f;
            GameObject fx = Instantiate(respawnEffect, spawnPos, Quaternion.identity);
            Destroy(fx, 3f);
        }

        Debug.Log($"✅ Minijuego {miniGameIndex} completado y jugador teletransportado al hub.");
    }
}
