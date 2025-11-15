using UnityEngine;

public class RotatingPiece : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    public float rotationAmount = 90f;
    public float correctAngle = 0f;
    public float angleTolerance = 3f;

    [Header("Interacción")]
    public float interactDistance = 2f;
    public KeyCode interactKey = KeyCode.E;
    public bool showUIHint = true;

    [Header("Colores")]
    public Renderer pieceRenderer;
    public Color baseColor = Color.white;
    public Color pulseColor = new Color(1f, 0.9f, 0.5f);
    public Color completedColor = new Color(0.4f, 1f, 0.4f);
    public float pulseSpeed = 3f;
    public float pulseIntensity = 0.6f;

    [Header("Puzzle")]
    public RotationPuzzleManager puzzleManager;

    private Transform player;
    private bool isCorrect = false;
    private bool playerInRange = false;

    private void Start()
    {
        var pgo = GameObject.FindGameObjectWithTag("Player");
        if (pgo != null) player = pgo.transform;

        if (puzzleManager == null)
            puzzleManager = FindObjectOfType<RotationPuzzleManager>();

        if (pieceRenderer == null)
            pieceRenderer = GetComponent<Renderer>();

        pieceRenderer.material.color = baseColor;
    }

    private void Update()
    {
        // --- NO INTERACTÚA SI YA ES CORRECTA ---
        if (isCorrect)
        {
            pieceRenderer.material.color = completedColor;
            return;
        }

        // --- EFECTO DE PARPADEO PERMANENTE ---
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1) * 0.5f;
        Color c = Color.Lerp(baseColor, pulseColor, t * pulseIntensity);
        pieceRenderer.material.color = c;

        // --- VERIFICAR DISTANCIA ---
        if (player != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            playerInRange = dist <= interactDistance;
        }
        else playerInRange = false;

        if (playerInRange && showUIHint)
        {
            UIMessageManager.Instance.ShowMessage("Presiona E para girar");
        }

        // --- INTERACCIÓN (MULTIPLES PRESIONES DE E SIN SALIR) ---
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            RotatePiece();
        }
    }

    // ============================================================
    //       🔄 ROTAR PIEZA Y VALIDAR SI ES CORRECTA
    // ============================================================
    public void RotatePiece()
    {
        if (isCorrect) return;

        transform.Rotate(Vector3.up * rotationAmount);

        if (CheckCorrect())
        {
            isCorrect = true;

            // Mensaje de correcto
            UIMessageManager.Instance.ShowMessage("CORRECTO!");

            // Cambiar color a verde (ya no parpadea)
            pieceRenderer.material.color = completedColor;

            // Avisar al puzzle manager
            puzzleManager?.CheckPuzzleState();
        }
    }

    // ============================================================
    //       🧠 VALIDAR ANGULO CORRECTO
    // ============================================================
    private bool CheckCorrect()
    {
        float y = transform.eulerAngles.y % 360f;
        float target = correctAngle % 360f;

        float diff = Mathf.DeltaAngle(y, target);
        return Mathf.Abs(diff) <= angleTolerance;
    }

    public bool IsCorrect()
    {
        return isCorrect;
    }

    // Para rotaciones por código desde otro script
    public void RotateSteps(int steps)
    {
        if (isCorrect || steps == 0) return;

        float amount = rotationAmount * steps;
        transform.Rotate(Vector3.up * amount);

        if (CheckCorrect())
        {
            isCorrect = true;
            UIMessageManager.Instance.ShowMessage("CORRECTO!");
            pieceRenderer.material.color = completedColor;
            puzzleManager?.CheckPuzzleState();
        }
    }
}
