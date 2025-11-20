using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Controlador principal del juego. Gestiona la persistencia de datos entre escenas,
/// el tiempo del último nivel completado y la recolección de prismas en el nivel 3.
/// Implementa patrón Singleton y DontDestroyOnLoad para mantenerse activo durante toda la ejecución.
/// </summary>
public class GameManager : MonoBehaviour
{
    // ===================================================================
    // INSTANCIA ÚNICA (Singleton)
    // ===================================================================
    public static GameManager Instance;

    // ===================================================================
    // DATOS DE RECOLECCIÓN - NIVEL 3 (Prismas)
    // ===================================================================
    [Header("Recolección de Prismas - Nivel 3")]
    [Tooltip("Lista que almacena los IDs de todos los prismas que el jugador ha recogido")]
    public List<string> collectedPrisms = new List<string>();

    [Tooltip("Número total de prismas existentes en el nivel 3")]
    public int totalPrisms = 5;

    // ===================================================================
    // DATOS DE TIEMPO
    // ===================================================================
    [Header("Tiempo del Nivel")]
    [Tooltip("Tiempo empleado por el jugador en el último nivel completado (en segundos)")]
    public float lastLevelTime = 0f;

    // ===================================================================
    // RUTA DE ARCHIVO JSON
    // ===================================================================
    [Header("Persistencia de Datos")]
    private string savePath;

    // ===================================================================
    // ESTRUCTURA DE DATOS PARA GUARDAR EN JSON
    // ===================================================================
    [System.Serializable]
    public class SaveData
    {
        public float lastLevelTime;                    // Tiempo del último nivel completado
        // NOTA: Los prismas se guardan por separado en el futuro (extensible)
    }

    // ===================================================================
    // INICIALIZACIÓN DEL GAME MANAGER
    // ===================================================================
    void Awake()
    {
        // Implementación del patrón Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);                    // Persiste entre cambios de escena
            savePath = Application.persistentDataPath + "/saveData.json";  // Ruta segura en todas las plataformas
        }
        else
        {
            Destroy(gameObject);  // Elimina duplicados si se carga la escena de nuevo
        }
    }

    // ================================
    // GUARDAR TIEMPO EN ARCHIVO JSON
    // ================================
    /// <summary>
    /// Guarda el tiempo del nivel completado en un archivo JSON.
    /// Se sobrescribe cada vez que se completa un nivel (mejor tiempo queda registrado).
    /// </summary>
    /// <param name="time">Tiempo en segundos que tardó el jugador</param>
    public void SaveLevelTime(float time)
    {
        lastLevelTime = time;                                 // Actualiza variable local

        SaveData data = new SaveData();                       // Crea nueva estructura de datos
        data.lastLevelTime = time;                            // Asigna el tiempo

        string json = JsonUtility.ToJson(data, true);         // Convierte a JSON con formato legible
        File.WriteAllText(savePath, json);                    // Escribe en disco

        Debug.Log("Tiempo guardado en JSON: " + time + " segundos");
        Debug.Log("Archivo guardado en: " + savePath);
    }

    // ================================
    // CARGAR DATOS DESDE JSON
    // ================================
    /// <summary>
    /// Carga el tiempo guardado previamente desde el archivo JSON.
    /// Se ejecuta automáticamente al iniciar el juego si existe el archivo.
    /// </summary>
    public void LoadData()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No hay archivo JSON todavía.");
            return;
        }

        string json = File.ReadAllText(savePath);             // Lee todo el contenido
        SaveData data = JsonUtility.FromJson<SaveData>(json); // Convierte JSON a objeto

        lastLevelTime = data.lastLevelTime;                  // Restaura el tiempo guardado

        Debug.Log("Tiempo cargado desde JSON: " + lastLevelTime);
    }

    // ================================
    // GESTIÓN DE PRISMAS RECOGIDOS
    // ================================
    /// <summary>
    /// Agrega un prisma a la lista de objetos recolectados.
    /// Se usa cuando el jugador toca un prisma en el nivel 3.
    /// </summary>
    /// <param name="id">Identificador único del prisma (ej: "Prisma_01")</param>
    public void AddPrism(string id)
    {
        collectedPrisms.Add(id);                              // Añade al registro
        Debug.Log("Prisma recogido: " + id);
    }

    /// <summary>
    /// Devuelve la cantidad actual de prismas recogidos por el jugador.
    /// </summary>
    /// <returns>Número de prismas en la lista</returns>
    public int GetCollectedCount()
    {
        return collectedPrisms.Count;
    }
}