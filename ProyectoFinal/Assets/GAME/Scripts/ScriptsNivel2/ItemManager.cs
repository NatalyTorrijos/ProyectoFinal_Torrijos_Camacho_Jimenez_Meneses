using UnityEngine;

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
