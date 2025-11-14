using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [Header("Detección")]
    public float rangoDeteccion = 10f;
    public float rangoAtaque = 2f;

    [Header("Movimiento")]
    public float velocidad = 3f;
    public float velocidadRotacion = 5f;

    [Header("Vida")]
    public int vidaMax = 100;
    public int vidaActual;
    bool estaMuerto = false;

    Transform jugador;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            jugador = playerObj.transform;
        }

        vidaActual = vidaMax;
    }

    void Update()
    {
        if (estaMuerto) return;
        if (jugador == null) return;

        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia <= rangoAtaque)
        {
            Atacar();
        }
        else if (distancia <= rangoDeteccion)
        {
            Perseguir();
        }
        else
        {
            Idle();
        }
    }

    // -----------------------------------------------------------
    //                        ESTADOS
    // -----------------------------------------------------------

    void Idle()
    {
        anim.SetFloat("VelX", 0);
        anim.SetFloat("VelY", 0);
    }

    void Perseguir()
    {
        // Rotar hacia el jugador
        Vector3 direccion = (jugador.position - transform.position);
        direccion.y = 0;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(direccion),
            Time.deltaTime * velocidadRotacion
        );

        // Mover hacia adelante
        transform.position += transform.forward * velocidad * Time.deltaTime;

        // Animaciones (Blend Tree)
        anim.SetFloat("VelX", 0);
        anim.SetFloat("VelY", 1);
    }

    void Atacar()
    {
        // Acercarse un poco más para evitar que ataque desde lejos
        transform.position = Vector3.MoveTowards(
            transform.position,
            jugador.position,
            1.5f * Time.deltaTime
        );

        anim.SetFloat("VelX", 0);
        anim.SetFloat("VelY", 0);

        anim.SetTrigger("AttackTrigger");
    }

    // -----------------------------------------------------------
    //                   SISTEMA DE DAÑO Y MUERTE
    // -----------------------------------------------------------

    public void RecibirDaño(int cantidad)
    {
        if (estaMuerto) return;

        vidaActual -= cantidad;

        anim.SetTrigger("Damage");

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        estaMuerto = true;

        // Frenar movimiento
        anim.SetFloat("VelX", 0);
        anim.SetFloat("VelY", 0);

        // Animación de muerte
        anim.SetTrigger("Die");

        // Desactivar este script
        this.enabled = false;

        // Desactivar colisionador
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;


    }
}
