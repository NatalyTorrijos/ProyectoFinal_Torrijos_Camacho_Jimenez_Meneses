using UnityEngine;

/// <summary>
/// Componente activador de portal para el minijuego del puente (minijuego 3).
/// Cada portal tiene un ID único (1 o 2). Al ser tocado por el jugador:
/// • Notifica al BridgeManager para registrar su activación
/// • Solo se activa una vez
/// • Muestra feedback contextual según el estado del otro portal
/// 
/// Forma parte del sistema de doble activación requerido para bajar el puente.
/// </summary>
public class PortalActivator : MonoBehaviour
{
    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Header("Identificación del Portal")]
    [Tooltip("ID único del portal. Debe ser 1 o 2 (coincidir con el otro portal del mismo minijuego)")]
    public int portalID = 1;

    [Header("Gestor del Puente")]
    [Tooltip("Referencia al BridgeManager que controla el estado de los portales y el puente")]
    public BridgeManager bridgeManager;

    // ===================================================================
    // ESTADO INTERNO
    // ===================================================================
    private bool activated = false;  // Evita activación múltiple

    // ===================================================================
    // DETECCIÓN DE CONTACTO CON EL JUGADOR
    // ===================================================================
    /// <summary>
    /// Se ejecuta cuando el jugador entra en el trigger del portal.
    /// Solo responde al jugador y solo la primera vez.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // Ignorar todo lo que no sea el jugador
        if (!other.CompareTag("Player")) return;

        // Prevenir activación repetida
        if (activated) return;

        // Marcar como activado
        activated = true;

        // Notificar al BridgeManager
        if (bridgeManager != null)
        {
            bridgeManager.ActivatePortal(portalID);
        }

        // ===================================================================
        // FEEDBACK AL JUGADOR SEGÚN EL ESTADO DEL PUZLE
        // ===================================================================
        if (UIMessageManager.Instance != null)
        {
            if (bridgeManager != null && !bridgeManager.AreBothPortalsActive())
            {
                UIMessageManager.Instance.ShowMessage("ACTIVA EL OTRO PORTAL");
            }
            else
            {
                UIMessageManager.Instance.ShowMessage("PUENTE ACTIVADO!");
            }
        }

        Debug.Log($"Portal {portalID} activado.");
    }
}