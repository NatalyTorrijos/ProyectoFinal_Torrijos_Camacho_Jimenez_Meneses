using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //Lista primas escena 3
    public List<string> collectedPrisms = new List<string>();
    public int totalPrisms = 5;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
    }

    void Update()
    {
    }

    //Metodos Primas Nivel 3
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