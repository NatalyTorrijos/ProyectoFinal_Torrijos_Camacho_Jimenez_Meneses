using UnityEngine;

public class Collectible : MonoBehaviour
{

    [Header("Efectos de Recolección")]
    public ParticleSystem collectParticles;      // <-- Asigna aquí tu sistema de partículas
    // public AudioClip collectSound;            // <-- (Opcional) sonido de recolección
    public enum Tipo { Prisma, Llave }
    public Tipo tipo = Tipo.Prisma;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (collectParticles != null)
            {
                ParticleSystem p = Instantiate(collectParticles, transform.position, Quaternion.identity);
                p.Play();
                Destroy(p.gameObject, 2f); // Se elimina después del efecto
            }

            // reproducir sonido
            // AudioSource.PlayClipAtPoint(collectSound, transform.position);

            // Llamar al controlador para spawnear el siguiente
            CollectibleController.Instance.SpawnNextCollectible();
            if (tipo == Tipo.Prisma)
            {
                SceneController.Instance.RegistrarPrisma();
                CollectibleController.Instance.SpawnNextCollectible();
            }
            else if (tipo == Tipo.Llave)
            {
                SceneController.Instance.RegistrarLlave();
            }

            // Destruir este objeto
            Destroy(gameObject);
        }
    }
}


