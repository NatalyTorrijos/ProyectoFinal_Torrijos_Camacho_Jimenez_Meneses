using UnityEngine;
using UnityEngine.SceneManagement;

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
    {
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
