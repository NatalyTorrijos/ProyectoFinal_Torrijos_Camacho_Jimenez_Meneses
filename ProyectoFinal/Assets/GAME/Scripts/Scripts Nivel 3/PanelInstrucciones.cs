using UnityEngine;
/// <summary>
/// Controla la visibilidad de un panel de mecánicas al detectar al jugador.
/// Muestra el panel cuando el jugador entra en el área del trigger
/// y lo oculta cuando sale.
/// </summary>

public class OpenPanelOnTrigger : MonoBehaviour
{
    public GameObject panelMecanicas;   

    void Start()
    {
        if (panelMecanicas != null)
            panelMecanicas.SetActive(false);  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            panelMecanicas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            panelMecanicas.SetActive(false);
        }
    }
}

