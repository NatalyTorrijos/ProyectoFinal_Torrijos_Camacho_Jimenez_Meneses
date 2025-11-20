using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Controla la generación secuencial de coleccionables en puntos específicos.
/// Instancia un nuevo prisma en cada ubicación definida y,
/// cuando todos han sido generados, activa la llave final.
/// </summary>

public class CollectibleController : MonoBehaviour
{
    public static CollectibleController Instance;   
    public List<Transform> spawnPoints;
    public GameObject collectiblePrefab;
    public GameObject keyObject;

    private int currentIndex = 0;
    /// <summary>
    /// Asigna la instancia del CollectibleController para permitir acceso global.
    /// </summary>

    private void Awake()
    {
        Instance = this; 
    }

    void Start()
    {
        keyObject.SetActive(false);
        SpawnNextCollectible();
    }
    /// <summary>
    /// Instancia el siguiente coleccionable en la lista de puntos de aparición.
    /// Cuando no quedan más puntos disponibles, activa la llave en la escena.
    /// </summary>

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
