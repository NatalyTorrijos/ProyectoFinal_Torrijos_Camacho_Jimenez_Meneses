using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class MovingPlatform : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] waypoints;

    [Header("Movement")]
    public float speed = 2f;
    public float stopTime = 0.5f;

    private List<Rigidbody> rigidbodies = new List<Rigidbody>();
    private List<CharacterController> players = new List<CharacterController>();

    private int currentIndex = 0;
    private float waitTimer = 0;

    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        if (waypoints.Length < 2) return;

        Vector3 beforeMove = transform.position;

        // Mover plataforma hacia el waypoint actual
        Transform target = waypoints[currentIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Si llegó al waypoint
        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= stopTime)
            {
                waitTimer = 0;
                currentIndex++;
                if (currentIndex >= waypoints.Length)
                    currentIndex = 0;
            }
        }

        // Δ de movimiento de la plataforma
        Vector3 delta = transform.position - beforeMove;

        // --- MOVER BLOQUES (rigidbody) ---
        foreach (var rb in rigidbodies)
        {
            if (rb != null)
                rb.MovePosition(rb.position + delta);
        }

        // --- MOVER JUGADORES (CharacterController) ---
        foreach (var cc in players)
        {
            if (cc != null)
                cc.Move(delta);
        }
    }

    // ===============================
    //  DETECCIÓN DE JUGADOR (TRIGGER)
    // ===============================
    private void OnTriggerEnter(Collider other)
    {
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null && !players.Contains(cc))
            players.Add(cc);
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null && players.Contains(cc))
            players.Remove(cc);
    }

    // ==================================
    //  DETECCIÓN DE BLOQUES (COLISIÓN)
    // ==================================
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.collider.attachedRigidbody;
        if (rb != null && !rigidbodies.Contains(rb))
            rigidbodies.Add(rb);
    }

    private void OnCollisionExit(Collision collision)
    {
        Rigidbody rb = collision.collider.attachedRigidbody;
        if (rb != null && rigidbodies.Contains(rb))
            rigidbodies.Remove(rb);
    }
}
