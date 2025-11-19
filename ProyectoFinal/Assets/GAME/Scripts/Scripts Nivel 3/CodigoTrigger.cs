using UnityEngine;

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
