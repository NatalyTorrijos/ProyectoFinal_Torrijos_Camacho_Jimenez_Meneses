using UnityEngine;
/// <summary>
/// Controla el acceso al minijuego cuando el jugador se acerca a la puerta.
/// Muestra una instrucción para entrar, detecta la tecla de acceso y,
/// al iniciar el minijuego, activa la fábrica, mueve al jugador a la entrada,
/// reinicia el temporizador y muestra un panel introductorio temporal.
/// </summary>

public class PuertaMinijuego : MonoBehaviour
{
    public GameObject fabricaMinijuego;
    public Transform puntoEntradaMinijuego;

   
    public GameObject panelInstruccionMinijuego;

    private bool dentro = false;
    /// <summary>
    /// Detecta cuando el jugador entra en el área de la puerta.
    /// Marca que está dentro y muestra la instrucción para entrar al minijuego.
    /// </summary>

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            dentro = true;

        SceneController.Instance.EnqueueInstruction(
            "Presiona T para entrar a la fábrica"
        );
    }
    /// <summary>
    /// Detecta cuando el jugador sale del área de la puerta
    /// y desactiva la posibilidad de entrar al minijuego.
    /// </summary>

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            dentro = false;
    }

    private void Update()
    {
        if (dentro && Input.GetKeyDown(KeyCode.T))
        {
            SceneController.Instance.HideInstruction();
            IniciarMinijuego();
        }
    }
    /// <summary>
    /// Activa la fábrica del minijuego, habilita el temporizador,
    /// reposiciona al jugador en la entrada del minijuego y muestra
    /// un panel de instrucciones temporal.
    /// </summary>

    void IniciarMinijuego()
    {
        Debug.Log("INICIANDO MINIJUEGO...");

        fabricaMinijuego.SetActive(true);
        SceneController.Instance.minijuegoActivo = true;

      
        var cc = SceneController.Instance.Jugador.GetComponent<CharacterController>();
        cc.enabled = false;
        SceneController.Instance.Jugador.transform.position = puntoEntradaMinijuego.position + Vector3.up * 1f;
        cc.enabled = true;

       
        SceneController.Instance.ResetTimer();
        SceneController.Instance.TimerText.gameObject.SetActive(true);
        SceneController.Instance.TimerRunning = true;

        if (panelInstruccionMinijuego != null)
        {
            panelInstruccionMinijuego.SetActive(true);
            StartCoroutine(CerrarPanelDespuesDeTiempo(5f)); 
        }
        else
        {
            Debug.LogWarning("No asignaste el panel de instrucción del minijuego en el inspector.");
        }
    }


    private System.Collections.IEnumerator CerrarPanelDespuesDeTiempo(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);

        if (panelInstruccionMinijuego != null)
            panelInstruccionMinijuego.SetActive(false);
    }
}

