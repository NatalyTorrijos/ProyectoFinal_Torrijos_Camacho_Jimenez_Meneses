using UnityEngine;

public class GameProgress : MonoBehaviour
{
    // ===========================
    // 📍 PROGRESO DE MINIJUEGOS
    // ===========================
    public static bool miniGame1Completed = false;
    public static bool miniGame2Completed = false;

    [Header("Pirámide")]
    public GameObject pyramidLocked;
    public GameObject pyramidUnlocked;

    // ===========================
    // 📍 PUNTO DE SPAWN PRINCIPAL (HUB)
    // ===========================
    [Header("Spawn Principal (Hub)")]
    public Transform spawnHub; // asigna aquí el cubo del hub
    public static Transform staticSpawnHub;

    private void Awake()
    {
        staticSpawnHub = spawnHub;
    }

    private void Start()
    {
        UpdatePyramidState();
    }

    // ===========================
    // 📍 MARCAR MINIJUEGOS COMO COMPLETADOS
    // ===========================
    public static void CompleteMiniGame(int index)
    {
        if (index == 1)
            miniGame1Completed = true;
        else if (index == 2)
            miniGame2Completed = true;
    }

    // ===========================
    // 📍 ACTUALIZAR ESTADO DE LA PIRÁMIDE
    // ===========================
    public void UpdatePyramidState()
    {
        bool allDone = miniGame1Completed && miniGame2Completed;

        if (pyramidLocked != null)
            pyramidLocked.SetActive(!allDone);

        if (pyramidUnlocked != null)
            pyramidUnlocked.SetActive(allDone);
    }

    // ===========================
    // 📍 VERIFICAR SI TODO ESTÁ COMPLETADO
    // ===========================
    public static bool AreAllMiniGamesDone()
    {
        return miniGame1Completed && miniGame2Completed;
    }

    // ===========================
    // 📍 TELETRANSPORTAR AL HUB
    // ===========================
    public static void TeleportToHub(GameObject player)
    {
        if (staticSpawnHub == null || player == null)
        {
            Debug.LogWarning("❌ No se encontró el spawnHub o el player para teletransportar.");
            return;
        }

        // Solo cambia la posición — el RespawnTrigger se encarga del CharacterController
        player.transform.position = staticSpawnHub.position;

        Debug.Log("✨ Teletransportado al Hub desde minijuego o trigger de respawn.");
    }
}
