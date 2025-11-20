using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // ---- LISTA DE PRISMAS RECOGIDOS EN NIVEL 3 ----
    public List<string> collectedPrisms = new List<string>();
    public int totalPrisms = 5;

    // ---- CLASE PARA GUARDAR TIEMPOS ----
    [System.Serializable]
    public class LevelTimeData
    {
        // Diccionario con nombreDelNivel : tiempo
        public Dictionary<string, float> levelTimes = new Dictionary<string, float>();
    }

    private LevelTimeData timeData = new LevelTimeData();
    private string savePath;

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Ruta donde se guarda el archivo JSON
            savePath = Application.persistentDataPath + "/level_times.json";

            LoadTimes();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Guarda el tiempo de un nivel
    public void SaveLevelTime(string levelName, float time)
    {
        timeData.levelTimes[levelName] = time;

        string json = JsonUtility.ToJson(timeData, true);
        File.WriteAllText(savePath, json);

        Debug.Log("TIEMPO GUARDADO (" + levelName + ") = " + time + " segundos");
    }

    // Carga los tiempos desde el JSON
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

    // Devuelve el tiempo guardado de un nivel
    public float GetLevelTime(string levelName)
    {
        if (timeData.levelTimes.ContainsKey(levelName))
            return timeData.levelTimes[levelName];

        return -1f;
    }

    // Guarda un prisma recogido
    public void AddPrism(string id)
    {
        collectedPrisms.Add(id);
        Debug.Log("Prisma recogido: " + id);
    }

    // Devuelve cuántos prismas se han recogido
    public int GetCollectedCount()
    {
        return collectedPrisms.Count;
    }
}
