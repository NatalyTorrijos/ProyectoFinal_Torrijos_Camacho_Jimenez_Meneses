using UnityEngine;

public class PlayerFallReset : MonoBehaviour
{
    public Transform[] checkpoints;
    private int currentCheckpoint = 0;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("SueloNivel3"))
        {
            ResetToCheckpoint();
        }
    }

    public void SetCheckpoint(int index)
    {
        currentCheckpoint = index;
    }

    private void ResetToCheckpoint()
    {
        controller.enabled = false;
        transform.position = checkpoints[currentCheckpoint].position;
        controller.enabled = true;
    }
}
