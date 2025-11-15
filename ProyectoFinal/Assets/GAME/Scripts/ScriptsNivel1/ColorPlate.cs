using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColorPlate : MonoBehaviour
{
    [Header("Color requerido")]
    public BlockID.BlockColor requiredColor = BlockID.BlockColor.Red;

    private PuzzleManager manager;
    private bool isCorrect = false;

    private void Start()
    {
        // recordar: PuzzleManager debe estar en escena
        manager = FindObjectOfType<PuzzleManager>();
        if (manager == null)
            Debug.LogError($"ColorPlate ({name}): No se encontró PuzzleManager en la escena.");
    }

    private void OnTriggerEnter(Collider other)
    {
        // busca BlockID hacia arriba por si está en hijo
        BlockID block = other.GetComponentInParent<BlockID>();
        if (block == null) return;

        if (block.color == requiredColor)
        {
            if (!isCorrect)
            {
                isCorrect = true;
                Debug.Log($"ColorPlate {name}: bloque CORRECTO ({block.name}).");
                manager?.PlateStateChanged(this, true);
                // opcional: efecto visual aquí
            }
        }
        else
        {
            Debug.Log($"ColorPlate {name}: bloque incorrecto ({block.name}) color = {block.color} (se espera {requiredColor}).");
            // si un bloque incorrecto se posa, no marca correcto (si quieres bloquear mover, añade lógica)
            manager?.PlateStateChanged(this, false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BlockID block = other.GetComponentInParent<BlockID>();
        if (block == null) return;

        if (block.color == requiredColor)
        {
            if (isCorrect)
            {
                isCorrect = false;
                Debug.Log($"ColorPlate {name}: bloque CORRECTO salio ({block.name}).");
                manager?.PlateStateChanged(this, false);
            }
        }
        else
        {
            // si salio un bloque incorrecto nada que cambiar si no estaba marcado
            manager?.PlateStateChanged(this, false);
        }
    }
}
