using UnityEngine;

/// <summary>
/// Identificador de color para bloques del puzzle.
/// Se encarga de:
/// • Identificar el color específico del bloque (Rojo, Azul o Amarillo)
/// • Aplicar automáticamente el color visual al material del bloque
/// • Crear una instancia local del material para evitar modificar prefabs
/// • Aplicar un efecto de pulso/titileo opcional para mayor visibilidad
/// • Proporcionar configuración de colores personalizables en el inspector
/// 
/// Usado por ColorPlate para verificar si un bloque es el correcto para cada placa.
/// </summary>
public class BlockID : MonoBehaviour
{
    // ===================================================================
    // ENUMERACIÓN DE COLORES
    // ===================================================================
    public enum BlockColor { Red, Blue, Yellow }

    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Tooltip("Color identificador de este bloque para el sistema de puzzles")]
    public BlockColor color = BlockColor.Red;

    [Header("Apariencia (opcional)")]
    [Tooltip("Si está activo, aplica automáticamente el color al material al iniciar")]
    public bool applyColorAtAwake = true;

    [Tooltip("Tono de color rojo personalizado")]
    public Color redColor = new Color(0.9f, 0.2f, 0.2f);

    [Tooltip("Tono de color azul personalizado")]
    public Color blueColor = new Color(0.3f, 0.6f, 1f);

    [Tooltip("Tono de color amarillo personalizado")]
    public Color yellowColor = new Color(1f, 0.9f, 0.2f);

    [Header("Pulse (titileo)")]
    [Tooltip("Activa el efecto de pulso/titileo en el color del bloque")]
    public bool pulseEffect = true;

    [Tooltip("Velocidad del efecto de pulso (mayor = más rápido)")]
    public float pulseSpeed = 2f;

    [Tooltip("Intensidad del pulso (0 = sin cambio, 1 = cambio completo)")]
    public float pulseStrength = 0.25f;

    // ===================================================================
    // ESTADO INTERNO
    // ===================================================================
    private Renderer rend;
    private Material mat;
    private Color baseColor;

    // ===================================================================
    // INICIALIZACIÓN
    // ===================================================================
    private void Awake()
    {
        // Buscar el Renderer en este objeto o en sus hijos
        rend = GetComponentInChildren<Renderer>();

        if (rend == null)
        {
            Debug.LogWarning($"BlockID ({name}): no se encontró Renderer en hijos.");
            return;
        }

        // Crear material local para no modificar el prefab global
        mat = new Material(rend.sharedMaterial);
        rend.material = mat;

        // Asignar el color base según la configuración
        AssignBaseColor();

        // Aplicar el color inmediatamente si está configurado
        if (applyColorAtAwake)
            mat.color = baseColor;
    }

    // ===================================================================
    // EFECTO DE PULSO
    // ===================================================================
    /// <summary>
    /// Aplica el efecto de pulso al color del bloque cada frame si está activado.
    /// </summary>
    private void Update()
    {
        if (pulseEffect && mat != null)
        {
            // Calcular el valor del pulso (oscila entre 0 y 1)
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;

            // Aplicar intensidad del pulso al brillo del color
            float intensity = 1f + pulse * pulseStrength;
            mat.color = baseColor * intensity;
        }
    }

    // ===================================================================
    // ASIGNACIÓN DE COLOR BASE
    // ===================================================================
    /// <summary>
    /// Asigna el color base del bloque según su identificador de color configurado.
    /// Puede ser llamado externamente para actualizar el color en tiempo real.
    /// </summary>
    public void AssignBaseColor()
    {
        // Seleccionar el color según el enum
        switch (color)
        {
            case BlockColor.Red:
                baseColor = redColor;
                break;
            case BlockColor.Blue:
                baseColor = blueColor;
                break;
            case BlockColor.Yellow:
                baseColor = yellowColor;
                break;
            default:
                baseColor = Color.white;
                break;
        }

        // Aplicar el color al material si ya está inicializado
        if (mat != null)
            mat.color = baseColor;
    }
}