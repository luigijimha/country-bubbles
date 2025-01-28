using UnityEngine;

public class ShowModal : MonoBehaviour
{
    public GameObject modal;

    private void OnMouseDown() 
    {
        if (modal != null)
        {
            modal.SetActive(true);
            DisableButtons();
        }
    }

    private void DisableButtons()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach (GameObject button in buttons)
        {
            BoxCollider2D collider = button.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }
    }
}