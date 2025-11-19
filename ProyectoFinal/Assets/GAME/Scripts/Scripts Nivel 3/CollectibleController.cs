using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    public static CollectibleController Instance;   // Acceso global seguro

    public List<Transform> spawnPoints;
    public GameObject collectiblePrefab;
    public GameObject keyObject;

    private int currentIndex = 0;

    private void Awake()
    {
        Instance = this; // Todos los coleccionables podrán usar esto
    }

    void Start()
    {
        keyObject.SetActive(false);
        SpawnNextCollectible();
    }

    public void SpawnNextCollectible()
    {
        if (currentIndex < spawnPoints.Count)
        {
            Instantiate(collectiblePrefab,
                        spawnPoints[currentIndex].position,
                        Quaternion.identity);
            currentIndex++;
        }
        else
        {
            keyObject.SetActive(true); // Activar llave al final
        }
    }
}
