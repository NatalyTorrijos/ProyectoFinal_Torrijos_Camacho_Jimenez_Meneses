using UnityEngine;

public class PanelTrigger : MonoBehaviour
{
    public GameObject panelUI;   // El panel del Canvas que quieres activar

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador entró en el trigger → Abrir panel");
            panelUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador salió del trigger → Cerrar panel");
            panelUI.SetActive(false);
        }
    }
}
