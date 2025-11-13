using UnityEngine;
using TMPro;
using System.Collections;

public class UIMessageManager : MonoBehaviour
{
    public static UIMessageManager Instance;

    [Header("Referencia al texto de mensaje")]
    public TextMeshProUGUI messageText;

    [Header("Duración del mensaje (segundos)")]
    public float messageDuration = 3f;

    [Header("Velocidad del fade")]
    public float fadeSpeed = 2f;

    private Coroutine currentRoutine;

    private void Awake()
    {
        // Singleton simple
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Asegurar que el texto esté vacío y transparente al inicio
        if (messageText != null)
        {
            messageText.text = "";
            messageText.alpha = 0;
        }
    }

    public void ShowMessage(string msg)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowMessageRoutine(msg));
    }

    private IEnumerator ShowMessageRoutine(string msg)
    {
        if (messageText == null) yield break;

        messageText.text = msg;

        // Fade IN
        while (messageText.alpha < 1)
        {
            messageText.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // Mantener visible
        yield return new WaitForSeconds(messageDuration);

        // Fade OUT
        while (messageText.alpha > 0)
        {
            messageText.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        messageText.text = "";
    }
}
