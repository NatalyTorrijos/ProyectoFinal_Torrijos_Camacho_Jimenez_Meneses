using UnityEngine;
using TMPro;
using System.Collections;

public class UIMessageManager : MonoBehaviour
{
    public static UIMessageManager Instance;

    [Header("Referencia al texto de mensaje")]
    public TextMeshProUGUI messageText;

    [Header("Duración por defecto")]
    public float defaultDuration = 2f;

    [Header("Fade")]
    public float fadeSpeed = 2f;

    private Coroutine currentRoutine;
    private bool priorityActive = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (messageText != null)
        {
            messageText.text = "";
            messageText.alpha = 0;
        }
    }

    // ======================================================
    // 🔥 RESTAURADO → COMPATIBILIDAD CON SCRIPTS ANTIGUOS
    // ======================================================
    public void ShowMessage(string msg)
    {
        // No interrumpe mensajes prioritarios
        if (priorityActive) return;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(msg, defaultDuration));
    }

    // ======================================================
    // 🔥 Hint (presiona E) → se muestra siempre pero suave
    // ======================================================
    public void ShowHint(string msg)
    {
        if (priorityActive) return; // no interfiere con prioridad

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(msg, defaultDuration));
    }

    // ======================================================
    // 🔥 Mensajes importantes → NO se interrumpen
    // ======================================================
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

    // ======================================================
    // 🔥 Rutina compartida (fade + duración)
    // ======================================================
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

    // ======================================================
    // 🔥 Limpieza manual
    // ======================================================
    public void ClearMessage()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        messageText.text = "";
        messageText.alpha = 0;
        priorityActive = false;
    }
}
