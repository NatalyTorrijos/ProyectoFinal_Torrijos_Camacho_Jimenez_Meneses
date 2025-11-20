using UnityEngine;
/// <summary>
/// Aqui se hace la logica para que player al colisionar con los objetos que tengan este script, se eliminen y en el Item Manager se tome como que o recogio.
/// </summary>

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
