using UnityEngine;

public class LeverWithAnimation : MonoBehaviour
{
    /// <summary>
    /// Controla la palanca que activa la rampa al presionar la tecla k.
    /// Muestra una instrucción al jugador una sola vez, reproduce un sonido
    /// y ejecuta la animación de la palanca cuando el jugador está dentro del área.
    /// </summary>


    public GameObject rampa;
    public bool showInstructionOneTime = true;
    public Animator leverAnimator;         
    public string triggerName = "Levantar"; 
    public AudioClip collectSound;
    private bool playerInRange = false;
    private Renderer rend;

    /// <summary>
    /// Obtiene y almacena el componente Renderer del objeto
    /// para manejar visuales u otros usos posteriores.
    /// </summary>

    void Start()
    {
        rend = GetComponent<Renderer>();
    }
    /// <summary>
    /// Detecta cuándo se presiona la tecla K y, si el jugador
    /// está dentro del área de activación, enciende la rampa,
    /// reproduce el sonido y ejecuta la animación de la palanca.
    /// </summary>

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Presioné K");

            if (!playerInRange)
            {
                Debug.Log("Presioné K pero NO estoy dentro del trigger");
                return;
            }

            
            if (rampa != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
            rampa.SetActive(true);

            
            if (leverAnimator != null)
                leverAnimator.SetTrigger(triggerName);

            Debug.Log("Activé la rampa y la animación de la palanca");
        }
    }
    /// <summary>
    /// Detecta cuando el jugador entra al área de activación.
    /// Habilita la interacción, muestra la instrucción una sola vez
    /// y registra el evento en la consola.
    /// </summary>

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Entré al trigger");
            if (showInstructionOneTime)
            {
                SceneController.Instance.EnqueueInstruction(
                    "Presiona K para activar la palanca.\n y subir la rampa."
                );

                showInstructionOneTime = false; 
            }
        }
    }
    /// <summary>
    /// Detecta cuando el jugador sale del área de activación.
    /// Deshabilita la interacción, oculta la instrucción
    /// y registra el evento en la consola.
    /// </summary>

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            SceneController.Instance.HideInstruction();
            Debug.Log("Salí del trigger");
        }
    }
}






