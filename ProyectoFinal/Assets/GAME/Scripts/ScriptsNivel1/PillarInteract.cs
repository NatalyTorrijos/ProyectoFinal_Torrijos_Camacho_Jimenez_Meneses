using UnityEngine;

/// <summary>
/// Componente para cada pilar del puzle de orden y orientación.
/// Permite al jugador rotar el pilar con la tecla E cuando está cerca.
/// Incluye:
/// • Feedback visual de proximidad (hint)
/// • Efecto de pulso mientras no está correcto
/// • Validación precisa de ángulo con tolerancia
/// • Comunicación con OrderedPuzzleManager para evaluar orden y estado
/// • Color verde permanente cuando el pilar está en posición correcta
/// </summary>
public class PillarInteract : MonoBehaviour
{
    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Header("Interacción")]
    [Tooltip("Distancia máxima desde la que el jugador puede interactuar con el pilar")]
    public float interactDistance = 2f;
    [Tooltip("Tecla que debe pulsar el jugador para rotar el pilar")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Rotación")]
    [Tooltip("Grados que rota el pilar cada vez que se pulsa la tecla (normalmente 90)")]
    public float rotationAmount = 90f;
    [Tooltip("Ángulo Y objetivo para considerar el pilar como correcto")]
    public float correctAngle = 0f;
    [Tooltip("Margen de error permitido en grados para considerar el ángulo válido")]
    public float angleTolerance = 3f;

    [Header("Feedback Visual")]
    [Tooltip("Renderer del pilar para cambiar colores dinámicamente")]
    public Renderer pillarRenderer;
    public Color baseColor = Color.white;
    public Color pulseColor = new Color(1f, 0.9f, 0.5f);   // Amarillo suave
    public Color correctColor = new Color(0.4f, 1f, 0.4f); // Verde brillante
    public float pulseSpeed = 3f;
    public float pulseIntensity = 0.7f;

    // ===================================================================
    // REFERENCIAS Y ESTADO
    // ===================================================================
    private Transform player;
    private bool isCorrect = false;
    private bool playerInRange = false;
    private OrderedPuzzleManager puzzleManager;
    private int pillarIndex;

    // ===================================================================
    // INICIALIZACIÓN
    // ===================================================================
    private void Start()
    {
        // Buscar jugador por tag
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
            player = playerGO.transform;

        // Auto-asignar renderer si no está configurado
        if (pillarRenderer == null)
            pillarRenderer = GetComponent<Renderer>();
    }

    // ===================================================================
    // ACTUALIZACIÓN CADA FRAME
    // ===================================================================
    private void Update()
    {
        // Si ya está correcto: color fijo verde y salir
        if (isCorrect)
        {
            pillarRenderer.material.color = correctColor;
            return;
        }

        // Efecto de pulso mientras no está correcto
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
        pillarRenderer.material.color = Color.Lerp(baseColor, pulseColor, t * pulseIntensity);

        // Detección de proximidad
        if (player != null)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            playerInRange = distance <= interactDistance;
        }

        // Mostrar hint solo cuando está cerca
        if (playerInRange)
            UIMessageManager.Instance.ShowHint("Presiona E para girar");

        // Interacción con tecla
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            RotatePillar();                                      // Rotar físicamente
            puzzleManager.TryActivatePillar(pillarIndex);        // Notificar al manager
        }
    }

    // ===================================================================
    // ROTACIÓN DEL PILAR
    // ===================================================================
    /// <summary>
    /// Rota el pilar la cantidad configurada en el eje Y.
    /// Solo se permite si aún no está marcado como correcto.
    /// </summary>
    private void RotatePillar()
    {
        if (isCorrect) return;
        transform.Rotate(Vector3.up * rotationAmount);
    }

    // ===================================================================
    // VALIDACIÓN DE ÁNGULO CORRECTO
    // ===================================================================
    /// <summary>
    /// Comprueba si el ángulo actual del pilar está dentro del rango permitido.
    /// Usa Mathf.DeltaAngle para manejar correctamente el wrap de 360°.
    /// </summary>
    public bool CheckAngleCorrect()
    {
        float currentY = transform.eulerAngles.y % 360f;
        float targetY = correctAngle % 360f;
        float difference = Mathf.Abs(Mathf.DeltaAngle(currentY, targetY));
        return difference <= angleTolerance;
    }

    // ===================================================================
    // CONFIGURACIÓN DESDE EL MANAGER
    // ===================================================================
    /// <summary>
    /// Asigna la referencia al OrderedPuzzleManager y el índice de este pilar.
    /// Llamado automáticamente por el manager al iniciar.
    /// </summary>
    public void SetPuzzleManager(OrderedPuzzleManager manager, int index)
    {
        puzzleManager = manager;
        pillarIndex = index;
    }

    // ===================================================================
    // MARCAR COMO CORRECTO
    // ===================================================================
    /// <summary>
    /// Llamado por OrderedPuzzleManager cuando este pilar está bien orientado y en orden.
    /// Fija color verde permanente y muestra feedback.
    /// </summary>
    public void MarkCorrect()
    {
        isCorrect = true;
        pillarRenderer.material.color = correctColor;
        UIMessageManager.Instance.ShowPriority("CORRECTO!", 2f);
    }
}