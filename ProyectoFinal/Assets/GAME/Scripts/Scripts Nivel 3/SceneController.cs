using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    [Header("Coleccionables (Prismas)")]
    public int totalPrismas = 5;        // Lo defines en el inspector
    public int prismasRestantes;        // Se actualiza solo
    public TextMeshProUGUI prismasText; // Texto UI que muestra la cantidad

    [Header("Llave")]
    public bool llaveObtenida = false;
    public TextMeshProUGUI llaveText;   // Texto UI que muestra si la llave está recogida

    [Header("UI MiniJuego")]
    public GameObject instructionPanel;
    public TextMeshProUGUI instructionText;

    private Queue<string> instructionQueue = new Queue<string>();
    private bool isShowing = false;

    [Header("Jugador")]
    public GameObject jugador;
    public Transform plataformaInicial;

    private CharacterController cc;

    [Header("Timer (Minijuego)")]
    public bool minijuegoActivo = false;  // 🔥 NUEVO: controla si el minijuego está activo
    private float maxTime = 30f;
    private float currentTime;
    private bool timerRunning = true;
    public TextMeshProUGUI timerText;

    private Color originalColor;

    [Header("Meta del juego")]
    public GameObject plataformaGanadora;

    // Getters y Setters existentes
    public GameObject Jugador { get => jugador; set => jugador = value; }
    public Transform PlataformaInicial { get => plataformaInicial; set => plataformaInicial = value; }
    public CharacterController Cc { get => cc; set => cc = value; }
    public float MaxTime { get => maxTime; set => maxTime = value; }
    public float CurrentTime { get => currentTime; set => currentTime = value; }
    public bool TimerRunning { get => timerRunning; set => timerRunning = value; }
    public TextMeshProUGUI TimerText { get => timerText; set => timerText = value; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        cc = jugador.GetComponent<CharacterController>();
    }

    private void Start()
    {
        originalColor = timerText.color;
        ResetTimer();

        prismasRestantes = totalPrismas;
        if (prismasText != null)
            prismasText.text =  prismasRestantes.ToString();

        // Inicializar llave
        if (llaveText != null)
            llaveText.text = 0.ToString();
    }

    private void Update()
    {
        // 🔥 El minijuego NO corre si no está activo
        if (!minijuegoActivo) return;

        if (!timerRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 5f)
            timerText.color = Color.red;

        timerText.text = Mathf.Ceil(currentTime).ToString();

        if (currentTime <= 0)
            TiempoAgotado();
    }
    public void RegistrarPrisma()
    {
        prismasRestantes--;

        if (prismasRestantes < 0)
            prismasRestantes = 0;

        if (prismasText != null)
            prismasText.text = prismasRestantes.ToString();
    }
    public void RegistrarLlave()
    {
        llaveObtenida = true;

        if (llaveText != null)
            llaveText.text = 1.ToString();
    }

    public void EnqueueInstruction(string message)
    {
        instructionQueue.Enqueue(message);
        TryShowNext();
    }

    private void TryShowNext()
    {
        if (isShowing) return;
        if (instructionQueue.Count == 0) return;

        string nextMessage = instructionQueue.Dequeue();
        instructionPanel.SetActive(true);
        instructionText.text = nextMessage;
        isShowing = true;
    }

    public void HideInstruction()
    {
        instructionPanel.SetActive(false);
        isShowing = false;
        TryShowNext();
    }

    public void ReiniciarJugadorSolo()
    {
        // 🔥 Solo permite reiniciar dentro del minijuego
        if (!minijuegoActivo) return;

        Debug.Log(" ReiniciarJugadorSolo() LLAMADO");

        cc.enabled = false;
        jugador.transform.position = plataformaInicial.position + Vector3.up * 1f;
        cc.enabled = true;
    }

    private void TiempoAgotado()
    {
        if (!minijuegoActivo) return;  // 🔥 Protección

        timerRunning = false;
        ReiniciarJugadorSolo();
        ResetTimer();
    }

    public void ResetTimer()
    {
        if (!minijuegoActivo) return;  // 🔥 NO resetea si no está en minijuego

        currentTime = maxTime;
        timerRunning = true;

        timerText.color = originalColor;
        timerText.text = Mathf.Ceil(maxTime).ToString();
    }

    public void RegistrarPlataformaCorrecta(GameObject plataforma)
    {
        if (!minijuegoActivo) return; // 🔥 SOLO valida plataformas en el minijuego

        if (plataforma == plataformaGanadora)
        {
            GanarMinijuego();
        }
    }

    public void RegistrarPlataformaIncorrecta(GameObject plataforma)
    {
        if (!minijuegoActivo) return; // No reinicia fuera del minijuego

        ReiniciarJugadorSolo();
    }

    public void GanarMinijuego()
    {
        if (!minijuegoActivo) return;

        timerRunning = false;
        Debug.Log("Ganaste el minijuego!");
    }
}
