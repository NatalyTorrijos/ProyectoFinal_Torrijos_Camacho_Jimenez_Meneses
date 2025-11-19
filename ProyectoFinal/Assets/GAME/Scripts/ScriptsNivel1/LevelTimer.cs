using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    public static LevelTimer Instance;   // <-- Accesible desde otros scripts

    [Header("Duración del nivel (segundos)")]
    public float totalTime = 120f;

    [Header("UI")]
    public TextMeshProUGUI timerText;

    [Header("Advertencia (cuando falten 2 minutos)")]
    public Color warningColor = Color.red;
    public float flashSpeed = 2f;

    private float currentTime;
    private bool finished = false;
    private bool warningMode = false;
    private Color originalColor;

    private void Start()
    {
        Instance = this;                     // <-- Guardamos referencia global
        currentTime = totalTime;
        originalColor = timerText.color;
        UpdateUI();
    }

    private void Update()
    {
        if (finished) return;

        currentTime -= Time.deltaTime;

        // ============================
        // 🔥 MODO ADVERTENCIA (faltan 2 min)
        // ============================
        if (!warningMode && currentTime <= 120f)
        {
            warningMode = true;
            UIMessageManager.Instance?.ShowHint("QUEDAN 2 MINUTOS!");
        }

        if (warningMode)
        {
            float t = (Mathf.Sin(Time.time * flashSpeed) + 1f) * 0.5f;
            timerText.color = Color.Lerp(originalColor, warningColor, t);
        }

        // ============================
        // 🔥 TIEMPO AGOTADO
        // ============================
        if (currentTime <= 0)
        {
            currentTime = 0;
            finished = true;
            RestartLevel();
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    private void RestartLevel()
    {
        UIMessageManager.Instance?.ShowPriority("⏳ Tiempo agotado. Reiniciando...", 2f);
        Invoke(nameof(ReloadScene), 2f);
    }

    private void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }

    // ======================================================
    // 🔥 MÉTODO CLAVE → TIEMPO USADO POR EL JUGADOR
    // ======================================================
    public float GetElapsedTime()
    {
        // Tiempo completado = tiempo inicial - tiempo restante
        return totalTime - currentTime;
    }
}
