using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LivesBoard : MonoBehaviour
{
    public int lives = 3;
    public TextMeshProUGUI livesLabel;

    void Start()
    {
        livesLabel = GetComponent<TextMeshProUGUI>();
    }

    public void DecreaseLives()
    {
        livesLabel.text = "Vidas: " + --lives;

        if (lives <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene("Game Over Screen");
    }
}