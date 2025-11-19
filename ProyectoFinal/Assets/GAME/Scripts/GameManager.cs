using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // ---- DATOS DE PRISMAS NIVEL 3 ----
    public List<string> collectedPrisms = new List<string>();
    public int totalPrisms = 5;

    // ---- DATOS DE TIEMPOS ----
    [System.Serializable]
    public class LevelTimeData
    {
        public Dictionary<string, float> levelTimes = new Dictionary<string, float>();
    }

    private LevelTimeData timeData = new LevelTimeData();
    private string savePath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            savePath = Application.persistentDataPath + "/level_times.json";
            LoadTimes();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // -------------------------------
    // 🔥 GUARDAR TIEMPO DE NIVEL
    // -------------------------------
    public void SaveLevelTime(string levelName, float time)
    {
        timeData.levelTimes[levelName] = time;

        string json = JsonUtility.ToJson(timeData, true);
        File.WriteAllText(savePath, json);

        Debug.Log("TIEMPO GUARDADO (" + levelName + ") = " + time + " segundos");
    }

    // -------------------------------
    // 🔥 CARGAR TIEMPOS
    // -------------------------------
    private void LoadTimes()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            timeData = JsonUtility.FromJson<LevelTimeData>(json);
            Debug.Log("Tiempos cargados correctamente.");
        }
        else
        {
            timeData = new LevelTimeData();
        }
    }

    // -------------------------------
    // 🔥 OBTENER TIEMPO DE NIVEL
    // -------------------------------
    public float GetLevelTime(string levelName)
    {
        if (timeData.levelTimes.ContainsKey(levelName))
            return timeData.levelTimes[levelName];

        return -1f;
    }

    // -------------------------------
    // 🔥 MANEJO DE PRISMAS (NIVEL 3)
    // -------------------------------
    public void AddPrism(string id)
    {
        collectedPrisms.Add(id);
        Debug.Log("Prisma recogido: " + id);
    }

    public int GetCollectedCount()
    {
        return collectedPrisms.Count;
    }
}
