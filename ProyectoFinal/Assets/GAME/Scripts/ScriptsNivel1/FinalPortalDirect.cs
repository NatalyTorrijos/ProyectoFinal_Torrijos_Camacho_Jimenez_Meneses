using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalPortalDirect : MonoBehaviour
{
    [Header("Escena a cargar al tocar el portal")]
    public string sceneToLoad = "NIVEL 2";

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;
        if (!other.CompareTag("Player")) return;

        activated = true;

        // 🔥 Guardar tiempo del nivel antes de cambiar de escena
        LevelTimer timer = FindObjectOfType<LevelTimer>();
        if (timer != null)
        {
            float elapsed = timer.GetElapsedTime();
            string levelName = SceneManager.GetActiveScene().name;
            GameManager.Instance.SaveLevelTime(levelName, elapsed);
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
