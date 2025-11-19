using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject continueButton;

    private void Start()
    {
        // Mostrar botón "Continuar" solo si hay progreso real
        if (continueButton != null)
            continueButton.SetActive(GameProgress.minijuego1Completed || GameProgress.minijuego2Completed);
    }

    public void PlayGame()
    {
        // Siempre inicia desde la escena principal del nivel
        SceneManager.LoadScene("NIVEL 1");
    }

    public void ContinueGame()
    {
        // Cargar última escena o el HUB
        SceneManager.LoadScene("Scene1");
    }

    public void OpenOptions()
    {
        Debug.Log("Abrir menú de Opciones");
    }

    public void OpenCredits()
    {
        Debug.Log("Abrir Créditos");
    }

    public void QuitGame()
    {
        Debug.Log("Salir del juego");
        Application.Quit();
    }
}
