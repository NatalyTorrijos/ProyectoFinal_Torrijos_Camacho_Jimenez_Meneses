using UnityEngine;

public class PuertaMinijuego : MonoBehaviour
{
    public GameObject fabricaMinijuego;            // OBJETO QUE ACTIVAREMOS
    public Transform puntoEntradaMinijuego;        // DONDE APARECE EL JUGADOR

    private bool dentro = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            dentro = true;
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
            IniciarMinijuego();
        }
    }

    void IniciarMinijuego()
    {
        Debug.Log("📌 INICIANDO MINIJUEGO...");

        // 1. Activar toda la fábrica
        fabricaMinijuego.SetActive(true);

        // 2. Activar estado de minijuego
        SceneController.Instance.minijuegoActivo = true;

        // 3. Configurar la plataforma inicial del SceneController
        SceneController.Instance.PlataformaInicial = puntoEntradaMinijuego;

        // 4. Teletransportar jugador
        var cc = SceneController.Instance.Jugador.GetComponent<CharacterController>();
        cc.enabled = false;
        SceneController.Instance.Jugador.transform.position = puntoEntradaMinijuego.position + Vector3.up * 1f;
        cc.enabled = true;

        // 5. Reiniciar el timer (solo funciona ahora que minijuegoActivo=true)
        SceneController.Instance.ResetTimer();

        // 6. Activar UI del tiempo
        SceneController.Instance.TimerText.gameObject.SetActive(true);

        // 7. Activar el conteo
        SceneController.Instance.TimerRunning = true;

        Debug.Log("🎮 Minijuego iniciado.");
    }
}

