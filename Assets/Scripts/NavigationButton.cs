using UnityEngine.SceneManagement;
using UnityEngine;

public class NavigationButton : MonoBehaviour
{
    public string scene;

    private void OnMouseDown() {
        Navigate();
    }
    
    private void Navigate(){
        SceneManager.LoadScene(scene);
    }
}
