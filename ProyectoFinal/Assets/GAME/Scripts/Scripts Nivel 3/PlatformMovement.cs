using UnityEngine;
/// <summary>
/// Controla el movimiento oscilante de una plataforma mediante una función seno.
/// La plataforma se desplaza en una dirección específica con una distancia y velocidad definidas.
/// También dibuja guías visuales en la escena para mostrar su trayectoria.
/// </summary>

public class PlatformMovement : MonoBehaviour
{
    [Header("Configuración de movimiento")]
    public Vector3 direction = Vector3.up;   
    public float distance = 2f;              
    public float speed = 2f;                

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; 
    }

    void Update()
    {
        
        float offset = Mathf.Sin(Time.time * speed) * distance;
        transform.position = startPos + direction.normalized * offset;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + direction.normalized * distance);
        Gizmos.DrawWireSphere(transform.position + direction.normalized * distance, 0.2f);
        Gizmos.DrawWireSphere(transform.position - direction.normalized * distance, 0.2f);
    }
}
