using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PushableBlock : MonoBehaviour
{
    public enum Axis { X, Z }

    [Header("Configuración de movimiento")]
    public Axis moveAxis = Axis.X;          // eje permitido
    public float slideSpeed = 2f;           // velocidad de empuje
    public float stopSmoothness = 5f;       // qué tan rápido se frena

    private Rigidbody rb;
    private bool beingPushed = false;
    private Vector3 pushDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // ✅ Solo bloqueamos la rotación, pero NO la posición en Y (puede caer)
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        if (beingPushed)
        {
            Vector3 dir = pushDirection.normalized;

            // Limitar dirección al eje permitido
            if (moveAxis == Axis.X)
                dir = new Vector3(dir.x, 0, 0);
            else
                dir = new Vector3(0, 0, dir.z);

            // Mantener la velocidad actual en Y para que no afecte la caída
            Vector3 newVel = new Vector3(dir.x * slideSpeed, rb.linearVelocity.y, dir.z * slideSpeed);
            rb.linearVelocity = newVel;

            beingPushed = false;
        }
        else
        {
            // Frenado horizontal suave, sin afectar el eje Y
            Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            Vector3 smoothed = Vector3.Lerp(horizontalVel, Vector3.zero, Time.fixedDeltaTime * stopSmoothness);
            rb.linearVelocity = new Vector3(smoothed.x, rb.linearVelocity.y, smoothed.z);
        }
    }

    // Llamado por el jugador
    public void Push(Vector3 direction)
    {
        pushDirection = direction;
        beingPushed = true;
    }
}
