using UnityEngine;

public class Plataforma : MonoBehaviour
{
    public bool esCorrecta;   // Activa si es parte del camino correcto
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
