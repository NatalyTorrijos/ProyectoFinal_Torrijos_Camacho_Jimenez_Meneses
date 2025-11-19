using UnityEngine;
using System.Collections;

public class SequenceCube : MonoBehaviour
{
    public int cubeID;
    public SequencePuzzle puzzle;

    [Header("Arrastra aquí el Mesh Renderer del cubo")]
    public Renderer rend;

    private Color defaultColor = Color.white;
    private Color flashColor = Color.yellow;
    private Color correctColor = Color.green;

    void Start()
    {
        if (rend == null)
        {
            Debug.LogError("ERROR: No se asignó el Renderer en el cubo: " + gameObject.name);
            return;
        }

        defaultColor = rend.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            puzzle.CubeStepped(cubeID);
        }
    }

    public void Flash()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        if (rend == null) yield break;

        rend.material.color = flashColor;
        yield return new WaitForSeconds(0.5f);
        rend.material.color = defaultColor;
    }

    public void SetCorrect()
    {
        if (rend == null) return;

        rend.material.color = correctColor;
    }

    public void ResetColor()
    {
        if (rend == null) return;

        rend.material.color = defaultColor;
    }
}
