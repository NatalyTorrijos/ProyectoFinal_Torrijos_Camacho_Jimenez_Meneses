using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Aqui se hace la logica del portal, ya que no con solo recoger los objetos se activa, evalua si el player ya hablo por segunda vez, una vez se evalue que hablo por segunda vez se activa el portal para pasar al NIvel3.
/// </summary>

public class PortalActivatorS2 : MonoBehaviour
{
    [Header("Referencias")]
    public NpcSpeak_Merlin merlin;
    public GameObject portalVisual;
    public string nextSceneName = "NIVEL3";

    private bool waitingForDialogClose = false;

    void Start()
    {
        if (portalVisual != null)
            portalVisual.SetActive(false);
    }

    void Update()
    {            //-------------------Aqui se evalua cuando el player interactua con merlin por segunda vvez y se activa el portal.
        if (merlin == null) return;

        if (merlin.PanelMerlin2.activeSelf)
        {
            waitingForDialogClose = true;
        }

        if (waitingForDialogClose && !merlin.PanelMerlin2.activeSelf)
        {
            waitingForDialogClose = false;

            if (portalVisual != null)
            {
                portalVisual.SetActive(true);
                Debug.Log("🔥 Portal ACTIVADO después de cerrar el diálogo de Merlin.");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!portalVisual.activeSelf) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("🔥 Teletransportando al jugador a: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
