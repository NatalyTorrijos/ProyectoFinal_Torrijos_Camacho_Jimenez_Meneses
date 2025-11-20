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
        Debug.Log("SpawnNextCollectible llamado. Índice actual: " + currentIndex);

       if (currentIndex < spawnPoints.Count)
        {
            GameObject col = Instantiate(
                collectiblePrefab,
                spawnPoints[currentIndex].position,
                Quaternion.identity
            );

            // ✔ Le decimos al coleccionable cuál checkpoint representa
            col.GetComponent<Collectible>().checkpointIndex = currentIndex;

            currentIndex++;
        }
        else
        {
            Debug.Log("No hay más prismas. Activando llave.");
            keyObject.SetActive(true);

        }
    }
   

}
