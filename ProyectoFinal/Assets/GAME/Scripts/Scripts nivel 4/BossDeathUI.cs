using UnityEngine;

public class BossDeathUI : MonoBehaviour
{
    public GameObject panel;       // panel que se muestra cuando el boss muere
    public BossHealth bossHealth;  // referencia al script de vida del boss

    [Header("otros paneles a ocultar")]
    public GameObject[] panelsToHide;
    // aqui puedes arrastrar paneles como pause, hud, timer, etc

    void Start()
    {
        // asegurarse de que el panel este oculto al inicio
        if (panel != null)
            panel.SetActive(false);
    }

    void Update()
    {
        if (bossHealth == null) return;

        // si la vida del boss es cero, mostrar el panel
        if (bossHealth.currentHealth <= 0)
        {
            panel.SetActive(true);

            // ocultar el resto de paneles cuando salga este
            foreach (GameObject p in panelsToHide)
            {
                if (p != null)
                    p.SetActive(false);
            }
        }
    }
}
