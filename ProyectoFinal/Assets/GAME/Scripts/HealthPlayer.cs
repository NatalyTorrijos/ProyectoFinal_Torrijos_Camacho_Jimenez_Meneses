using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// en este escript esta todo lo que es la vida del Player, tambien del nivel2 ya que a la hora de morir, el player reinicia el nivel y activa el panel retry del nivel2.
/// </summary>

public class HealthPlayer : MonoBehaviour
{
    [Header("Salud del jugador")]
    public int maxHealth = 5;
    public int currentHealth;

    public GameObject[] hearts;        
    public GameObject panelRetry;      

    bool isDead = false;

    private Animator anim;            

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHearts();

        anim = GetComponent<Animator>();  

        if (panelRetry != null)
            panelRetry.SetActive(false);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

      //-----------------------------------------Animacion Hit (al recibir daño del enemigo)
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

    void Die() //---------------------------------------------------Animacion de morir
    {
        isDead = true;

        
        if (anim != null)
            anim.SetTrigger("Die");

        
        if (panelRetry != null)
            panelRetry.SetActive(true);

        
        StartCoroutine(RestartLevel());
    }

    System.Collections.IEnumerator RestartLevel() //----------------------se reinicia el nivel al morir
    {
        yield return new WaitForSeconds(2f);

        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
