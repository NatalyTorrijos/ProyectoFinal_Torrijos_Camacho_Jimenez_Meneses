using UnityEngine;

public class OpenPanelOnClick : MonoBehaviour
{
    public GameObject panelMecanicas;
    Camera cam;

    void Start()
    {
        cam = Camera.main;
        panelMecanicas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    panelMecanicas.SetActive(true);
                }
            }
        }
    }
}
