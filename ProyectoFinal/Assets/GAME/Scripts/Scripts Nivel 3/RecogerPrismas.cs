using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Efectos de Recolección")]
    public ParticleSystem collectParticles;      // <-- Asigna aquí tu sistema de partículas
    // public AudioClip collectSound;            // <-- (Opcional) sonido de recolección

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //  Reproducir partículas al recoger
            if (collectParticles != null)
            {
                ParticleSystem p = Instantiate(collectParticles, transform.position, Quaternion.identity);
                p.Play();
                Destroy(p.gameObject, 2f); // Se elimina después del efecto
            }

            //  (Opcional) reproducir sonido
            // AudioSource.PlayClipAtPoint(collectSound, transform.position);

            // Llamar al controlador para spawnear el siguiente
            CollectibleController.Instance.SpawnNextCollectible();

            // Destruir este objeto
            Destroy(gameObject);
        }
    }
}
