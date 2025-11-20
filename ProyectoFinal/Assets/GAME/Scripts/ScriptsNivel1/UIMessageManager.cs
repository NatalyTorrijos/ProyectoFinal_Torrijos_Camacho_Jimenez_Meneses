using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Gestor centralizado de mensajes UI en pantalla.
/// Se encarga de:
/// • Mostrar mensajes temporales al jugador con fade in/out
/// • Gestionar diferentes tipos de mensajes (normales, hints, prioritarios)
/// • Prevenir que mensajes prioritarios sean interrumpidos
/// • Controlar la duración y velocidad de transición de los mensajes
/// • Proporcionar un singleton accesible desde cualquier script
/// 
/// Los mensajes prioritarios no pueden ser interrumpidos por mensajes normales o hints.
/// </summary>
public class UIMessageManager : MonoBehaviour
{
    // ===================================================================
    // SINGLETON
    // ===================================================================
    public static UIMessageManager Instance;

    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Header("Referencia al texto de mensaje")]
    [Tooltip("Componente TextMeshProUGUI donde se mostrarán los mensajes")]
    public TextMeshProUGUI messageText;

    [Header("Duración por defecto")]
    [Tooltip("Tiempo en segundos que permanece visible un mensaje normal")]
    public float defaultDuration = 2f;

    [Header("Fade")]
    [Tooltip("Velocidad de transición del fade in/out (mayor = más rápido)")]
    public float fadeSpeed = 2f;

    // ===================================================================
    // ESTADO INTERNO
    // ===================================================================
    private Coroutine currentRoutine;
    private bool priorityActive = false;

    // ===================================================================
    // INICIALIZACIÓN DEL SINGLETON
    // ===================================================================
    private void Awake()
    {
        // Configurar singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Inicializar el texto como invisible y vacío
        if (messageText != null)
        {
            messageText.text = "";
            messageText.alpha = 0;
        }
    }

    // ===================================================================
    // MENSAJES NORMALES
    // ===================================================================
    /// <summary>
    /// Muestra un mensaje normal con la duración por defecto.
    /// No interrumpe mensajes prioritarios activos.
    /// </summary>
    /// <param name="msg">Texto del mensaje a mostrar</param>
    public void ShowMessage(string msg)
    {
        // No interrumpe mensajes prioritarios
        if (priorityActive) return;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(msg, defaultDuration));
    }

    // ===================================================================
    // MENSAJES DE AYUDA (HINTS)
    // ===================================================================
    /// <summary>
    /// Muestra un hint al jugador (por ejemplo: "Presiona E para interactuar").
    /// No interrumpe mensajes prioritarios activos.
    /// </summary>
    /// <param name="msg">Texto del hint a mostrar</param>
    public void ShowHint(string msg)
    {
        // No interfiere con mensajes prioritarios
        if (priorityActive) return;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(msg, defaultDuration));
    }

    // ===================================================================
    // MENSAJES PRIORITARIOS
    // ===================================================================
    /// <summary>
    /// Muestra un mensaje prioritario que NO puede ser interrumpido por otros mensajes.
    /// Útil para notificaciones importantes como victorias o activaciones de objetivos.
    /// </summary>
    /// <param name="msg">Texto del mensaje prioritario</param>
    /// <param name="duration">Duración específica en segundos</param>
    public void ShowPriority(string msg, float duration)
    {
        priorityActive = true;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(msg, duration, () =>
        {
            priorityActive = false;
        }));
    }

    // ===================================================================
    // RUTINA DE VISUALIZACIÓN
    // ===================================================================
    /// <summary>
    /// Corrutina compartida que maneja el fade in, duración y fade out del mensaje.
    /// </summary>
    /// <param name="msg">Texto a mostrar</param>
    /// <param name="duration">Tiempo que permanece visible</param>
    /// <param name="onFinish">Callback opcional al terminar la animación</param>
    private IEnumerator ShowRoutine(string msg, float duration, System.Action onFinish = null)
    {
        if (messageText == null) yield break;

        messageText.text = msg;

        // Fade IN
        while (messageText.alpha < 1)
        {
            messageText.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // Esperar la duración especificada
        yield return new WaitForSeconds(duration);

        // Fade OUT
        while (messageText.alpha > 0)
        {
            messageText.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        messageText.text = "";
        onFinish?.Invoke();
    }

    // ===================================================================
    // LIMPIEZA MANUAL
    // ===================================================================
    /// <summary>
    /// Limpia inmediatamente el mensaje actual y detiene cualquier animación en curso.
    /// Resetea el estado de prioridad.
    /// </summary>
    public void ClearMessage()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        messageText.text = "";
        messageText.alpha = 0;
        priorityActive = false;
    }
}