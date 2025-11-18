using UnityEngine;

public class IntroLevel : MonoBehaviour
{
    [TextArea(2, 4)]
    public string introMessage =
        "BIENVENIDO A EL TEMPLO DE LA EDAD ANTIGUA\nSUPERA LOS MINIJUEGOS PARA AVANZAR A LA PIRAMIDE\n \nTEN CUIDADO CON EL TIEMPO";

    public float duration = 4f;

    private void Start()
    {
        if (UIMessageManager.Instance != null)
        {
            // Usamos PRIORIDAD para que nada lo interrumpa
            UIMessageManager.Instance.ShowPriority(introMessage, duration);
        }
    }
}
