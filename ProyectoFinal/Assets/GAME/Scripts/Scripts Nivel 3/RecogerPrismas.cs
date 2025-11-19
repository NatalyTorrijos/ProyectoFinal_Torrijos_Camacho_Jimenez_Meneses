using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectibleController.Instance.SpawnNextCollectible();
            Destroy(gameObject);
        }
    }
}


