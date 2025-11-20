using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 200f;
    public float currentHealth;

    [Header("UI")]
    public Image healthFill;           // imagen que representa la barra de vida
    public CanvasGroup uiGroup;        // grupo para hacer fade de la ui
    public TextMeshProUGUI bossNameText;

    [Header("Detection")]
    public Transform player;           // referencia al jugador para mostrar ui
    public float detectDistance = 15f; // distancia para mostrar la ui

    [Header("Final Screen")]
    public CanvasGroup finalPanel;     // panel final que aparece al morir el boss

    float nameTimer = 0f;             // tiempo restante para mostrar el nombre
    bool uiVisible = false;           // si la ui esta visible
    bool nameShownOnce = false;       // para mostrar el nombre solo la primera vez

    Animator anim;
    bool isDead = false;

    Collider bossCollider;
    MonoBehaviour bossControllerScript; // referencia al script de control del boss

    void Start()
    {
        // obtener componentes necesarios
        anim = GetComponentInChildren<Animator>();
        bossCollider = GetComponent<Collider>();

        // buscar automaticamente el script de la ia del boss
        bossControllerScript = GetComponent<BossController>();

        currentHealth = maxHealth;

        // iniciar ui oculta
        uiGroup.alpha = 0f;
        uiGroup.gameObject.SetActive(false);
        bossNameText.gameObject.SetActive(false);

        if (finalPanel != null)
        {
            finalPanel.alpha = 0f;
            finalPanel.gameObject.SetActive(false);
        }

        UpdateBar(); // actualizar barra inicial
    }

    void Update()
    {
        // si no hay jugador o ya murio, salir
        if (player == null || isDead) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // mostrar u ocultar ui segun distancia
        if (dist <= detectDistance)
            ShowUI();
        else
            HideUI();

        HandleNameTimer(); // gestionar el temporizador del nombre
    }

    // mostrar la ui y empezar fade in
    void ShowUI()
    {
        if (uiVisible) return;

        uiVisible = true;
        uiGroup.gameObject.SetActive(true);
        StartFadeIn();

        // mostrar nombre una sola vez al iniciar la pelea
        if (!nameShownOnce)
        {
            nameShownOnce = true;
            nameTimer = 5f;
            bossNameText.gameObject.SetActive(true);
        }
    }

    // iniciar fade out de la ui
    void HideUI()
    {
        if (!uiVisible) return;

        uiVisible = false;
        StartFadeOut();
    }

    // reducir el temporizador del nombre mientras la ui esta activa
    void HandleNameTimer()
    {
        if (!uiVisible) return;
        if (nameTimer <= 0) return;

        nameTimer -= Time.deltaTime;

        if (nameTimer <= 0)
            bossNameText.gameObject.SetActive(false);
    }

    // aplicar daño al boss
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        UpdateBar(); // actualizar barra

        if (anim != null)
            anim.SetTrigger("Hit"); // reproducir anim hit

        if (currentHealth <= 0)
            Die();
    }

    // actualizar la imagen de la barra de vida
    void UpdateBar()
    {
        if (healthFill != null)
            healthFill.fillAmount = currentHealth / maxHealth;
    }

    // manejo de la muerte del boss
    void Die()
    {
        if (isDead) return;

        isDead = true;

        if (anim != null)
            anim.SetTrigger("Die");

        // desactivar el script de IA para que deje de moverse
        if (bossControllerScript != null)
            bossControllerScript.enabled = false;

        // asegurar collider activo/desactivado segun lo que se espere
        if (bossCollider != null)
            bossCollider.enabled = true;

        // ocultar nombre y detener timer
        bossNameText.gameObject.SetActive(false);
        nameTimer = 0f;

        // mostrar pantalla final con fade
        if (finalPanel != null)
            StartCoroutine(FadeFinalPanel());
    }

    // fade in / fade out de la ui principal
    void StartFadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeUI(0f, 1f, 0.8f));
    }

    void StartFadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeUI(1f, 0f, 0.8f));
    }

    // coroutine que hace el fade de la ui
    System.Collections.IEnumerator FadeUI(float from, float to, float duration)
    {
        float t = 0f;
        uiGroup.alpha = from;

        while (t < duration)
        {
            t += Time.deltaTime;
            uiGroup.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }

        uiGroup.alpha = to;

        if (to == 0f)
            uiGroup.gameObject.SetActive(false);
    }

    // coroutine que muestra la pantalla final con fade cuando el boss muere
    System.Collections.IEnumerator FadeFinalPanel()
    {
        finalPanel.gameObject.SetActive(true);

        float t = 0f;
        float duration = 2f;

        while (t < duration)
        {
            t += Time.deltaTime;
            finalPanel.alpha = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }

        finalPanel.alpha = 1f;
    }
}
