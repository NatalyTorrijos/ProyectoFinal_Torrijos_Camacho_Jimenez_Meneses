using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthPlayer : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;

    public GameObject[] hearts;        // Los 5 corazones
    public GameObject panelRetry;      // Panel que aparece al morir

    bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHearts();

        if (panelRetry != null)
            panelRetry.SetActive(false);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

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

        // Mostrar panel
        if (panelRetry != null)
            panelRetry.SetActive(true);

        // Iniciar reinicio automático
        StartCoroutine(RestartLevel());
    }

    System.Collections.IEnumerator RestartLevel()
    {
        // Esperar 2 segundos
        yield return new WaitForSeconds(2f);

        // Reiniciar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
