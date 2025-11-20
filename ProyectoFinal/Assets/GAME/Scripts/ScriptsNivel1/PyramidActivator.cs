using UnityEngine;

/// <summary>
/// Controlador visual y sonoro de la activación de la Pirámide Principal.
/// Se encarga de:
/// • Detectar cuándo los minijuegos 1 y 2 están completados (GameProgress)
/// • Cambiar suavemente el color del material de gris (bloqueado) a dorado (desbloqueado)
/// • Activar luz, partículas y sonido al desbloquearse
/// • Mostrar mensaje de victoria al jugador
/// 
/// Se actualiza cada frame para detectar el progreso y solo se activa una vez.
/// </summary>
public class PyramidActivator : MonoBehaviour
{
    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Header("Referencias Visuales")]
    [Tooltip("Renderer del modelo de la pirámide o portal para cambiar su color")]
    public Renderer pyramidRenderer;

    [Tooltip("Luz opcional que se enciende y cambia de color al desbloquear")]
    public Light pyramidLight;

    [Header("Colores de Estado")]
    [Tooltip("Color cuando la pirámide está bloqueada")]
    public Color lockedColor = Color.gray;

    [Tooltip("Color dorado cálido cuando la pirámide se desbloquea")]
    public Color unlockedColor = new Color(1f, 0.85f, 0.3f);

    [Header("Efectos al Activarse")]
    [Tooltip("Sistema de partículas que se reproduce al desbloquear la pirámide")]
    public ParticleSystem activationEffect;

    [Tooltip("Sonido opcional que se reproduce al activar la pirámide")]
    public AudioSource activationSound;

    [Header("Transición Visual")]
    [Tooltip("Velocidad del cambio suave de color (cuanto mayor, más rápido)")]
    public float colorTransitionSpeed = 2f;

    // ===================================================================
    // ESTADO INTERNO
    // ===================================================================
    private bool isActive = false;
    private Color currentColor;

    // ===================================================================
    // INICIALIZACIÓN
    // ===================================================================
    private void Start()
    {
        currentColor = lockedColor;
        UpdateVisual(false);
    }

    // ===================================================================
    // ACTUALIZACIÓN CADA FRAME
    // ===================================================================
    private void Update()
    {
        // Si ya está activada → no hacer nada más
        if (isActive) return;

        // Detectar cuándo se cumplen los requisitos (minijuegos 1 y 2 completados)
        if (GameProgress.AreMiniGamesForPyramidDone())
        {
            ActivatePyramid();
        }

        // Transición suave del color del material
        if (pyramidRenderer != null)
        {
            pyramidRenderer.material.color = Color.Lerp(
                pyramidRenderer.material.color,
                currentColor,
                Time.deltaTime * colorTransitionSpeed
            );
        }
    }

    // ===================================================================
    // ACTIVACIÓN DE LA PIRÁMIDE
    // ===================================================================
    /// <summary>
    /// Se ejecuta cuando se cumplen los requisitos de progreso.
    /// Activa todos los efectos visuales, sonoros y de feedback.
    /// </summary>
    public void ActivatePyramid()
    {
        isActive = true;
        currentColor = unlockedColor;
        UpdateVisual(true);

        // Efectos de partículas
        if (activationEffect != null)
            activationEffect.Play();

        // Sonido de activación
        if (activationSound != null)
            activationSound.Play();

        // Mensaje al jugador
        if (UIMessageManager.Instance != null)
            UIMessageManager.Instance.ShowPriority("¡LA PIRÁMIDE HA SIDO ACTIVADA!", 4f);

        Debug.Log("Pirámide desbloqueada y activada visualmente.");
    }

    // ===================================================================
    // ACTUALIZAR EFECTOS VISUALES
    // ===================================================================
    /// <summary>
    /// Actualiza color del renderer y estado de la luz según si está desbloqueada o no.
    /// </summary>
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