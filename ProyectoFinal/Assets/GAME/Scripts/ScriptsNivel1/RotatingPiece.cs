using UnityEngine;

public class RotatingPiece : MonoBehaviour
{
    public enum RotationAxis
    {
        X,
        Y,
        Z,
        Custom
    }

    [Header("Configuración de Rotación")]
    public RotationAxis rotationAxis = RotationAxis.Y;
    public Vector3 customAxis = Vector3.up; // usado solo si RotationAxis.Custom
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
        if (isCorrect)
        {
            pieceRenderer.material.color = completedColor;
            return;
        }

        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1) * 0.5f;
        pieceRenderer.material.color =
            Color.Lerp(baseColor, pulseColor, t * pulseIntensity);

        if (player != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            playerInRange = dist <= interactDistance;
        }

        if (playerInRange && showUIHint)
            UIMessageManager.Instance.ShowMessage("Presiona E para girar");

        if (playerInRange && Input.GetKeyDown(interactKey))
            RotatePiece();
    }

    // -----------------------------------------
    // ROTAR EN EL EJE SELECCIONADO
    // -----------------------------------------
    private Vector3 GetRotationVector()
    {
        switch (rotationAxis)
        {
            case RotationAxis.X: return Vector3.right;
            case RotationAxis.Y: return Vector3.up;
            case RotationAxis.Z: return Vector3.forward;
            case RotationAxis.Custom: return customAxis.normalized;
        }
        return Vector3.up;
    }

    public void RotatePiece()
    {
        if (isCorrect) return;

        Vector3 axis = GetRotationVector();
        transform.Rotate(axis * rotationAmount);

        if (CheckCorrect(axis))
        {
            isCorrect = true;
            UIMessageManager.Instance.ShowMessage("CORRECTO!");
            pieceRenderer.material.color = completedColor;
            puzzleManager?.CheckPuzzleState();
        }
    }

    // -----------------------------------------
    // VALIDAR ÁNGULO EN EL EJE CORRECTO
    // -----------------------------------------
    private bool CheckCorrect(Vector3 axis)
    {
        float current;

        if (rotationAxis == RotationAxis.X)
            current = transform.eulerAngles.x;
        else if (rotationAxis == RotationAxis.Y)
            current = transform.eulerAngles.y;
        else if (rotationAxis == RotationAxis.Z)
            current = transform.eulerAngles.z;
        else
        {
            // custom axis → usar proyección
            current = Vector3.Dot(transform.eulerAngles, axis.normalized);
        }

        float diff = Mathf.DeltaAngle(current % 360f, correctAngle % 360f);
        return Mathf.Abs(diff) <= angleTolerance;
    }

    public bool IsCorrect() => isCorrect;

    public void RotateSteps(int steps)
    {
        if (isCorrect || steps == 0) return;

        Vector3 axis = GetRotationVector();
        transform.Rotate(axis * rotationAmount * steps);

        if (CheckCorrect(axis))
        {
            isCorrect = true;
            UIMessageManager.Instance.ShowMessage("CORRECTO!");
            pieceRenderer.material.color = completedColor;
            puzzleManager?.CheckPuzzleState();
        }
    }
}
