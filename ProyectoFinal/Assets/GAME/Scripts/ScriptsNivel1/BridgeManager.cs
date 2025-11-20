using UnityEngine;

/// <summary>
/// Controlador de activación del puente entre portales.
/// Se encarga de:
/// • Mantener el estado de activación de dos portales (portal 1 y portal 2)
/// • Detectar cuándo ambos portales están activos simultáneamente
/// • Activar el puente visual cuando se cumple la condición de ambos portales
/// • Proporcionar métodos públicos para que otros scripts activen los portales
/// 
/// El puente permanece oculto hasta que ambos portales hayan sido activados.
/// </summary>
public class BridgeManager : MonoBehaviour
{
    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Header("Puente que aparecerá")]
    [Tooltip("GameObject del puente que se activará cuando ambos portales estén activos")]
    public GameObject bridgeObject;

    // ===================================================================
    // ESTADO INTERNO DE LOS PORTALES
    // ===================================================================
    [HideInInspector] public bool portal1Active = false;
    [HideInInspector] public bool portal2Active = false;

    // ===================================================================
    // INICIALIZACIÓN
    // ===================================================================
    private void Start()
    {
        // Iniciar con el puente oculto
        if (bridgeObject != null)
            bridgeObject.SetActive(false);
    }

    // ===================================================================
    // ACTIVACIÓN DE PORTALES
    // ===================================================================
    /// <summary>
    /// Activa un portal específico según su ID y verifica si el puente debe aparecer.
    /// </summary>
    /// <param name="portalID">ID del portal a activar (1 o 2)</param>
    public void ActivatePortal(int portalID)
    {
        if (portalID == 1)
            portal1Active = true;
        else if (portalID == 2)
            portal2Active = true;

        // Verificar si ambos portales ya están activos
        CheckBridgeActivation();
    }

    // ===================================================================
    // VERIFICACIÓN DE ESTADO
    // ===================================================================
    /// <summary>
    /// Verifica si ambos portales están activos simultáneamente.
    /// </summary>
    /// <returns>True si ambos portales están activos, False en caso contrario</returns>
    public bool AreBothPortalsActive()
    {
        return portal1Active && portal2Active;
    }

    // ===================================================================
    // ACTIVACIÓN DEL PUENTE
    // ===================================================================
    /// <summary>
    /// Verifica si ambos portales están activos y, de ser así, activa el puente.
    /// </summary>
    private void CheckBridgeActivation()
    {
        if (AreBothPortalsActive())
        {
            bridgeObject.SetActive(true);
            Debug.Log("¡PUENTE ACTIVADO!");
        }
    }
}