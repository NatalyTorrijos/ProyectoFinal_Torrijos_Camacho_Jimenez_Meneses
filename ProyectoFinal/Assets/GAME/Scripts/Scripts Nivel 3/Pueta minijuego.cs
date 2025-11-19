using UnityEngine;

public class PuertaMinijuego : MonoBehaviour
{
    public GameObject fabricaMinijuego;
    public Transform puntoEntradaMinijuego;

    // Panel de instrucciones del minijuego
    public GameObject panelInstruccionMinijuego;

    private bool dentro = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            dentro = true;

        SceneController.Instance.EnqueueInstruction(
            "Presiona T para entrar a la fábrica"
        );
    }

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

    void IniciarMinijuego()
    {
        Debug.Log("INICIANDO MINIJUEGO...");

        fabricaMinijuego.SetActive(true);
        SceneController.Instance.minijuegoActivo = true;

        // Teletransportar jugador
        var cc = SceneController.Instance.Jugador.GetComponent<CharacterController>();
        cc.enabled = false;
        SceneController.Instance.Jugador.transform.position = puntoEntradaMinijuego.position + Vector3.up * 1f;
        cc.enabled = true;

        // Timer
        SceneController.Instance.ResetTimer();
        SceneController.Instance.TimerText.gameObject.SetActive(true);
        SceneController.Instance.TimerRunning = true;

        // Mostrar instrucción del minijuego
        if (panelInstruccionMinijuego != null)
        {
            panelInstruccionMinijuego.SetActive(true);
            StartCoroutine(CerrarPanelDespuesDeTiempo(5f)); // ⬅ cierre automático
        }
        else
        {
            Debug.LogWarning("No asignaste el panel de instrucción del minijuego en el inspector.");
        }
    }

    // ⬇ NUEVA CORRUTINA PARA CERRAR PANEL
    private System.Collections.IEnumerator CerrarPanelDespuesDeTiempo(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);

        if (panelInstruccionMinijuego != null)
            panelInstruccionMinijuego.SetActive(false);
    }
}

