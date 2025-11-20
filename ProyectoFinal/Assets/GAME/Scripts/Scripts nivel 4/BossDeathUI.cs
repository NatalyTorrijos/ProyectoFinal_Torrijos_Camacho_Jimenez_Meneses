using UnityEngine;

public class BossDeathUI : MonoBehaviour
{
    public GameObject panel;       // panel que se muestra cuando el boss muere
    public BossHealth bossHealth;  // referencia al script de vida del boss

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
            panel.SetActive(true);
    }
}
