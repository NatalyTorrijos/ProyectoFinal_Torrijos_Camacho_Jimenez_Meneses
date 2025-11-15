using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("ID del objeto")]
    public string itemID;  // Bag_3, Fishing_Rod

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ItemManager.instance.CollectItem(itemID);

            Destroy(gameObject);
        }
    }
}
