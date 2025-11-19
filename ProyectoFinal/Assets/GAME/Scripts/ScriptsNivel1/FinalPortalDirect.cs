using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalPortalDirect : MonoBehaviour
{
    [Header("Escena a cargar al tocar el portal")]
    public string sceneToLoad = "NIVEL 2"; // <-- Cambia por el nombre exacto de tu escena

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;
        if (!other.CompareTag("Player")) return;

        activated = true;

        // Cargar directamente la escena final
        SceneManager.LoadScene(sceneToLoad);
    }
}
