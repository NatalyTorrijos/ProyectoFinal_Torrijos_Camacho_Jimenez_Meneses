using UnityEngine;
/// <summary>
/// Gestiona el comportamiento de un coleccionable (prisma o llave).
/// Al ser recogido por el jugador, reproduce efectos visuales y de sonido,
/// actualiza el progreso del minijuego y ajusta el checkpoint del jugador.
/// </summary>

public class Collectible : MonoBehaviour
{

    [Header("Efectos de Recolección")]
    public ParticleSystem collectParticles;      
    public AudioClip collectSound;            
    public enum Tipo { Prisma, Llave }
    public Tipo tipo = Tipo.Prisma;

    public int checkpointIndex = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (collectParticles != null)
            {
                ParticleSystem p = Instantiate(collectParticles, transform.position, Quaternion.identity);
                p.Play();
                Destroy(p.gameObject, 2f); 
            }

           
             AudioSource.PlayClipAtPoint(collectSound, transform.position);

          
           
            if (tipo == Tipo.Prisma)
            {
                CollectibleController.Instance.SpawnNextCollectible();
                SceneController.Instance.RegistrarPrisma();
                other.GetComponent<PlayerFallReset>().SetCheckpoint(checkpointIndex);

            }
            else if (tipo == Tipo.Llave)
            {
                SceneController.Instance.RegistrarLlave();
                other.GetComponent<PlayerFallReset>().SetCheckpoint(checkpointIndex);
            }

            

            Destroy(gameObject);
        }
    }
}


