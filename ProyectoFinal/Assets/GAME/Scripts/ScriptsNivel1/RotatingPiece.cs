using UnityEngine;

/// <summary>
/// Pieza individual del puzle de rotación (minijuego 1).
/// Permite rotar una pieza en un eje configurable (X/Y/Z o personalizado) con la tecla E.
/// Incluye:
/// • Feedback visual (pulso mientras no está correcto, verde al completarse)
/// • Detección de proximidad y hint de interacción
/// • Validación precisa del ángulo objetivo con tolerancia
/// • Comunicación automática con RotationPuzzleManager al resolverse
/// • Soporte para rotación múltiple mediante RotateSteps()
/// </summary>
public class RotatingPiece : MonoBehaviour
{
    // ===================================================================
    // CONFIGURACIÓN DE ROTACIÓN
    // ===================================================================
    public enum RotationAxis { X, Y, Z, Custom }

    [Header("Configuración de Rotación")]
    [Tooltip("Eje sobre el que rota la pieza")]
    public RotationAxis rotationAxis = RotationAxis.Y;

    [Tooltip("Eje personalizado (solo si se selecciona Custom)")]
    public Vector3 customAxis = Vector3.up;

    [Tooltip("Grados que rota cada vez que se pulsa E")]
    public float rotationAmount = 90f;

    [Tooltip("Ángulo objetivo considerado como correcto (en grados)")]
    public float correctAngle = 0f;

    [Tooltip("Margen de error permitido para considerar la pieza como correcta")]
    public float angleTolerance = 3f;

    // ===================================================================
    // INTERACCIÓN CON EL JUGADOR
    // ===================================================================
    [Header("Interacción")]
    [Tooltip("Distancia máxima desde la que se puede interactuar")]
    public float interactDistance = 2f;

    [Tooltip("Tecla para rotar la pieza")]
    public KeyCode interactKey = KeyCode.E;

    [Tooltip("Mostrar mensaje en pantalla cuando el jugador está cerca")]
    public bool showUIHint = true;

    // ===================================================================
    // FEEDBACK VISUAL
    // ===================================================================
    [Header("Colores y Efectos")]
    public Renderer pieceRenderer;
    public Color baseColor = Color.white;
    public Color pulseColor = new Color(1f, 0.9f, 0.5f);     // Amarillo cálido
    public Color completedColor = new Color(0.4f, 1f, 0.4f); // Verde brillante
    public float pulseSpeed = 3f;
    public float pulseIntensity = 0.6f;

    // ===================================================================
    // GESTOR DEL PUZLE
    // ===================================================================
    [Header("Puzzle")]
    [Tooltip("Referencia al RotationPuzzleManager que controla este puzle")]
    public RotationPuzzleManager puzzleManager;

    // ===================================================================
    // VARIABLES PRIVADAS
    // ===================================================================
    private Transform player;
    private bool isCorrect = false;
    private bool playerInRange = false;

    // ===================================================================
    // INICIALIZACIÓN
    // ===================================================================
    private void Start()
    {
        // Buscar jugador
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
            player = playerGO.transform;

        // Auto-asignar manager si no está configurado
        if (puzzleManager == null)
            puzzleManager = FindObjectOfType<RotationPuzzleManager>();

        // Auto-asignar renderer si falta
        if (pieceRenderer == null)
            pieceRenderer = GetComponent<Renderer>();

        // Color inicial
        if (pieceRenderer != null)
            pieceRenderer.material.color = baseColor;
    }

    // ===================================================================
    // ACTUALIZACIÓN CADA FRAME
    // ===================================================================
    private void Update()
    {
        // Si ya está correcto → color fijo y salir
        if (isCorrect)
        {
            if (pieceRenderer != null)
                pieceRenderer.material.color = completedColor;
            return;
        }

        // Efecto de pulso mientras no está resuelta
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
        if (pieceRenderer != null)
            pieceRenderer.material.color = Color.Lerp(baseColor, pulseColor, t * pulseIntensity);

        // Detección de proximidad
        if (player != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            playerInRange = dist <= interactDistance;
        }

        // Mostrar hint si está cerca
        if (playerInRange && showUIHint)
            UIMessageManager.Instance.ShowMessage("Presiona E para girar");

        // Interacción
        if (playerInRange && Input.GetKeyDown(interactKey))
            RotatePiece();
    }

    // ===================================================================
    // OBTENER EJE DE ROTACIÓN
    // ===================================================================
    private Vector3 GetRotationVector()
    {
        return rotationAxis switch
        {
            RotationAxis.X => Vector3.right,
            RotationAxis.Y => Vector3.up,
            RotationAxis.Z => Vector3.forward,
            RotationAxis.Custom => customAxis.normalized,
            _ => Vector3.up
        };
    }

    // ===================================================================
    // ROTAR LA PIEZA (INTERACCIÓN PRINCIPAL)
    // ===================================================================
    /// <summary>
    /// Rota la pieza y comprueba si alcanzó la orientación correcta.
    /// Si es correcta, la marca permanentemente y notifica al manager.
    /// </summary>
    public void RotatePiece()
    {
        if (isCorrect) return;

        Vector3 axis = GetRotationVector();
        transform.Rotate(axis * rotationAmount);

        if (CheckCorrect(axis))
        {
            isCorrect = true;
            UIMessageManager.Instance.ShowMessage("CORRECTO!");
            if (pieceRenderer != null)
                pieceRenderer.material.color = completedColor;
            puzzleManager?.CheckPuzzleState();
        }
    }

    // ===================================================================
    // VALIDACIÓN DE ORIENTACIÓN
    // ===================================================================
    private bool CheckCorrect(Vector3 axis)
    {
        float currentAngle = rotationAxis switch
        {
            RotationAxis.X => transform.eulerAngles.x,
            RotationAxis.Y => transform.eulerAngles.y,
            RotationAxis.Z => transform.eulerAngles.z,
            _ => Vector3.Dot(transform.eulerAngles, axis.normalized)
        };

        float diff = Mathf.DeltaAngle(currentAngle % 360f, correctAngle % 360f);
        return Mathf.Abs(diff) <= angleTolerance;
    }

    // ===================================================================
    // ESTADO PÚBLICO
    // ===================================================================
    public bool IsCorrect() => isCorrect;

    // ===================================================================
    // ROTACIÓN MÚLTIPLE (USADO POR EL MANAGER EN CASO DE REINICIO)
    // ===================================================================
    /// <summary>
    /// Permite rotar la pieza varios pasos a la vez (usado por el manager).
    /// </summary>
    public void RotateSteps(int steps)
    {
        if (isCorrect || steps == 0) return;

        Vector3 axis = GetRotationVector();
        transform.Rotate(axis * rotationAmount * steps);

        if (CheckCorrect(axis))
        {
            isCorrect = true;
            UIMessageManager.Instance.ShowMessage("CORRECTO!");
            if (pieceRenderer != null)
                pieceRenderer.material.color = completedColor;
            puzzleManager?.CheckPuzzleState();
        }
    }
}