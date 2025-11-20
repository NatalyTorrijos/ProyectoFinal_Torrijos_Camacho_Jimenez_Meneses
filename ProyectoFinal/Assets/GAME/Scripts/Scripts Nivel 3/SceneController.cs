using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    /// <summary>
    /// Controla toda la lógica del minijuego y el progreso del jugador.
    /// Administra los coleccionables, la llave, el temporizador,
    /// la cola de instrucciones en pantalla y el reinicio del jugador.
    /// También gestiona la detección de plataformas correctas o incorrectas
    /// y define la condición de victoria del minijuego.
    /// </summary>


    public static SceneController Instance;
    [Header("Coleccionables (Prismas)")]
    public int totalPrismas = 5;     
    public int prismasRestantes;        
    public TextMeshProUGUI prismasText; 

    [Header("Llave")]
    public bool llaveObtenida = false;
    public TextMeshProUGUI llaveText;  

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
    public bool minijuegoActivo = false;  
    private float maxTime = 30f;
    private float currentTime;
    private bool timerRunning = true;
    public TextMeshProUGUI timerText;

    private Color originalColor;

    [Header("Meta del juego")]
    public GameObject plataformaGanadora;

  
    public GameObject Jugador { get => jugador; set => jugador = value; }
    public Transform PlataformaInicial { get => plataformaInicial; set => plataformaInicial = value; }
    public CharacterController Cc { get => cc; set => cc = value; }
    public float MaxTime { get => maxTime; set => maxTime = value; }
    public float CurrentTime { get => currentTime; set => currentTime = value; }
    public bool TimerRunning { get => timerRunning; set => timerRunning = value; }
    public TextMeshProUGUI TimerText { get => timerText; set => timerText = value; }

    /// <summary>
    /// Inicializa la instancia del SceneController y obtiene el
    /// CharacterController del jugador para acciones posteriores.
    /// </summary>

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

        
        if (llaveText != null)
            llaveText.text = 0.ToString();
    }

    private void Update()
    {
       
        if (!minijuegoActivo) return;

        if (!timerRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 5f)
            timerText.color = Color.red;

        timerText.text = Mathf.Ceil(currentTime).ToString();

        if (currentTime <= 0)
            TiempoAgotado();
    }
    /// <summary>
    /// Reduce el contador de prismas restantes y actualiza el texto en pantalla.
    /// </summary>

    public void RegistrarPrisma()
    {
        prismasRestantes--;

        if (prismasRestantes < 0)
            prismasRestantes = 0;

        if (prismasText != null)
            prismasText.text = prismasRestantes.ToString();
    }

    /// <summary>
    /// Marca que la llave fue obtenida y actualiza la UI correspondiente.
    /// </summary>

    public void RegistrarLlave()
    {
        llaveObtenida = true;

        if (llaveText != null)
            llaveText.text = 1.ToString();
    }
    /// <summary>
    /// Agrega un mensaje a la cola de instrucciones y
    /// trata de mostrarlo si no hay otro en pantalla.
    /// </summary>

    public void EnqueueInstruction(string message)
    {
        instructionQueue.Enqueue(message);
        TryShowNext();
    }
    /// <summary>
    /// Muestra la siguiente instrucción de la cola si no hay una activa.
    /// </summary>

    private void TryShowNext()
    {
        if (isShowing) return;
        if (instructionQueue.Count == 0) return;

        string nextMessage = instructionQueue.Dequeue();
        instructionPanel.SetActive(true);
        instructionText.text = nextMessage;
        isShowing = true;
    }
    /// <summary>
    /// Oculta la instrucción actual y permite que se muestre la siguiente.
    /// </summary>

    public void HideInstruction()
    {
        instructionPanel.SetActive(false);
        isShowing = false;
        TryShowNext();
    }
    /// <summary>
    /// Reposiciona al jugador en la plataforma inicial
    /// desactivando temporalmente el CharacterController.
    /// Solo funciona si el minijuego está activo.
    /// </summary>

    public void ReiniciarJugadorSolo()
    {
        
        if (!minijuegoActivo) return;

        Debug.Log(" ReiniciarJugadorSolo() LLAMADO");

        cc.enabled = false;
        jugador.transform.position = plataformaInicial.position + Vector3.up * 1f;
        cc.enabled = true;
    }
    /// <summary>
    /// Detiene el temporizador, reinicia al jugador y restablece el tiempo
    /// cuando llega a cero.
    /// </summary>

    private void TiempoAgotado()
    {
        if (!minijuegoActivo) return;  

        timerRunning = false;
        ReiniciarJugadorSolo();
        ResetTimer();
    }
    /// <summary>
    /// Restaura el temporizador al máximo, reinicia su color y actualiza la UI.
    /// Solo funciona cuando el minijuego está activado.
    /// </summary>

    public void ResetTimer()
    {
        if (!minijuegoActivo) return;  

        currentTime = maxTime;
        timerRunning = true;

        timerText.color = originalColor;
        timerText.text = Mathf.Ceil(maxTime).ToString();
    }
    /// <summary>
    /// Comprueba si la plataforma tocada es la ganadora
    /// y activa la victoria del minijuego.
    /// </summary>

    public void RegistrarPlataformaCorrecta(GameObject plataforma)
    {
        if (!minijuegoActivo) return; 

        if (plataforma == plataformaGanadora)
        {
            GanarMinijuego();
        }
    }
    /// <summary>
    /// Reinicia al jugador si pisa una plataforma equivocada.
    /// </summary>

    public void RegistrarPlataformaIncorrecta(GameObject plataforma)
    {
        if (!minijuegoActivo) return; 

        ReiniciarJugadorSolo();
    }
    /// <summary>
    /// Marca el minijuego como completado y detiene el temporizador.
    /// </summary>

    public void GanarMinijuego()
    {
        if (!minijuegoActivo) return;

        timerRunning = false;
        Debug.Log("Ganaste el minijuego!");
    }
}
