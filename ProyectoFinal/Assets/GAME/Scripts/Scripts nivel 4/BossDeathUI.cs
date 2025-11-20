using UnityEngine;

public class BossDeathUI : MonoBehaviour
{
    public GameObject panel;
    public BossHealth bossHealth;

    void Start()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    void Update()
    {
        if (bossHealth == null) return;

        if (bossHealth.currentHealth <= 0)
            panel.SetActive(true);
    }
}
