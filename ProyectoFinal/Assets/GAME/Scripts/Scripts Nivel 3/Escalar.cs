using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Gestiona el comportamiento de escalada en escaleras.
/// Detecta cuándo el jugador entra o sale del área, activa o desactiva
/// el modo de escalada, controla la animación correspondiente y mueve
/// al jugador verticalmente según la entrada del teclado.
/// </summary>

public class LadderClimb : MonoBehaviour
{
    public float climbSpeed = 4f;
    public bool showInstructionOneTime = true;
    private bool playerInside = false;
    private bool climbing = false;

    private CharacterController controller;
    private PlayerMovement playerMovement;
    private Animator anim;

    /// <summary>
    /// Detecta cuando el jugador entra en el área de la escalera.
    /// Obtiene las referencias necesarias, muestra la instrucción
    /// una sola vez y habilita la posibilidad de escalar.
    /// </summary>

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            controller = other.GetComponentInChildren<CharacterController>();
            playerMovement = other.GetComponentInChildren<PlayerMovement>();
            anim = other.GetComponentInChildren<Animator>();

            Debug.Log("Entró al trigger, controller: " + (controller != null));

            if (showInstructionOneTime)
            {
                SceneController.Instance.EnqueueInstruction(
                    "Presiona E para subir la escalera.\nCuando llegues arriba, salta o presiona E para salir."
                );

                showInstructionOneTime = false; 
            }
        }
    }
    /// <summary>
    /// Detecta cuando el jugador sale del área de la escalera.
    /// Desactiva el modo de escalada, devuelve el control normal
    /// del movimiento y restablece las animaciones.
    /// </summary>

    void OnTriggerExit(Collider other)
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
            SceneController.Instance.HideInstruction();
            Debug.Log("Salió del trigger. Escalar OFF");
        }
    }
    /// <summary>
    /// Escucha la tecla E para activar/desactivar el modo de escalada,
    /// controla el movimiento vertical del jugador mientras escala
    /// y actualiza los parámetros de animación correspondientes.
    /// </summary>

    void Update()
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
        {
            anim.SetFloat("ClimbSpeed", vertical);
        }

    
        Vector3 move = Vector3.up * vertical * climbSpeed;
        controller.Move(move * Time.deltaTime);
    }
}



