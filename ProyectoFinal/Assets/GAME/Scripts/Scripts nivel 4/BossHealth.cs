using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 200f;
    public float currentHealth;

    [Header("UI")]
    public Image healthFill;
    public CanvasGroup uiGroup;
    public TextMeshProUGUI bossNameText;

    [Header("Detection")]
    public Transform player;
    public float detectDistance = 15f;

    [Header("Final Screen")]
    public CanvasGroup finalPanel;   // 🔥 Panel final que aparecerá al morir el boss

    float nameTimer = 0f;
    bool uiVisible = false;
    bool nameShownOnce = false;

    Animator anim;
    bool isDead = false;

    Collider bossCollider;
    MonoBehaviour bossControllerScript;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        bossCollider = GetComponent<Collider>();

        // Busca automáticamente el BossController (tu AI)
        bossControllerScript = GetComponent<BossController>();

        currentHealth = maxHealth;

        // UI inicial
        uiGroup.alpha = 0f;
        uiGroup.gameObject.SetActive(false);
        bossNameText.gameObject.SetActive(false);

        if (finalPanel != null)
        {
            finalPanel.alpha = 0f;
            finalPanel.gameObject.SetActive(false);
        }

        UpdateBar();
    }

    void Update()
    {
        if (player == null || isDead) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= detectDistance)
            ShowUI();
        else
            HideUI();

        HandleNameTimer();
    }

    // ============================================================
    // UI SHOW / HIDE
    // ============================================================

    void ShowUI()
    {
        if (uiVisible) return;

        uiVisible = true;
        uiGroup.gameObject.SetActive(true);
        StartFadeIn();

        // Mostrar nombre del boss una sola vez cuando empieza la pelea
        if (!nameShownOnce)
        {
            nameShownOnce = true;
            nameTimer = 5f;
            bossNameText.gameObject.SetActive(true);
        }
    }

    void HideUI()
    {
        if (!uiVisible) return;

        uiVisible = false;
        StartFadeOut();
    }

    // Solo contar el tiempo mientras la UI está activa
    void HandleNameTimer()
    {
        if (!uiVisible) return;    // 🔥 FIX: evita desaparecerlo antes de tiempo
        if (nameTimer <= 0) return;

        nameTimer -= Time.deltaTime;

        if (nameTimer <= 0)
            bossNameText.gameObject.SetActive(false);
    }

    // ============================================================
    // DAMAGE
    // ============================================================

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        UpdateBar();
        if (anim != null)
            anim.SetTrigger("Hit");

        if (currentHealth <= 0)
            Die();
    }

    void UpdateBar()
    {
        if (healthFill != null)
            healthFill.fillAmount = currentHealth / maxHealth;
    }

    // ============================================================
    // DEATH
    // ============================================================

    void Die()
    {
        if (isDead) return;

        isDead = true;

        if (anim != null)
            anim.SetTrigger("Die");

        if (bossControllerScript != null)
            bossControllerScript.enabled = false;

        if (bossCollider != null)
            bossCollider.enabled = true;

        // Apagar nombre definitivamente
        bossNameText.gameObject.SetActive(false);
        nameTimer = 0f;

        // 🔥 Mostrar pantalla final
        if (finalPanel != null)
            StartCoroutine(FadeFinalPanel());
    }


    // ============================================================
    // UI COROUTINES
    // ============================================================

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

    // ============================================================
    // FINAL SCREEN FADE
    // ============================================================

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
