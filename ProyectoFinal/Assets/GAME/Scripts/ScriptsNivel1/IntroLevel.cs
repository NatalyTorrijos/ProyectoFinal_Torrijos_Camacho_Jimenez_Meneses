using UnityEngine;

/// <summary>
/// Muestra un mensaje de bienvenida e instrucciones al iniciar cualquier nivel.
/// Se ejecuta automáticamente en Start() y utiliza el sistema de mensajes prioritarios
/// para que nada lo interrumpa durante los primeros segundos del juego.
/// Mejora la inmersión y asegura que el jugador entienda el objetivo y mecánicas básicas.
/// </summary>
public class IntroLevel : MonoBehaviour
{
    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Header("Mensaje de Introducción")]
    [TextArea(2, 4)]
    [Tooltip("Texto que aparecerá al comenzar el nivel. Admite saltos de línea con \\n")]
    public string introMessage =
        "BIENVENIDO A EL TEMPLO DE LA EDAD ANTIGUA\nSUPERA LOS MINIJUEGOS PARA AVANZAR A LA PIRÁMIDE\n \nTEN CUIDADO CON EL TIEMPO";

    [Header("Duración")]
    [Tooltip("Tiempo en segundos que permanecerá visible el mensaje")]
    public float duration = 4f;

    // ===================================================================
    // EJECUCIÓN AL INICIAR LA ESCENA
    // ===================================================================
    /// <summary>
    /// Se llama automáticamente al cargar la escena.
    /// Muestra el mensaje de introducción usando prioridad alta para que no sea
    /// interrumpido por hints o mensajes normales.
    /// </summary>
    private void Start()
    {
        // Verifica que el sistema de mensajes esté disponible
        if (UIMessageManager.Instance != null)
        {
            // PRIORITY = nadie lo puede sobreescribir hasta que termine
            UIMessageManager.Instance.ShowPriority(introMessage, duration);
        }
        else
        {
            Debug.LogWarning("IntroLevel: No se encontró UIMessageManager en la escena.");
        }
    }
}