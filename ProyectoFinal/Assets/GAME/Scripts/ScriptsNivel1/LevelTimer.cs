using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Controlador del temporizador global del nivel.
/// Implementa patrón Singleton para ser accesible desde cualquier script (FinalPortalDirect, HUD, etc.).
/// Funcionalidades:
/// • Cuenta regresiva desde el tiempo configurado
/// • Advertencia visual + mensaje cuando quedan 2 minutos
/// • Reinicio automático del nivel al agotarse el tiempo
/// • Método público GetElapsedTime() usado para guardar el tiempo del jugador
/// </summary>
public class LevelTimer : MonoBehaviour
{
    // ===================================================================
    // SINGLETON GLOBAL
    // ===================================================================
    public static LevelTimer Instance;                      // Acceso único desde cualquier script

    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Header("Configuración del Temporizador")]
    [Tooltip("Duración total del nivel en segundos (ej: 120 = 2 minutos)")]
    public float totalTime = 120f;

    [Header("Interfaz de Usuario")]
    [Tooltip("Texto TextMeshPro que muestra el tiempo restante en formato MM:SS")]
    public TextMeshProUGUI timerText;

    [Header("Efecto de Advertencia (últimos 2 minutos)")]
    [Tooltip("Color al que parpadea el texto cuando queda poco tiempo")]
    public Color warningColor = Color.red;
    [Tooltip("Velocidad del parpadeo de advertencia")]
    [Range(0.5f, 10f)] public float flashSpeed = 2f;

    // ===================================================================
    // VARIABLES INTERNAS
    // ===================================================================
    private float currentTime;          // Tiempo restante actual
    private bool finished = false;      // Indica si el tiempo ya llegó a cero
    private bool warningMode = false;   // Activa el modo de advertencia visual
    private Color originalColor;        // Color original del texto antes de la advertencia

    // ===================================================================
    // INICIALIZACIÓN
    // ===================================================================
    private void Start()
    {
        Instance = this;                                    // Establece la instancia global
        currentTime = totalTime;                            // Inicia la cuenta regresiva
        originalColor = timerText.color;                    // Guarda el color original del texto
        UpdateUI();                                         // Muestra el tiempo inicial
    }

    // ===================================================================
    // ACTUALIZACIÓN CADA FRAME
    // ===================================================================
    private void Update()
    {
        if (finished) return;                               // Si ya se acabó, no hacer nada

        currentTime -= Time.deltaTime;                      // Reducir tiempo real

        // ---------------------------------------------------------------
        // ACTIVAR ADVERTENCIA CUANDO QUEDEN 2 MINUTOS O MENOS
        // ---------------------------------------------------------------
        if (!warningMode && currentTime <= 120f)
        {
            warningMode = true;
            UIMessageManager.Instance?.ShowHint("QUEDAN 2 MINUTOS!");
        }

        // Efecto de parpadeo en modo advertencia
        if (warningMode)
        {
            float t = (Mathf.Sin(Time.time * flashSpeed) + 1f) * 0.5f;
            timerText.color = Color.Lerp(originalColor, warningColor, t);
        }

        // ---------------------------------------------------------------
        // TIEMPO AGOTADO → REINICIAR NIVEL
        // ---------------------------------------------------------------
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            finished = true;
            RestartLevel();
        }

        UpdateUI();                                         // Actualizar texto del temporizador
    }

    // ===================================================================
    // ACTUALIZAR TEXTO DEL TEMPORIZADOR (MM:SS)
    // ===================================================================
    /// <summary>
    /// Convierte el tiempo restante a formato minutos:segundos con dos dígitos.
    /// </summary>
    private void UpdateUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    // ===================================================================
    // REINICIAR EL NIVEL AL AGOTARSE EL TIEMPO
    // ===================================================================
    /// <summary>
    /// Muestra mensaje prioritario y recarga la escena actual tras 2 segundos.
    /// </summary>
    private void RestartLevel()
    {
        UIMessageManager.Instance?.ShowPriority("Tiempo agotado. Reiniciando...", 2f);
        Invoke(nameof(ReloadScene), 2f);
    }

    /// <summary>
    /// Recarga la escena actual (reinicio completo del nivel).
    /// </summary>
    private void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }

    // ===================================================================
    // MÉTODO PÚBLICO CLAVE: OBTENER TIEMPO EMPLEADO
    // ===================================================================
    /// <summary>
    /// Devuelve el tiempo real que el jugador tardó en completar el nivel.
    /// Usado por FinalPortalDirect para guardar el mejor tiempo en JSON.
    /// Fórmula: tiempo inicial - tiempo restante.
    /// </summary>
    /// <returns>Tiempo transcurrido en segundos (float)</returns>
    public float GetElapsedTime()
    {
        return totalTime - currentTime;
    }
}