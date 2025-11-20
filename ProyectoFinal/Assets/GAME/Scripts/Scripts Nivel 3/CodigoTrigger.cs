using UnityEngine;
/// <summary>
/// Activa el panel de código cuando el jugador entra en el área del trigger.
/// Se usa para mostrar una interfaz o mensaje al detectar al jugador.
/// </summary>

public class CodigoTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UI_Codigo.Instance.MostrarPanel();
        }
    }
}
