using UnityEngine;
using UnityEngine.InputSystem;

public class LadderClimb : MonoBehaviour
{
    public float climbSpeed = 4f;

    private bool playerInside = false;
    private bool climbing = false;

    private CharacterController controller;
    private PlayerMovement playerMovement;
    private Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            controller = other.GetComponentInChildren<CharacterController>();
            playerMovement = other.GetComponentInChildren<PlayerMovement>();
            anim = other.GetComponentInChildren<Animator>();

            // Mostrar instrucciones inmediatamente
            SceneController.Instance.EnqueueInstruction(
                "Presiona E para subir la escalera.\nCuando llegues arriba, salta o presiona E para salir."
            );
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            climbing = false;

            if (playerMovement != null)
                playerMovement.canMove = true;

            if (anim != null)
            {
                anim.transform.localRotation = Quaternion.identity;
                anim.SetBool("IsClimbing", false);
                anim.SetFloat("ClimbSpeed", 0f);
                anim.speed = 1f;
            }

            // Ocultar instrucciones al salir del trigger
            SceneController.Instance.HideInstruction();

            Debug.Log("Salió del trigger. Escalar OFF");
        }
    }

    private void Update()
    {
        if (!playerInside) return;

        bool pressedE = (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                        || Input.GetKeyDown(KeyCode.E);

        if (pressedE)
        {
            climbing = !climbing;

            if (playerMovement != null)
                playerMovement.canMove = !climbing;

            if (anim != null)
                anim.SetBool("IsClimbing", climbing);

            Debug.Log("Modo escalar: " + climbing);
        }

        if (!climbing || controller == null) return;

        float vertical = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) vertical = 1f;
            else if (Keyboard.current.sKey.isPressed) vertical = -1f;
            else vertical = 0f;
        }
        else
        {
            float rawV = Input.GetAxisRaw("Vertical");
            if (rawV > 0.1f) vertical = 1f;
            else if (rawV < -0.1f) vertical = -1f;
            else vertical = 0f;
        }

        if (anim != null)
            anim.SetFloat("ClimbSpeed", vertical);

        Vector3 move = Vector3.up * vertical * climbSpeed;
        controller.Move(move * Time.deltaTime);
    }
}
