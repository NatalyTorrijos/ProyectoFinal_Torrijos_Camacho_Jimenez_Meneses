using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RotateInteract : MonoBehaviour
{
    [Tooltip("Referencia a la pieza rotatoria que queremos accionar")]
    public RotatingPiece piece;

    [Tooltip("Si true, comprueba que la pieza esté asignada automáticamente buscando en el mismo GameObject o hijos")]
    public bool autoFindPiece = true;

    private void Awake()
    {
        if (piece == null && autoFindPiece)
        {
            piece = GetComponentInParent<RotatingPiece>();
            if (piece == null)
                piece = GetComponentInChildren<RotatingPiece>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (piece == null) return;
        if (piece.IsCorrect()) return; // si ya está correcta, no interactuar

        // Detectar tecla E (este método funciona con Input clásico)
        if (Input.GetKeyDown(KeyCode.E))
        {
            piece.RotatePiece(); // Llamada a método público -> no más CS0122
        }
    }

    // opcional: mostrar ayuda al entrar en el trigger
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (piece == null) return;
        if (piece.IsCorrect()) return;

        UIMessageManager.Instance.ShowMessage("Presiona E para girar");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        UIMessageManager.Instance.ClearMessage();
    }
}
