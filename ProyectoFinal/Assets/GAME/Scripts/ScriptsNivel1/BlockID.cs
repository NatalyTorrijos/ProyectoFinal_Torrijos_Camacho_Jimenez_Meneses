using UnityEngine;

public class BlockID : MonoBehaviour
{
    public enum BlockColor { Red, Blue, Yellow }
    public BlockColor color = BlockColor.Red;

    [Header("Apariencia (opcional)")]
    public bool applyColorAtAwake = true;
    public Color redColor = new Color(0.9f, 0.2f, 0.2f);
    public Color blueColor = new Color(0.3f, 0.6f, 1f);
    public Color yellowColor = new Color(1f, 0.9f, 0.2f);

    [Header("Pulse (titileo)")]
    public bool pulseEffect = true;
    public float pulseSpeed = 2f;
    public float pulseStrength = 0.25f;

    private Renderer rend;
    private Material mat;
    private Color baseColor;

    private void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
        if (rend == null)
        {
            Debug.LogWarning($"BlockID ({name}): no se encontró Renderer en hijos.");
            return;
        }

        // crear material local para no modificar el prefab global
        mat = new Material(rend.sharedMaterial);
        rend.material = mat;

        AssignBaseColor();
        if (applyColorAtAwake)
            mat.color = baseColor;
    }

    private void Update()
    {
        if (pulseEffect && mat != null)
        {
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
            float intensity = 1f + pulse * pulseStrength;
            mat.color = baseColor * intensity;
        }
    }

    public void AssignBaseColor()
    {
        switch (color)
        {
            case BlockColor.Red: baseColor = redColor; break;
            case BlockColor.Blue: baseColor = blueColor; break;
            case BlockColor.Yellow: baseColor = yellowColor; break;
            default: baseColor = Color.white; break;
        }
        if (mat != null) mat.color = baseColor;
    }
}
