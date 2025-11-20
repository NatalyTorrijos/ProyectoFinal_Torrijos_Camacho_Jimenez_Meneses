using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Paneles del menú")]
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public GameObject infoPanel;
    public GameObject creditsPanel;

    // ===========================
    // MENÚ PRINCIPAL
    // ===========================

    public void PlayGame()
    {
        SceneManager.LoadScene("NIVEL 1");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Salir del juego");
    }

    // ===========================
    // SUB-MENÚ DE NIVELES
    // ===========================

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

    // ===========================
    // PANEL DE INFORMACIÓN / HISTORIA
    // ===========================

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

    // ===========================
    // PANEL DE CRÉDITOS
    // ===========================

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

    // ===========================
    // CARGA DE NIVELES (para Level Select)
    // ===========================

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
