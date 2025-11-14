using UnityEngine;

public class PortalActivator : MonoBehaviour
{
    [Header("ID único de este portal (1 o 2)")]
    public int portalID = 1;

    [Header("Referencia al gestor del puente")]
    public BridgeManager bridgeManager;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (activated) return; // ya se activó antes

        activated = true;
        bridgeManager.ActivatePortal(portalID);

        // 📢 Mensajes según progreso
        if (UIMessageManager.Instance != null)
        {
            if (!bridgeManager.AreBothPortalsActive())
            {
                UIMessageManager.Instance.ShowMessage("ACTIVA EL OTRO PORTAL");
            }
            else
            {
                UIMessageManager.Instance.ShowMessage("¡PUENTE ACTIVADO!");
            }
        }

        Debug.Log("Portal " + portalID + " activado.");
    }
}
