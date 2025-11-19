using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Codigo : MonoBehaviour
{
    public static UI_Codigo Instance;

    [Header("UI")]
    public GameObject panelCodigo;
    public TMP_InputField campoCodigo;
    public TextMeshProUGUI textoError;

    [Header("Código Correcto")]
    public string codigoCorrecto = "7";

    [Header("Control del Jugador")]
    public MonoBehaviour movimientoJugador;
    public MonoBehaviour camaraJugador;

    [Header("Escena Siguiente")]
    public string nombreSiguienteEscena;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        panelCodigo.SetActive(false);
        textoError.text = "";
    }

    // -----------------------------
    //   MOSTRAR PANEL DE CÓDIGO
    // -----------------------------
    public void MostrarPanel()
    {
        panelCodigo.SetActive(true);

        if (movimientoJugador != null) movimientoJugador.enabled = false;
        if (camaraJugador != null) camaraJugador.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        campoCodigo.text = "";
        textoError.text = "";
    }

    // -----------------------------
    //   OCULTAR PANEL
    // -----------------------------
    public void OcultarPanel()
    {
        panelCodigo.SetActive(false);

        if (movimientoJugador != null) movimientoJugador.enabled = true;
        if (camaraJugador != null) camaraJugador.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // -----------------------------
    //   VALIDAR CÓDIGO
    // -----------------------------
    public void ValidarCodigo()
    {
        if (campoCodigo.text == codigoCorrecto)
        {
            textoError.text = "Código correcto";
            PasarSiguienteNivel();
        }
        else
        {
            textoError.text = "Código incorrecto";
        }
    }

    // -----------------------------
    //   PASAR AL SIGUIENTE NIVEL
    // -----------------------------
    private void PasarSiguienteNivel()
    {
        if (!string.IsNullOrEmpty(nombreSiguienteEscena))
        {
            SceneManager.LoadScene(nombreSiguienteEscena);
        }
        else
        {
            Debug.LogWarning("No asignaste la escena siguiente en el inspector.");
        }
    }
}

