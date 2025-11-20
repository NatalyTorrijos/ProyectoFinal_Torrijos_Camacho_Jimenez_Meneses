using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controlador principal de la interfaz del Menú Principal del juego.
/// Gestiona la navegación entre todos los paneles del menú (Menú Principal, Selección de Niveles,
/// Información/Historia y Créditos) y la carga de niveles o salida del juego.
/// 
/// Todos los métodos están diseñados para ser llamados desde botones de UI (OnClick).
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    // ===================================================================
    // REFERENCIAS A PANELES DE LA INTERFAZ
    // ===================================================================
    [Header("Paneles del Menú Principal")]
    [Tooltip("Panel principal que contiene los botones Play, Levels, Info, Credits y Quit")]
    public GameObject mainMenuPanel;

    [Tooltip("Panel de selección de niveles que aparece al pulsar 'Niveles'")]
    public GameObject levelSelectPanel;

    [Tooltip("Panel con información del juego, historia o instrucciones")]
    public GameObject infoPanel;

    [Tooltip("Panel de créditos del equipo de desarrollo")]
    public GameObject creditsPanel;

    // ===================================================================
    // BOTONES DEL MENÚ PRINCIPAL
    // ===================================================================
    /// <summary>
    /// Inicia el juego cargando directamente el primer nivel.
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene("NIVEL 1");
    }

    /// <summary>
    /// Cierra la aplicación. En el Editor de Unity solo muestra un mensaje en consola.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego... (solo visible en el Editor)");
    }

    // ===================================================================
    // NAVEGACIÓN: SELECCIÓN DE NIVELES
    // ===================================================================
    /// <summary>
    /// Muestra el panel de selección de niveles y oculta el menú principal.
    /// </summary>
    public void OpenLevelSelect()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    /// <summary>
    /// Regresa al menú principal desde el panel de selección de niveles.
    /// </summary>
    public void CloseLevelSelect()
    {
        levelSelectPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // ===================================================================
    // NAVEGACIÓN: PANEL DE INFORMACIÓN / HISTORIA
    // ===================================================================
    /// <summary>
    /// Abre el panel de información o lore del juego.
    /// </summary>
    public void OpenInfoPanel()
    {
        mainMenuPanel.SetActive(false);
        infoPanel.SetActive(true);
    }

    /// <summary>
    /// Cierra el panel de información y vuelve al menú principal.
    /// </summary>
    public void CloseInfoPanel()
    {
        infoPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // ===================================================================
    // NAVEGACIÓN: PANEL DE CRÉDITOS
    // ===================================================================
    /// <summary>
    /// Muestra el panel de créditos del equipo.
    /// </summary>
    public void OpenCreditsPanel()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    /// <summary>
    /// Cierra los créditos y regresa al menú principal.
    /// </summary>
    public void CloseCreditsPanel()
    {
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // ===================================================================
    // CARGA DE NIVELES DESDE EL SELECTOR
    // ===================================================================
    /// <summary>
    /// Carga cualquier nivel por su nombre exacto. 
    /// Se usa desde los botones del panel de selección de niveles.
    /// </summary>
    /// <param name="sceneName">Nombre de la escena tal como aparece en Build Settings</param>
    public void LoadLevel(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("MainMenuUI: Intentando cargar una escena con nombre vacío.");
        }
    }
}