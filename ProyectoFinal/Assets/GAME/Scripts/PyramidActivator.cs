using UnityEngine;

public class PyramidActivator : MonoBehaviour
{
    [Header("Referencia al material o luz")]
    public Renderer pyramidRenderer;        // arrastra aquí el MeshRenderer del teleporter
    public Light pyramidLight;              // opcional: una luz para resplandor
    public Color lockedColor = Color.gray;
    public Color unlockedColor = Color.yellow;

    [Header("Partículas al activarse")]
    public ParticleSystem activationEffect;

    private bool isActive = false;

    private void Start()
    {
        // Inicialmente, la pirámide está bloqueada visualmente
        UpdateVisual(false);
    }

    private void Update()
    {
        // Si ya está activa, no hacer nada
        if (isActive) return;

        // Revisar si los dos minijuegos están completos
        if (GameProgress.AreAllMiniGamesDone())
        {
            ActivatePyramid();
        }
    }

    public void ActivatePyramid()
    {
        isActive = true;

        // Cambiar el color o material
        UpdateVisual(true);

        // Efecto visual opcional
        if (activationEffect != null)
            activationEffect.Play();

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
        }
    }
}
