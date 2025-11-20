using UnityEngine;
/// <summary>
/// en este script se hace la logica de los 2 objetos del nivel2, donde el player para ayudar a merlin tiene que nencontrar 2 objetos, la bag_3 y la DishingRod,  si los recogio o no.
/// </summary>

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    private bool bagCollected = false;
    private bool fishingRodCollected = false;

    [Header("Referencia al NPC Merlin")]
    public NpcSpeak_Merlin merlin;

    private void Awake()
    {
        instance = this;
    }

    public void CollectItem(string id)
    {
        if (id == "Bag_3")
        {
            bagCollected = true;
            Debug.Log("Objeto recogido: Bag_3");
        }

        if (id == "Fishing_Rod")
        {
            fishingRodCollected = true;
            Debug.Log("Objeto recogido: Fishing_Rod");
        }

        if (bagCollected && fishingRodCollected)
        {
            merlin.ActivarSiguienteDialogo();
        }
    }
}
