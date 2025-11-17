using UnityEngine;

public class SceneController : MonoBehaviour
{
    public GameObject[] prismPrefabs;      // 1 prisma por orden
    public Transform[] prismSpawnPoints;   // dónde aparece cada prisma

    public GameObject keyPrefab;           // llave final
    public Transform keySpawnPoint;        // lugar de la llave

    void Start()
    {
        SpawnNextPrism();
    }

    public void SpawnNextPrism()
    {
        int count = GameManager.Instance.GetCollectedCount();

        // Si faltan prismas → instanciar el siguiente
        if (count < prismPrefabs.Length)
        {
            Instantiate(prismPrefabs[count],
                        prismSpawnPoints[count].position,
                        Quaternion.identity);
        }
        else
        {
            // Si ya recogiste todos → spawnear la llave
            Instantiate(keyPrefab, keySpawnPoint.position, Quaternion.identity);
            Debug.Log("¡Llave generada!");
        }
    }
}

