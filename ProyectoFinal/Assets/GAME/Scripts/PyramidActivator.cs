using UnityEngine;

public class PyramidActivator : MonoBehaviour
{
    [Header("Referencias visuales")]
    public Renderer pyramidRenderer;       // arrastra el MeshRenderer de la pirámide o portal
    public Light pyramidLight;             // opcional: luz para el resplandor

    [Header("Colores de estado")]
    public Color lockedColor = Color.gray;
    public Color unlockedColor = new Color(1f, 0.85f, 0.3f); // dorado cálido

    [Header("Efectos al activarse")]
    public ParticleSystem activationEffect; // partículas tipo brillo o energía
    public AudioSource activationSound;     // sonido opcional al activarse

    [Header("Transición visual")]
    public float colorTransitionSpeed = 2f; // velocidad del cambio de color

    private bool isActive = false;
    private Color currentColor;

    private void Start()
    {
        // Inicialmente bloqueada
        currentColor = lockedColor;
        UpdateVisual(false);
    }

    private void Update()
    {
        if (isActive) return;

        // Si los dos minijuegos están completos, activar la pirámide
        if (GameProgress.AreAllMiniGamesDone())
        {
            ActivatePyramid();
        }

        // Suavizar color de transición visual
        if (pyramidRenderer != null)
        {
            pyramidRenderer.material.color = Color.Lerp(
                pyramidRenderer.material.color,
                currentColor,
                Time.deltaTime * colorTransitionSpeed
            );
        }
    }

    public void ActivatePyramid()
    {
        isActive = true;
        currentColor = unlockedColor;
        UpdateVisual(true);

        // 💥 Efectos visuales
        if (activationEffect != null)
            activationEffect.Play();

        // 🔊 Sonido de activación
        if (activationSound != null)
            activationSound.Play();

        // 💬 Mensaje en pantalla
        if (UIMessageManager.Instance != null)
            UIMessageManager.Instance.ShowMessage("🔔 ¡La pirámide ha sido activada!");

        Debug.Log("✨ Pirámide desbloqueada y activada visualmente.");
    }

    private void UpdateVisual(bool unlocked)
    {
        if (pyramidRenderer != null)
        {
            pyramidRenderer.material.color = unlocked ? unlockedColor : lockedColor;
        }

        if (pyramidLight != null)
        {
            pyramidLight.enabled = unlocked;
            pyramidLight.color = unlocked ? unlockedColor : lockedColor;
            pyramidLight.intensity = unlocked ? 4f : 0f;
        }
    }
}
