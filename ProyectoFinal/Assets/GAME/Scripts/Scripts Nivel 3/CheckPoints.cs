using UnityEngine;

public class PlayerFallReset : MonoBehaviour
{

    /// <summary>
    /// Gestiona el sistema de reinicio del jugador cuando cae o toca
    /// el suelo del nivel. Permite definir múltiples checkpoints
    /// y reposiciona al jugador en el punto de control actual
    /// desactivando temporalmente el CharacterController
    /// para evitar errores de movimiento.
    /// </summary>


    public Transform[] checkpoints;
    private int currentCheckpoint = 0;

    private CharacterController controller;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    /// <summary>
    /// Detecta colisiones del CharacterController.
    /// Si el jugador toca el suelo designado, lo reinicia
    /// al último punto de control guardado.
    /// </summary>

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("SueloNivel3"))
        {
            ResetToCheckpoint();
        }
    }
    /// <summary>
    /// Cambia el punto de control actual al indicado,
    /// permitiendo actualizar el progreso del jugador.
    /// </summary>

    public void SetCheckpoint(int index)
    {
        currentCheckpoint = index;
    }
    /// <summary>
    /// Desactiva temporalmente el CharacterController para evitar errores,
    /// reposiciona al jugador en el punto de control actual
    /// y vuelve a activar el controlador.
    /// </summary>

    private void ResetToCheckpoint()
    {
        controller.enabled = false;
        transform.position = checkpoints[currentCheckpoint].position;
        controller.enabled = true;
    }
}
