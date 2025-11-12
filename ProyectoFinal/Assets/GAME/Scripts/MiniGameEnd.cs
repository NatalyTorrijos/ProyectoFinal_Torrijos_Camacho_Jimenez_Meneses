using UnityEngine;

public class MiniGameEnd : MonoBehaviour
{
    [Header("Número del minijuego (1 o 2)")]
    [Tooltip("Indica qué minijuego es: 1 o 2")]
    public int miniGameIndex = 1;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;

            // 1️⃣ Marca el minijuego como completado
            GameProgress.CompleteMiniGame(miniGameIndex);

            // 2️⃣ Actualiza el estado de la pirámide
            var progress = FindObjectOfType<GameProgress>();
            if (progress != null)
                progress.UpdatePyramidState();

            // 3️⃣ Teletransporta al jugador al Hub con método SEGURO
            TeleportToHubHelper.SafeTeleport(other.gameObject);

            Debug.Log($"✅ Minijuego {miniGameIndex} completado y jugador teletransportado al hub.");
        }
    }
}
