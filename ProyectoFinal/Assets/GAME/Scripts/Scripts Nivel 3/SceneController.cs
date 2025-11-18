using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    public GameObject instructionPanel;
    public TextMeshProUGUI instructionText;

    private Queue<string> messageQueue = new Queue<string>();
    private bool showingMessage = false;

    private void Awake()
    {
        Instance = this;
        instructionPanel.SetActive(false);
    }

    public void EnqueueInstruction(string msg)
    {
        messageQueue.Enqueue(msg);

        // Si no se está mostrando un mensaje → mostrar el siguiente
        if (!showingMessage)
            ShowNextMessage();
    }

    private void ShowNextMessage()
    {
        if (messageQueue.Count == 0)
        {
            instructionPanel.SetActive(false);
            showingMessage = false;
            return;
        }

        showingMessage = true;

        string nextMsg = messageQueue.Dequeue();
        instructionText.text = nextMsg;
        instructionPanel.SetActive(true);
    }

    public void HideInstruction()
    {
        instructionPanel.SetActive(false);
        showingMessage = false;

        // Mostrar el siguiente mensaje en la cola
        ShowNextMessage();
    }
}
