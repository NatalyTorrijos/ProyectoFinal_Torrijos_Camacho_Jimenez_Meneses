using UnityEngine;

public class MemoryTile : MonoBehaviour
{
    public int tileID;   // ID único de esta losa
    public Renderer tileRenderer;

    public Color baseColor = Color.gray;
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;

    private MemoryPuzzleManager manager;

    private void Start()
    {
        tileRenderer.material.color = baseColor;
        manager = FindObjectOfType<MemoryPuzzleManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        manager.CheckTile(tileID, this);
    }

    // --------- Efectos visuales ---------
    public void FlashCorrect()
    {
        tileRenderer.material.color = correctColor;
        Invoke(nameof(ResetColor), 0.4f);
    }

    public void FlashWrong()
    {
        tileRenderer.material.color = wrongColor;
        Invoke(nameof(ResetColor), 0.7f);
    }

    private void ResetColor()
    {
        tileRenderer.material.color = baseColor;
    }
}
