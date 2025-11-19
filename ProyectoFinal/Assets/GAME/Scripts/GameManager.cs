using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //Lista prisms escena 3
    public List<string> collectedPrisms = new List<string>();
    public int totalPrisms = 5;

    // 🔥 NUEVO: Tiempo final del nivel
    public float lastLevelTime = 0;

    // 🔥 Ruta del archivo JSON
    private string savePath;

    [System.Serializable]
    public class SaveData
    {
        public float lastLevelTime;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/saveData.json";
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ================================
    // 🔥 GUARDAR TIEMPO EN JSON
    // ================================
    public void SaveLevelTime(float time)
    {
        lastLevelTime = time;

        SaveData data = new SaveData();
        data.lastLevelTime = time;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log("⏱ Tiempo guardado en JSON: " + time + " segundos");
        Debug.Log("Archivo guardado en: " + savePath);
    }

    // ================================
    // 🔥 CARGAR JSON
    // ================================
    public void LoadData()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No hay archivo JSON todavía.");
            return;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        lastLevelTime = data.lastLevelTime;

        Debug.Log("⏱ Tiempo cargado desde JSON: " + lastLevelTime);
    }

    // Nivel 3 prisms (YA LO TENÍAS)
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
