using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    
    public string itemID; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ItemManager.instance.CollectItem(itemID);

            Destroy(gameObject);
        }
    }
}
