using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Componente utilitario para la carga de escenas desde botones de UI o llamadas externas.
/// Se utiliza principalmente en menús (Main Menu, Pause, Game Over, etc.) para cambiar de escena 
/// mediante el nombre de la escena, evitando referencias directas y facilitando la navegación.
/// </summary>
public class LoaderScene : MonoBehaviour
{
    // ===================================================================
    // INICIALIZACIÓN (no requiere lógica específica al inicio)
    // ===================================================================
    void Start()
    {
        // Este método se deja vacío intencionadamente.
        // El objeto puede estar presente en cualquier escena sin necesidad de configuración inicial.
    }

    // ===================================================================
    // ACTUALIZACIÓN FRAME A FRAME (no usado)
    // ===================================================================
    void Update()
    {
        // No se requiere lógica por frame. El componente solo expone un método público.
    }

    // ===================================================================
    // MÉTODO PÚBLICO PARA CARGAR ESCENAS
    // ===================================================================
    /// <summary>
    /// Carga una escena por su nombre exacto tal como aparece en Build Settings.
    /// Este método está pensado para ser llamado desde botones de UI (OnClick) o desde otros scripts.
    /// </summary>
    /// <param name="nameScene">Nombre exacto de la escena a cargar (ej: "Nivel1", "MainMenu", "Credits")</param>
    public void LoaderScenes(string nameScene)
    {
        // Verificación básica de seguridad (opcional pero recomendada)
        if (string.IsNullOrEmpty(nameScene))
        {
            Debug.LogError("LoaderScene: Intentando cargar una escena con nombre vacío o nulo.");
            return;
        }

        // Carga la escena en modo Single (reemplaza la escena actual)
        SceneManager.LoadScene(nameScene);

        Debug.Log($"Escena cargada correctamente: {nameScene}");
    }
}