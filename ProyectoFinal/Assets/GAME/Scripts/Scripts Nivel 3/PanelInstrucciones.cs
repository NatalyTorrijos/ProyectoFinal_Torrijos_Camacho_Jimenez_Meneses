using UnityEngine;

public class OpenPanelOnTrigger : MonoBehaviour
{
    public GameObject panelMecanicas;   // Lo asignas desde el inspector

    void Start()
    {
        if (panelMecanicas != null)
            panelMecanicas.SetActive(false);   // Se asegura de que inicie apagado
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

