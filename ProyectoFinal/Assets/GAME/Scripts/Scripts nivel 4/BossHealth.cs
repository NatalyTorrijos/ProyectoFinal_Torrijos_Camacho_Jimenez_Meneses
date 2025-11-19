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
        bossControllerScript = GetComponent<MonoBehaviour>();

        currentHealth = maxHealth;

        uiGroup.alpha = 0f;
        uiGroup.gameObject.SetActive(false);
        bossNameText.gameObject.SetActive(false);

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

    void ShowUI()
    {
        if (uiVisible) return;

        uiVisible = true;
        uiGroup.gameObject.SetActive(true);
        StartFadeIn();

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

    void HandleNameTimer()
    {
        if (nameTimer <= 0) return;

        nameTimer -= Time.deltaTime;

        if (nameTimer <= 0)
            bossNameText.gameObject.SetActive(false);
    }

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
    }

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
}
