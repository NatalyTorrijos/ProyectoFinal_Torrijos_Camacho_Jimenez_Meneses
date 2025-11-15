using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("Placas (assign in inspector)")]
    public ColorPlate[] plates;

    [Header("Objeto final a activar (puente/portal)")]
    public GameObject finalBridge;

    private bool[] plateStates;

    private void Start()
    {
        if (plates == null || plates.Length == 0)
            Debug.LogError("PuzzleManager: no hay plates asignadas.");

        plateStates = new bool[plates != null ? plates.Length : 0];

        if (finalBridge != null)
            finalBridge.SetActive(false);
    }

    public void PlateStateChanged(ColorPlate plate, bool correct)
    {
        int idx = System.Array.IndexOf(plates, plate);
        if (idx < 0)
        {
            Debug.LogWarning("PuzzleManager: PlateStateChanged recibió una plate no registrada.");
            return;
        }

        plateStates[idx] = correct;
        Debug.Log($"PuzzleManager: plate '{plate.name}' changed -> {correct}");
        CheckPuzzleState();
    }

    private void CheckPuzzleState()
    {
        if (plateStates == null || plateStates.Length == 0) return;

        foreach (bool p in plateStates)
        {
            if (!p)
            {
                // hay una placa incompleta: asegurarse puente apagado
                if (finalBridge != null && finalBridge.activeSelf)
                    finalBridge.SetActive(false);
                return;
            }
        }

        // todas true
        Debug.Log("PuzzleManager: Todas las placas correctas -> activar finalBridge");
        if (finalBridge != null)
            finalBridge.SetActive(true);
    }
}
