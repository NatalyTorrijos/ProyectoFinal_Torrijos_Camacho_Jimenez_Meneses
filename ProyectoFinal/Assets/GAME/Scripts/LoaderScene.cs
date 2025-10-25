using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderScene : MonoBehaviour
{
  
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void LoaderScenes(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }
}
