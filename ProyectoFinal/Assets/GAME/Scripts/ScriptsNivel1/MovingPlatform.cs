using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Plataforma móvil que recorre una secuencia de waypoints en bucle.
/// Soporta de forma física correcta:
/// • Jugadores con CharacterController (usando trigger + cc.Move)
/// • Objetos con Rigidbody (usando OnCollision + rb.MovePosition)
/// 
/// Incluye tiempo de parada configurable en cada waypoint y movimiento suave.
/// Ideal para puzles de plataformas dinámicas y niveles avanzados.
/// </summary>
[RequireComponent(typeof(Collider))]
public class MovingPlatform : MonoBehaviour
{
    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Header("Ruta de Movimiento")]
    [Tooltip("Array de puntos por los que pasará la plataforma. Mínimo 2 waypoints")]
    public Transform[] waypoints;

    [Header("Parámetros de Movimiento")]
    [Tooltip("Velocidad de desplazamiento en unidades por segundo")]
    public float speed = 2f;

    [Tooltip("Tiempo en segundos que la plataforma se detiene en cada waypoint")]
    public float stopTime = 0.5f;

    // ===================================================================
    // LISTAS DE OBJETOS AFECTADOS
    // ===================================================================
    private List<Rigidbody> rigidbodies = new List<Rigidbody>();           // Bloques y objetos físicos
    private List<CharacterController> players = new List<CharacterController>(); // Jugadores sobre la plataforma

    // ===================================================================
    // ESTADO INTERNO
    // ===================================================================
    private int currentIndex = 0;        // Índice actual del waypoint objetivo
    private float waitTimer = 0f;         // Contador para el tiempo de parada

    // ===================================================================
    // INICIALIZACIÓN
    // ===================================================================
    private void Start()
    {
        // Registrar posición inicial para cálculos de delta
        if (waypoints.Length > 0)
            transform.position = waypoints[0].position;
    }

    // ===================================================================
    // ACTUALIZACIÓN DEL MOVIMIENTO (CADA FRAME)
    // ===================================================================
    private void Update()
    {
        // Necesita al menos 2 waypoints para funcionar
        if (waypoints == null || waypoints.Length < 2) return;

        // Guardar posición antes del movimiento para calcular delta
        Vector3 beforeMove = transform.position;

        // Obtener waypoint objetivo actual
        Transform target = waypoints[currentIndex];

        // Mover la plataforma suavemente hacia el objetivo
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Detectar llegada al waypoint (con pequeña tolerancia)
        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            waitTimer += Time.deltaTime;

            // Esperar el tiempo configurado antes de pasar al siguiente
            if (waitTimer >= stopTime)
            {
                waitTimer = 0f;
                currentIndex++;

                // Bucle infinito al llegar al final
                if (currentIndex >= waypoints.Length)
                    currentIndex = 0;
            }
        }
        else
        {
            waitTimer = 0f; // Reiniciar espera si aún no llegó
        }

        // Calcular cuánto se movió la plataforma este frame
        Vector3 deltaMovement = transform.position - beforeMove;

        // ===================================================================
        // MOVER OBJETOS FÍSICOS (RIGIDBODY)
        // ===================================================================
        for (int i = rigidbodies.Count - 1; i >= 0; i--)
        {
            var rb = rigidbodies[i];
            if (rb != null)
                rb.MovePosition(rb.position + deltaMovement);
            else
                rigidbodies.RemoveAt(i); // Limpieza de referencias nulas
        }

        // ===================================================================
        // MOVER JUGADORES (CHARACTER CONTROLLER)
        // ===================================================================
        for (int i = players.Count - 1; i >= 0; i--)
        {
            var cc = players[i];
            if (cc != null)
                cc.Move(deltaMovement);
            else
                players.RemoveAt(i); // Limpieza de referencias destruidas
        }
    }

    // ===================================================================
    // DETECCIÓN DE JUGADORES (USANDO TRIGGER)
    // ===================================================================
    /// <summary>
    /// Detecta cuando un jugador entra en contacto con la plataforma (debe tener trigger activado).
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null && !players.Contains(cc))
        {
            players.Add(cc);
        }
    }

    /// <summary>
    /// Detecta cuando el jugador abandona la plataforma.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null && players.Contains(cc))
        {
            players.Remove(cc);
        }
    }

    // ===================================================================
    // DETECCIÓN DE OBJETOS FÍSICOS (USANDO COLISIÓN)
    // ===================================================================
    /// <summary>
    /// Detecta bloques empujables u objetos con Rigidbody que están sobre la plataforma.
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.collider.attachedRigidbody;
        if (rb != null && !rigidbodies.Contains(rb) && !rb.isKinematic)
        {
            rigidbodies.Add(rb);
        }
    }

    /// <summary>
    /// Remueve objetos que ya no están en contacto con la plataforma.
    /// </summary>
    private void OnCollisionExit(Collision collision)
    {
        Rigidbody rb = collision.collider.attachedRigidbody;
        if (rb != null && rigidbodies.Contains(rb))
        {
            rigidbodies.Remove(rb);
        }
    }
}
