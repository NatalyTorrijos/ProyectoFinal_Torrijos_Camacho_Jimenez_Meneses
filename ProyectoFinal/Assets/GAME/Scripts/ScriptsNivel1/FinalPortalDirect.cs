using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla el portal final de cada nivel. 
/// Cuando el jugador lo atraviesa:
/// 1. Obtiene el tiempo empleado en el nivel actual
/// 2. Guarda ese tiempo de forma persistente en JSON mediante GameManager
/// 3. Carga la siguiente escena del juego
/// 
/// Se asegura de que solo se active una vez por jugador.
/// </summary>
public class FinalPortalDirect : MonoBehaviour
{
    // ===================================================================
    // CONFIGURACIÓN EN EL INSPECTOR
    // ===================================================================
    [Header("Configuración del Portal")]
    [Tooltip("Nombre exacto de la escena que se cargará al atravesar el portal")]
    public string sceneToLoad = "NIVEL 2";

    // ===================================================================
    // ESTADO INTERNO
    // ===================================================================
    [Header("Estado")]
    [Tooltip("Evita que el portal se active más de una vez")]
    private bool activated = false;

    // ===================================================================
    // DETECCIÓN DE COLISIÓN CON EL JUGADOR
    // ===================================================================
    /// <summary>
    /// Se ejecuta cuando cualquier collider entra en el trigger del portal.
    /// Solo responde al jugador y solo una vez por partida.
    /// </summary>
    /// <param name="other">Collider que entró en el trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        // Evitar activación múltiple
        if (activated) return;

        // Verificar que sea el jugador quien entró
        if (!other.CompareTag("Player")) return;

        // Marcar como activado para evitar ejecución repetida
        activated = true;

        // ---------------------------------------------------------------
        // 1. Obtener el tiempo que tardó el jugador en completar el nivel
        // ---------------------------------------------------------------
        float finalTime = LevelTimer.Instance.GetElapsedTime();

        // ---------------------------------------------------------------
        // 2. Guardar el tiempo de forma persistente en archivo JSON
        // ---------------------------------------------------------------
        GameManager.Instance.SaveLevelTime(finalTime);

        // ---------------------------------------------------------------
        // 3. Cargar la siguiente escena del juego
        // ---------------------------------------------------------------
        SceneManager.LoadScene(sceneToLoad);
    }
}