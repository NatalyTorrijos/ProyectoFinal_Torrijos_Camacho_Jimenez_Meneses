using UnityEngine;

public class PillarInteract : MonoBehaviour
{
    [Header("Interacción")]
    public float interactDistance = 2f;
    public KeyCode interactKey = KeyCode.E;

    [Header("Rotación")]
    public float rotationAmount = 90f;
    public float correctAngle = 0f;
    public float angleTolerance = 3f;

    [Header("Colores")]
    public Renderer pillarRenderer;
    public Color baseColor = Color.white;
    public Color pulseColor = new Color(1f, 0.9f, 0.5f);
    public Color correctColor = new Color(0.4f, 1f, 0.4f);
    public float pulseSpeed = 3f;
    public float pulseIntensity = 0.7f;

    private Transform player;
    private bool isCorrect = false;
    private bool playerInRange = false;

    private OrderedPuzzleManager puzzleManager;
    private int pillarIndex;

    private void Start()
    {
        var pgo = GameObject.FindGameObjectWithTag("Player");
        if (pgo != null) player = pgo.transform;

        if (pillarRenderer == null)
            pillarRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        // Si YA es correcto, no rota ni parpadea
        if (isCorrect)
        {
            pillarRenderer.material.color = correctColor;
            return;
        }

        // Parpadeo
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1) * 0.5f;
        pillarRenderer.material.color = Color.Lerp(baseColor, pulseColor, t * pulseIntensity);

        // Distancia
        if (player != null)
        {
            float d = Vector3.Distance(player.position, transform.position);
            playerInRange = d <= interactDistance;
        }

        if (playerInRange)
            UIMessageManager.Instance.ShowHint("Presiona E para girar");

        // Interacción
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            RotatePillar();                 // ← Siempre rota
            puzzleManager.TryActivatePillar(pillarIndex);  // ← Luego se evalúa el orden y el ángulo
        }
    }

    // -------------------------
    //     ROTACIÓN REAL
    // -------------------------
    private void RotatePillar()
    {
        if (isCorrect) return;
        transform.Rotate(Vector3.up * rotationAmount);
    }

    // -------------------------
    //   VALIDAR ÁNGULO CORRECTO
    // -------------------------
    public bool CheckAngleCorrect()
    {
        float y = transform.eulerAngles.y % 360f;
        float target = correctAngle % 360f;

        float diff = Mathf.DeltaAngle(y, target);
        return Mathf.Abs(diff) <= angleTolerance;
    }

    public void SetPuzzleManager(OrderedPuzzleManager manager, int index)
    {
        puzzleManager = manager;
        pillarIndex = index;
    }

    // Marcar como correcto
    public void MarkCorrect()
    {
        isCorrect = true;
        pillarRenderer.material.color = correctColor;

        UIMessageManager.Instance.ShowPriority("CORRECTO!", 2f);
    }
}
