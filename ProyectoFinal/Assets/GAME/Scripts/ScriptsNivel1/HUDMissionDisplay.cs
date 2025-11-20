using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controla el HUD de misiones y progreso visible durante el juego.
/// Muestra en tiempo real el estado de los tres minijuegos y el acceso a la pirámide,
/// actualiza colores según completado/bloqueado y llena una barra de progreso general.
/// Se actualiza cada frame para reflejar cambios inmediatos en GameProgress.
/// </summary>
public class HUDMissionDisplay : MonoBehaviour
{
    // ===================================================================
    // REFERENCIAS A TEXTOS DEL HUD (TextMeshPro)
    // ===================================================================
    [Header("Textos de Misiones")]
    [Tooltip("Texto que muestra el estado del Minijuego 1")]
    public TextMeshProUGUI miniGame1Text;

    [Tooltip("Texto que muestra el estado del Minijuego 2")]
    public TextMeshProUGUI miniGame2Text;

    [Tooltip("Texto que indica si la Pirámide está desbloqueada")]
    public TextMeshProUGUI pyramidText;

    [Tooltip("Texto que muestra el estado del Minijuego 3")]
    public TextMeshProUGUI miniGame3Text;

    // ===================================================================
    // COLORES PERSONALIZADOS
    // ===================================================================
    [Header("Colores de Estado")]
    [Tooltip("Color por defecto cuando la misión aún no está completada")]
    public Color defaultColor = Color.white;

    [Tooltip("Color verde claro cuando la misión está completada")]
    public Color completedColor = new Color(0.4f, 1f, 0.4f); // Verde suave

    [Tooltip("Color rojo claro cuando la misión está bloqueada (pirámide)")]
    public Color lockedColor = new Color(1f, 0.3f, 0.3f);    // Rojo suave

    // ===================================================================
    // BARRA DE PROGRESO GENERAL
    // ===================================================================
    [Header("Barra de Progreso")]
    [Tooltip("Slider que indica el progreso total (0 a 3 misiones completadas)")]
    public Slider progressBar;

    // ===================================================================
    // ACTUALIZACIÓN CADA FRAME
    // ===================================================================
    /// <summary>
    /// Se ejecuta cada frame. Llama a la función que actualiza todo el HUD.
    /// </summary>
    private void Update()
    {
        UpdateHUD();
    }

    // ===================================================================
    // FUNCIÓN PRINCIPAL DE ACTUALIZACIÓN DEL HUD
    // ===================================================================
    /// <summary>
    /// Actualiza textos, colores y barra de progreso según el estado actual guardado en GameProgress.
    /// Se ejecuta cada frame para reflejar cambios instantáneamente.
    /// </summary>
    private void UpdateHUD()
    {
        // ================================
        // MINIJUEGO 1
        // ================================
        bool m1 = GameProgress.miniGame1Completed;
        miniGame1Text.text = "Minijuego 1";
        miniGame1Text.color = m1 ? completedColor : defaultColor;

        // ================================
        // MINIJUEGO 2
        // ================================
        bool m2 = GameProgress.miniGame2Completed;
        miniGame2Text.text = "Minijuego 2";
        miniGame2Text.color = m2 ? completedColor : defaultColor;

        // ================================
        // ESTADO DE LA PIRÁMIDE
        // ================================
        bool pyramidUnlocked = GameProgress.AreMiniGamesForPyramidDone();
        pyramidText.text = "Pirámide";
        pyramidText.color = pyramidUnlocked ? completedColor : lockedColor;

        // ================================
        // MINIJUEGO 3
        // ================================
        bool m3 = GameProgress.miniGame3Completed;
        miniGame3Text.text = "Minijuego 3";
        miniGame3Text.color = m3 ? completedColor : defaultColor;

        // ================================
        // BARRA DE PROGRESO GENERAL (0-3 misiones)
        // ================================
        int count = 0;
        if (m1) count++;
        if (m2) count++;
        if (m3) count++;

        progressBar.value = count / 3f;  // 0f = 0%, 1f = 100%
    }
}
