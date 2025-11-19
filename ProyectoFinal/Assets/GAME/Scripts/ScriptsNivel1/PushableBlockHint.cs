using UnityEngine;

public class PushableBlockHint : MonoBehaviour
{
    [Header("Distancia para mostrar mensaje")]
    public float hintDistance = 2f;

    private Transform player;
    private BlockID blockID;
    private bool playerInRange = false;

    private void Start()
    {
        // Obtener jugador
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        // Obtener ID de color del bloque
        blockID = GetComponent<BlockID>();
        if (blockID == null)
        {
            Debug.LogWarning($"PushableBlockHint ({name}): No se encontró BlockID.");
        }
    }

    private void Update()
    {
        if (player == null || blockID == null) return;

        float dist = Vector3.Distance(player.position, transform.position);
        bool nowInRange = dist <= hintDistance;

        // Si acaba de entrar en la zona → mostrar hint
        if (nowInRange && !playerInRange)
        {
            UIMessageManager.Instance.ShowHint(GetMessageForColor(blockID.color));
        }

        playerInRange = nowInRange;
    }

    // ============================
    // 🔥 Mensajes por color
    // ============================
    private string GetMessageForColor(BlockID.BlockColor color)
    {
        switch (color)
        {
            case BlockID.BlockColor.Red:
                return "lleva el bloque hacia el portal rojo ";
            case BlockID.BlockColor.Blue:
                return "Lleva el bloque hacia el portal azul ";
            case BlockID.BlockColor.Yellow:
                return "Empuja el bloque ";
            default:
                return "Empuja este bloque";
        }
    }
}
