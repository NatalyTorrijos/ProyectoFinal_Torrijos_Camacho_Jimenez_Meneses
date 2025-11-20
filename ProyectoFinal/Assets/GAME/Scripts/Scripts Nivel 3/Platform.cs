using UnityEngine;
/// <summary>
/// Representa una plataforma que puede ser correcta o incorrecta
/// dentro del minijuego. Cambia su color según el resultado y
/// notifica al SceneController para validar o reiniciar al jugador.
/// </summary>

public class Plataforma : MonoBehaviour
{
    public bool esCorrecta;  
    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void TocarPlataforma()
    {
        if (esCorrecta)
        {
            rend.material.color = Color.yellow;
            SceneController.Instance.RegistrarPlataformaCorrecta(gameObject);
        }
        else
        {
            rend.material.color = Color.black;
            SceneController.Instance.RegistrarPlataformaIncorrecta(gameObject);
        }
    }

    public void ResetColor()
    {
        rend.material.color = Color.white;
    }
}
