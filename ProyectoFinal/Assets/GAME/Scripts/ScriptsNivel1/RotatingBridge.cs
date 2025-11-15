using UnityEngine;

public class RotatingBridge : MonoBehaviour
{
    [Header("Rotación")]
    public float rotationSpeed = 40f;

    [Header("Movimiento Opcional")]
    public bool moveForward = false;
    public float moveSpeed = 2f;
    public Vector3 moveDirection = Vector3.forward;

    private bool active = false;

    private void Update()
    {
        if (!active) return;

        // Rotación constante
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

        // Movimiento opcional (si quieres que avance también)
        if (moveForward)
            transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
    }

    // Se llama desde el manager
    public void ActivateBridge()
    {
        active = true;
        gameObject.SetActive(true);
    }
}
