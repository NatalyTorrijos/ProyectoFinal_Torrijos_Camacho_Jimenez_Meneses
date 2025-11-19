using UnityEngine;

public class GameProgress : MonoBehaviour
{
    public static bool miniGame1Completed = false;
    public static bool miniGame2Completed = false;
    public static bool miniGame3Completed = false;

    [Header("Pirámide")]
    public GameObject pyramidLocked;
    public GameObject pyramidUnlocked;

    [Header("Spawn Principal (Hub)")]
    public Transform spawnHub;
    public static Transform staticSpawnHub;

    private void Awake()
    {
        staticSpawnHub = spawnHub;
    }

    private void Start()
    {
        UpdatePyramidState();
    }

    // =============================
    // 🔥 Registrar minijuegos
    // =============================
    public static void CompleteMiniGame(int index)
    {
        if (index == 1) miniGame1Completed = true;
        else if (index == 2) miniGame2Completed = true;
        else if (index == 3) miniGame3Completed = true;
    }

    // =============================
    // 🔥 Condición para la pirámide
    // =============================
    public static bool AreMiniGamesForPyramidDone()
    {
        return miniGame1Completed && miniGame2Completed;
    }

    // =============================
    // 🔥 Estado visual de la pirámide
    // =============================
    public void UpdatePyramidState()
    {
        bool unlocked = AreMiniGamesForPyramidDone();

        if (pyramidLocked != null)
            pyramidLocked.SetActive(!unlocked);

        if (pyramidUnlocked != null)
            pyramidUnlocked.SetActive(unlocked);
    }

    // =============================
    // 🔥 Todos los minijuegos del nivel
    // =============================
    public static bool AreAllMiniGamesDone()
    {
        return miniGame1Completed && miniGame2Completed && miniGame3Completed;
    }

    // =============================
    // 🔥 Teleport
    // =============================
    public static void TeleportToHub(GameObject player)
    {
        if (staticSpawnHub == null || player == null)
        {
            Debug.LogWarning("No se encontró spawnHub o player.");
            return;
        }

        player.transform.position = staticSpawnHub.position;
    }
}
