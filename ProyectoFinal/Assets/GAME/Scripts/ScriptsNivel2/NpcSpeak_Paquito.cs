using UnityEngine;
/// <summary>
/// Aqui se hace la unica interaccion que tiene el player al iniciar el nivel2 con Paquito, abre el unico panel despues de darle a la e.
/// </summary>

public class NpcSpeak_Paquito : MonoBehaviour
{
    [Header("Paneles del NPC")]
    public GameObject PanelPaquito;
    public GameObject Panel_PressE;

    private bool playerInRange = false;

    void Start()
    {
        PanelPaquito.SetActive(false);
        Panel_PressE.SetActive(false);
    }

    void Update()
    {
        
        if (playerInRange && !PanelPaquito.activeSelf)
            Panel_PressE.SetActive(true);
        else
            Panel_PressE.SetActive(false);

        // Abrir diálogo al presionar E
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PanelPaquito.SetActive(true);
            Panel_PressE.SetActive(false);
        }
    }

    
    public void CerrarPanel()
    {
        PanelPaquito.SetActive(false);

        if (playerInRange)
            Panel_PressE.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            PanelPaquito.SetActive(false);
            Panel_PressE.SetActive(false);
        }
    }
}
