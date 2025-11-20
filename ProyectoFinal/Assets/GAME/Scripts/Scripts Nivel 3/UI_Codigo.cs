using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// Gestiona la interfaz de ingreso de código del jugador.
/// Permite mostrar y ocultar el panel, deshabilitar temporalmente el movimiento y la cámara,
/// validar el código ingresado y, si es correcto, cargar la siguiente escena.
/// </summary>

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
    /// <summary>
    /// Muestra el panel de código, desactiva el movimiento y la cámara del jugador,
    /// desbloquea el cursor y limpia los campos de texto.
    /// </summary>

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
    /// <summary>
    /// Oculta el panel de código, reactiva el movimiento y la cámara del jugador,
    /// bloquea el cursor y lo oculta.
    /// </summary>

    public void OcultarPanel()
    {
        panelCodigo.SetActive(false);

        if (movimientoJugador != null) movimientoJugador.enabled = true;
        if (camaraJugador != null) camaraJugador.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Compara el texto ingresado con el código correcto.
    /// Si es correcto, muestra mensaje de éxito y carga la siguiente escena;
    /// si no, muestra mensaje de error.
    /// </summary>

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
    /// <summary>
    /// Carga la siguiente escena si está asignada en el inspector,
    /// o muestra una advertencia si no se ha definido.
    /// </summary>

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

