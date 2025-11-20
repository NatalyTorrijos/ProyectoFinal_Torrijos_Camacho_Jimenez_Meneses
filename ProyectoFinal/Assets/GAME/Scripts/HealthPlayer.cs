using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthPlayer : MonoBehaviour
{
    [Header("Player Health Settings")]
    public int maxHealth = 5;              // Vida máxima del jugador
    public int currentHealth;              // Vida actual del jugador
    public GameObject[] hearts;            // Objetos de corazón en el UI
    public GameObject panelRetry;          // Panel que aparece cuando muere

    bool isDead = false;                   // ¿El jugador ya murió?
    private Animator anim;                 // Referencia al Animator para animaciones Hit y Die

    void Start()
    {
        currentHealth = maxHealth;         // Iniciar vida al máximo
        UpdateHearts();                    // Actualizar UI de corazones

        anim = GetComponent<Animator>();   // Obtener Animator del jugador

        // Asegurar que el panel Retry esté oculto al iniciar
        if (panelRetry != null)
            panelRetry.SetActive(false);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;                // Evitar daño si ya está muerto

        currentHealth -= amount;           // Reducir vida

        // Activar animación de golpe (Hit)
        if (anim != null)
            anim.SetTrigger("Hit");

        // Evitar que la vida baje de 0
        if (currentHealth < 0)
            currentHealth = 0;

        UpdateHearts();                    // Actualizar corazones en pantalla

        // Si la vida llega a 0 → morir
        if (currentHealth == 0)
        {
            Die();
        }
    }

    void UpdateHearts()
    {
        // Activar/desactivar corazones según la vida actual
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < currentHealth);
        }
    }

    void Die()
    {
        isDead = true;                     // Marcar como muerto

        // Activar animación de muerte (Die)
        if (anim != null)
            anim.SetTrigger("Die");

        // Mostrar panel Retry
        if (panelRetry != null)
            panelRetry.SetActive(true);

        // Reiniciar la escena después de un delay
        StartCoroutine(RestartLevel());
    }

    System.Collections.IEnumerator RestartLevel()
    {
        // Esperar 2 segundos antes de reiniciar
        yield return new WaitForSeconds(2f);

        // Recargar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
