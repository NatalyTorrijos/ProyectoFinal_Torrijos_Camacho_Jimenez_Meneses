using UnityEngine;

public class MemoryPuzzleManager : MonoBehaviour
{
    [Header("Secuencia correcta")]
    public int[] correctOrder;  // Ej: {2, 5, 1, 3}

    [Header("Objetos que se activan al completar")]
    public GameObject finalPortal;
    public GameObject finalPlatform;

    private int currentProgress = 0;

    private void Start()
    {
        if (finalPortal != null) finalPortal.SetActive(false);
        if (finalPlatform != null) finalPlatform.SetActive(false);
    }

    public void CheckTile(int id, MemoryTile tile)
    {
        // PISÓ CORRECTO
        if (id == correctOrder[currentProgress])
        {
            tile.FlashCorrect();
            currentProgress++;

            if (currentProgress >= correctOrder.Length)
                PuzzleCompleted();
        }
        else
        {
            tile.FlashWrong();
            ResetPuzzle();
        }
    }

    private void ResetPuzzle()
    {
        currentProgress = 0;
        UIMessageManager.Instance.ShowPriority("SECUENCIA INCORRECTA,INTENTA OTRO ORDEN!", 2f);
    }

    private void PuzzleCompleted()
    {
        UIMessageManager.Instance.ShowPriority("!!MINIJUEGO COMPLETADO!!!", 2f);

        if (finalPortal != null) finalPortal.SetActive(true);
        if (finalPlatform != null) finalPlatform.SetActive(true);
    }
}
