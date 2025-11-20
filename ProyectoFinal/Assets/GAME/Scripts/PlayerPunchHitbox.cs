using UnityEngine;
/// <summary>
/// en este script lo que se hace es que con la hitbox del PLayer en la mano izquierda, y lo evalua si el objeto que esta pegando 
/// tiene alguna componente en este caso Tag o layer llamado Enemy.
/// </summary>

public class PlayerPunchHitbox : MonoBehaviour
{
    [Header("Config")]
    public float radius = 0.6f;          
    public int damage = 1;               
    public LayerMask enemyLayer;         

    [Header("Referencia")]
    public PlayerMovement player;        

    private void OnTriggerEnter(Collider other)
    {
        //--------------------------------------------------Aqui evalua si el player esta pegando o no
        if (player == null || !player.isPunching) return;

        
        if (((1 << other.gameObject.layer) & enemyLayer.value) == 0) return;

        //---------------------------------------------Aqui obtiene el el tag Enemy
        Enemigo enemy = other.GetComponent<Enemigo>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log("Player golpea al Enemy de manera cor");
        }
    }
}
