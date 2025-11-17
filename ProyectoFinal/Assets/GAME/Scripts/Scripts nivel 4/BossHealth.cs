using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossUIController : MonoBehaviour
{
    public float maxHealth = 200f;
    public float currentHealth;

    public Image healthFill;
    public CanvasGroup uiGroup;
    public TextMeshProUGUI bossNameText;

    public Transform player;
    public float detectDistance = 15f;

    float nameTimer = 0f;
    bool uiVisible = false;
    bool nameShownOnce = false;

    void Start()
    {
        currentHealth = maxHealth;
        uiGroup.alpha = 0f;
        uiGroup.gameObject.SetActive(false);
        bossNameText.gameObject.SetActive(false);
        UpdateBar();
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= detectDistance)
        {
            if (!uiVisible)
            {
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
        }
        else
        {
            if (uiVisible)
            {
                uiVisible = false;
                StartFadeOut();
            }
        }

        if (nameTimer > 0)
        {
            nameTimer -= Time.deltaTime;
            if (nameTimer <= 0)
                bossNameText.gameObject.SetActive(false);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;
        UpdateBar();
    }

    void UpdateBar()
    {
        healthFill.fillAmount = currentHealth / maxHealth;
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
