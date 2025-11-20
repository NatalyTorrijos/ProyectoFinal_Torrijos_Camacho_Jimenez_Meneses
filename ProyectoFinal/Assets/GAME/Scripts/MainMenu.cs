using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public GameObject infoPanel;
    public GameObject creditsPanel;

    // Cargar nivel 1 desde PLAY
    public void PlayGame()
    {
        SceneManager.LoadScene("NIVEL 1");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Salir del juego");
    }

    // Abrir submenú de niveles
    public void OpenLevelSelect()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    public void CloseLevelSelect()
    {
        levelSelectPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // Abrir panel de historia
    public void OpenInfoPanel()
    {
        mainMenuPanel.SetActive(false);
        infoPanel.SetActive(true);
    }

    public void CloseInfoPanel()
    {
        infoPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // Abrir créditos
    public void OpenCreditsPanel()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void CloseCreditsPanel()
    {
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // Cargar niveles desde Submenú
    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
