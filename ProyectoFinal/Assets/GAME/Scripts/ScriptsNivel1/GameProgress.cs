using UnityEngine;
using System.IO;

/// <summary>
/// Controlador global y persistente del progreso del jugador.
/// Implementa patrón Singleton + DontDestroyOnLoad para mantenerse activo entre escenas.
/// Gestiona:
/// • Estado de los tres minijuegos
/// • Desbloqueo visual de la pirámide
/// • Teletransporte al Hub
/// • Guardado y carga automática del progreso en formato JSON
/// </summary>
public class GameProgress : MonoBehaviour
{
    // ===================================================================
    // SINGLETON PERSISTENTE
    // ===================================================================
    public static GameProgress Instance;                     // Acceso global único

    private void Awake()
    {
        // Implementación del patrón Singleton con persistencia entre escenas
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);                  // Mantiene el objeto al cambiar de escena
            staticSpawnHub = spawnHub;                      // Guarda referencia estática del punto de spawn
            LoadGame();                                     // Carga el progreso guardado al iniciar el juego
        }
        else
        {
            Destroy(gameObject);                            // Elimina duplicados si se recarga la escena
        }
    }

    // ===================================================================
    // ESTADO DE LOS MINIJUEGOS (persistente)
    // ===================================================================
    [Header("Estado de Minijuegos")]
    public static bool miniGame1Completed = false;
    public static bool miniGame2Completed = false;
    public static bool miniGame3Completed = false;

    // ===================================================================
    // VISUALES DE LA PIRÁMIDE
    // ===================================================================
    [Header("Pirámide")]
    [Tooltip("Objeto visible cuando la pirámide está bloqueada")]
    public GameObject pyramidLocked;
    [Tooltip("Objeto visible cuando la pirámide está desbloqueada")]
    public GameObject pyramidUnlocked;

    // ===================================================================
    // PUNTO DE RESPAWN EN EL HUB
    // ===================================================================
    [Header("Spawn Principal (Hub)")]
    public Transform spawnHub;
    public static Transform staticSpawnHub;                  // Referencia estática accesible desde cualquier script

    private void Start()
    {
        UpdatePyramidState();                               // Actualiza visuales al iniciar la escena
    }

    // ===================================================================
    // REGISTRO DE MINIJUEGOS COMPLETADOS
    // ===================================================================
    /// <summary>
    /// Marca un minijuego como completado y guarda automáticamente el progreso.
    /// </summary>
    /// <param name="index">1 = Minijuego 1, 2 = Minijuego 2, 3 = Minijuego 3</param>
    public static void CompleteMiniGame(int index)
    {
        if (index == 1) miniGame1Completed = true;
        else if (index == 2) miniGame2Completed = true;
        else if (index == 3) miniGame3Completed = true;

        // Guardado automático cada vez que se completa un minijuego
        Instance.SaveGame();
    }

    // ===================================================================
    // CONDICIÓN PARA DESBLOQUEAR LA PIRÁMIDE
    // ===================================================================
    /// <summary>
    /// Indica si los minijuegos 1 y 2 están completados (requisito para entrar a la pirámide).
    /// </summary>
    public static bool AreMiniGamesForPyramidDone()
    {
        return miniGame1Completed && miniGame2Completed;
    }

    // ===================================================================
    // ACTUALIZACIÓN VISUAL DE LA PIRÁMIDE
    // ===================================================================
    /// <summary>
    /// Activa/desactiva los objetos visuales de la pirámide según su estado de desbloqueo.
    /// </summary>
    public void UpdatePyramidState()
    {
        bool unlocked = AreMiniGamesForPyramidDone();
        if (pyramidLocked != null) pyramidLocked.SetActive(!unlocked);
        if (pyramidUnlocked != null) pyramidUnlocked.SetActive(unlocked);
    }

    // ===================================================================
    // VERIFICACIÓN DE PROGRESO TOTAL
    // ===================================================================
    /// <summary>
    /// Devuelve true si los tres minijuegos han sido completados.
    /// </summary>
    public static bool AreAllMiniGamesDone()
    {
        return miniGame1Completed && miniGame2Completed && miniGame3Completed;
    }

    // ===================================================================
    // TELETRANSPORTE AL HUB
    // ===================================================================
    /// <summary>
    /// Teletransporta al jugador al punto de spawn principal del Hub.
    /// </summary>
    /// <param name="player">GameObject del jugador</param>
    public static void TeleportToHub(GameObject player)
    {
        if (staticSpawnHub == null || player == null)
        {
            Debug.LogWarning("No se encontró spawnHub o player.");
            return;
        }
        player.transform.position = staticSpawnHub.position;
    }

    // ===================================================================
    // PERSISTENCIA EN JSON
    // ===================================================================
    [System.Serializable]
    private class SaveData
    {
        public bool m1, m2, m3;
    }

    /// <summary>
    /// Guarda el estado actual de los minijuegos en un archivo JSON.
    /// </summary>
    public void SaveGame()
    {
        SaveData data = new SaveData
        {
            m1 = miniGame1Completed,
            m2 = miniGame2Completed,
            m3 = miniGame3Completed
        };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        Debug.Log("Progreso guardado: " + json);
    }

    /// <summary>
    /// Carga el progreso guardado desde el archivo JSON al iniciar el juego.
    /// </summary>
    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/save.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            miniGame1Completed = data.m1;
            miniGame2Completed = data.m2;
            miniGame3Completed = data.m3;
            UpdatePyramidState();                           // Actualiza visuales tras cargar
            Debug.Log("Progreso cargado correctamente");
        }
    }
}