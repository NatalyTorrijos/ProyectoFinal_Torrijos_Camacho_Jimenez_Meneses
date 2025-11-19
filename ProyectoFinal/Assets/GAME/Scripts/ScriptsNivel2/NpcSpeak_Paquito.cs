using UnityEngine;

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
        // Mostrar panel Press E cuando el jugador esté cerca y no esté ya abierto el diálogo
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

    // Este método se llama al cerrar el panel (usado por botón cerrar)
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
