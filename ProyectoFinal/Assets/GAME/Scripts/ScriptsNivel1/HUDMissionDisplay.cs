using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDMissionDisplay : MonoBehaviour
{
    [Header("Textos del HUD")]
    public TextMeshProUGUI miniGame1Text;
    public TextMeshProUGUI miniGame2Text;
    public TextMeshProUGUI pyramidText;
    public TextMeshProUGUI miniGame3Text;

    [Header("Colores")]
    public Color defaultColor = Color.white;
    public Color completedColor = new Color(0.4f, 1f, 0.4f); // verde suave
    public Color lockedColor = new Color(1f, 0.3f, 0.3f);    // rojo suave

    [Header("Barra de progreso")]
    public Slider progressBar;

    private void Update()
    {
        UpdateHUD();
    }

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
        // PIRÁMIDE
        // ================================
        bool pyramidUnlocked = m1 && m2;
        pyramidText.text = pyramidUnlocked ? "Pirámide " : "Pirámide ";
        pyramidText.color = pyramidUnlocked ? completedColor : lockedColor;

        // ================================
        // MINIJUEGO 3 (solo aparece cuando pirámide está activa)
        // ================================
        bool m3 = GameProgress.miniGame3Completed;

        miniGame3Text.text = "Minijuego 3";
        miniGame3Text.color = m3 ? completedColor : defaultColor;

        // ================================
        // PROGRESO
        // ================================
        int count = 0;
        if (m1) count++;
        if (m2) count++;
        if (m3) count++;

        progressBar.value = count / 3f;
    }
}
