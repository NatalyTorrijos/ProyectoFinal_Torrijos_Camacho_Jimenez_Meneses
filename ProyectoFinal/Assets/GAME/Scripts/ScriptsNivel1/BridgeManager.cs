using UnityEngine;

public class BridgeManager : MonoBehaviour
{
    [Header("Puente que aparecerá")]
    public GameObject bridgeObject;

    [HideInInspector] public bool portal1Active = false;
    [HideInInspector] public bool portal2Active = false;

    private void Start()
    {
        if (bridgeObject != null)
            bridgeObject.SetActive(false); // inicia oculto
    }

    public void ActivatePortal(int portalID)
    {
        if (portalID == 1)
            portal1Active = true;
        else if (portalID == 2)
            portal2Active = true;

        CheckBridgeActivation();
    }

    public bool AreBothPortalsActive()
    {
        return portal1Active && portal2Active;
    }

    private void CheckBridgeActivation()
    {
        if (AreBothPortalsActive())
        {
            bridgeObject.SetActive(true);
            Debug.Log("✨ ¡Puente activado!");
        }
    }
}
