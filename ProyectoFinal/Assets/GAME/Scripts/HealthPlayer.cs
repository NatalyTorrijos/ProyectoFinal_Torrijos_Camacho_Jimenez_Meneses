using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthPlayer : MonoBehaviour
{
    [Header("Player Health Settings")]
    public int maxHealth = 5;
    public int currentHealth;

    public GameObject[] hearts;        // Los 5 corazones en UI
    public GameObject panelRetry;      // Panel que aparece al morir

    bool isDead = false;

    private Animator anim;             // ← Para animaciones Hit y Die

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHearts();

        anim = GetComponent<Animator>();   // ← Busca la animación en el Player

        if (panelRetry != null)
            panelRetry.SetActive(false);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        // --- Animación HIT ---
        if (anim != null)
            anim.SetTrigger("Hit");

        if (currentHealth < 0)
            currentHealth = 0;

        UpdateHearts();

        if (currentHealth == 0)
        {
            Die();
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < currentHealth);
        }
    }

    void Die()
    {
        isDead = true;

        // --- Animación DIE ---
        if (anim != null)
            anim.SetTrigger("Die");

        // Mostrar panel Retry
        if (panelRetry != null)
            panelRetry.SetActive(true);

        // Reiniciar con delay
        StartCoroutine(RestartLevel());
    }

    System.Collections.IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(2f);

        // Reiniciar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
