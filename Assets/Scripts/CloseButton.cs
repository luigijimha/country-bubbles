using UnityEngine;

public class CloseButton : MonoBehaviour
{
    private void OnMouseDown() {
        CloseApp();
    }
    
    private void CloseApp(){
        Application.Quit();
    }
}
