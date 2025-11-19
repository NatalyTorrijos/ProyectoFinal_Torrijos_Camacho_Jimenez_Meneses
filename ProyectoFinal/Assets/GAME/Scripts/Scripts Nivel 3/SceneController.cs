using UnityEngine;
using TMPro;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    [Header("Jugador")]
    public GameObject jugador;
    public Transform plataformaInicial;

    private CharacterController cc;

    [Header("Timer")]
    public float maxTime = 30f;
    private float currentTime;
    private bool timerRunning = true;
    public TextMeshProUGUI timerText;

    private Color originalColor;

    [Header("Meta del juego")]
    public GameObject plataformaGanadora;   // 👈 Se asigna desde el Inspector

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        cc = jugador.GetComponent<CharacterController>();
    }

    private void Start()
    {
        originalColor = timerText.color;
        ResetTimer(); // arranca el minijuego con el tiempo completo
    }

    private void Update()
    {
        if (!timerRunning) return;

        currentTime -= Time.deltaTime;

        // Cambiar a rojo cuando queden 5 segundos
        if (currentTime <= 5f)
            timerText.color = Color.red;

        timerText.text = Mathf.Ceil(currentTime).ToString();

        if (currentTime <= 0)
            TiempoAgotado();
    }

    public void ReiniciarJugadorSolo()
    {
        cc.enabled = false;
        jugador.transform.position = plataformaInicial.position + Vector3.up * 1f;
        cc.enabled = true;
    }

    private void TiempoAgotado()
    {
        timerRunning = false;
        ReiniciarJugadorSolo();
        ResetTimer(); // el único caso donde se reinicia el tiempo
    }

    private void ResetTimer()
    {
        currentTime = maxTime;
        timerRunning = true;

        timerText.color = originalColor;
        timerText.text = Mathf.Ceil(maxTime).ToString();
    }

    public void RegistrarPlataformaCorrecta(GameObject plataforma)
    {
        // Si la plataforma correcta es también la meta final
        if (plataforma == plataformaGanadora)
        {
            GanarMinijuego();
        }
    }

    public void RegistrarPlataformaIncorrecta(GameObject plataforma)
    {
        
        ReiniciarJugadorSolo();
    }

    public void GanarMinijuego()
    {
        timerRunning = false; // tiempo se congela
        Debug.Log("Ganaste el minijuego!");
    }
}
