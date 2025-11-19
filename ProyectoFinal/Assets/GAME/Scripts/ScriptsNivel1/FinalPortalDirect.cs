using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalPortalDirect : MonoBehaviour
{
    public string sceneToLoad = "NIVEL 2";
    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;
        if (!other.CompareTag("Player")) return;

        activated = true;

        // 🔥 1. Obtener el tiempo final
        float finalTime = LevelTimer.Instance.GetElapsedTime();

        // 🔥 2. Guardarlo en el GameManager como JSON
        GameManager.Instance.SaveLevelTime(finalTime);

        // 🔥 3. Cargar siguiente escena
        SceneManager.LoadScene(sceneToLoad);
    }
}
