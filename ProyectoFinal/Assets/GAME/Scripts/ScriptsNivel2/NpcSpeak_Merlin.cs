using UnityEngine;

public class NpcSpeak_Merlin : MonoBehaviour
{
    [Header("Paneles del NPC")]
    public GameObject PanelMerlin1;  // Primer diálogo
    public GameObject PanelMerlin2;  // Segundo diálogo
    public GameObject Panel_PressE;

    private bool playerInRange = false;

    // Se activa desde ItemManager cuando el jugador recolecta ambos objetos
    private bool dialogoFinalDisponible = false;

    void Start()
    {
        PanelMerlin1.SetActive(false);
        PanelMerlin2.SetActive(false);
        Panel_PressE.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && !PanelMerlin1.activeSelf && !PanelMerlin2.activeSelf)
            Panel_PressE.SetActive(true);
        else
            Panel_PressE.SetActive(false);

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            AbrirDialogoCorrecto();
        }
    }

    void AbrirDialogoCorrecto()
    {
        if (!dialogoFinalDisponible)
        {
            PanelMerlin1.SetActive(true);
        }
        else
        {
            PanelMerlin2.SetActive(true);
        }

        Panel_PressE.SetActive(false);
    }

    public void CerrarPanel()
    {
        PanelMerlin1.SetActive(false);
        PanelMerlin2.SetActive(false);

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
            PanelMerlin1.SetActive(false);
            PanelMerlin2.SetActive(false);
            Panel_PressE.SetActive(false);
        }
    }

    // Llamado por ItemManager cuando el jugador tiene ambos objetos
    public void ActivarSiguienteDialogo()
    {
        dialogoFinalDisponible = true;
        Debug.Log("Merlin: ¡Tienes ambos objetos! Ahora puedes ver el diálogo final.");
    }
}
