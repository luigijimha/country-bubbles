using UnityEngine;

public class HideModal : MonoBehaviour
{
    private void OnMouseDown() 
    {
        gameObject.SetActive(false);
        EnableButtons();
    }

    private void EnableButtons()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach (GameObject button in buttons)
        {
            BoxCollider2D collider = button.GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                collider.enabled = true;
            }
        }
    }
}
