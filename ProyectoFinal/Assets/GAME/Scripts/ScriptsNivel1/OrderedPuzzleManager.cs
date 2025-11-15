using UnityEngine;

public class OrderedPuzzleManager : MonoBehaviour
{
    [Header("Pilares en orden correcto (1 a 4)")]
    public PillarInteract[] pillars;

    [Header("Objetos que se activan al completar")]
    public GameObject rotatingPlatform;
    public GameObject endPortal;

    private int currentIndex = 0;

    private void Start()
    {
        if (rotatingPlatform != null)
            rotatingPlatform.SetActive(false);

        if (endPortal != null)
            endPortal.SetActive(false);

        for (int i = 0; i < pillars.Length; i++)
        {
            pillars[i].SetPuzzleManager(this, i);
        }
    }

    public void TryActivatePillar(int index)
    {
        // NO ES EL QUE SIGUE
        if (index != currentIndex)
        {
            UIMessageManager.Instance.ShowPriority("ESE NO ES EL ORDEN CORRECTO", 2f);
            return;
        }

        // ES EL QUE TOCA PERO...
        // ¿ESTÁ ROTADO BIEN?
        if (!pillars[index].CheckAngleCorrect())
        {
            UIMessageManager.Instance.ShowPriority("LA ROTACIÓN NO ES LA CORRECTA", 2f);
            return;
        }

        // AHORA SÍ ES CORRECTO
        pillars[index].MarkCorrect();
        currentIndex++;

        // SI COMPLETA TODO
        if (currentIndex >= pillars.Length)
            PuzzleCompleted();
    }

    private void PuzzleCompleted()
    {
        UIMessageManager.Instance.ShowPriority("PUZZLE COMPLETADO!", 2f);

        if (rotatingPlatform != null)
            rotatingPlatform.SetActive(true);

        if (endPortal != null)
            endPortal.SetActive(true);
    }
}
