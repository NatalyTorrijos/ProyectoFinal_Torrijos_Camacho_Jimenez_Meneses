using UnityEngine;

public class BossDeathUI : MonoBehaviour
{
    public GameObject panel;
    // panel que se muestra cuando el boss muere

    public BossHealth bossHealth;
    // referencia al sistema de vida del boss

    void Start()
    {
        if (panel != null)
            panel.SetActive(false);
        // oculta el panel al inicio :3
    }

    void Update()
    {
        if (bossHealth == null) return;

        // si la vida baja a cero se muestra el panel
        if (bossHealth.currentHealth <= 0)
            panel.SetActive(true);
    }
}
