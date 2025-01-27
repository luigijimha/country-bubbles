using UnityEngine;
using UnityEngine.SceneManagement;

public class LivesBoard : MonoBehaviour
{
    public int maxLives = 5;
    private int lives = 5;
    public SpriteRenderer livesIcon;

    private Vector3 originalScale;

    public float scaleSpeed = 2f;
    public float scaleAmount = 0.05f;

    void Start()
    {
        livesIcon = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (lives <= 2)
        {
            float scaleModifier = Mathf.Sin(Time.time * scaleSpeed) * scaleAmount;
            transform.localScale = originalScale + new Vector3(scaleModifier, scaleModifier, 0);
        }
        else
        {
            transform.localScale = originalScale;
        }
    }

    public void DecreaseLives()
    {
        --lives;

        double alpha = 1 - ((float)lives) / maxLives;
        livesIcon.color = new Color(1, (float)alpha, (float)alpha);

        if (lives <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene("Game Over Screen");
    }

    public void SetLives(int lives)
    {
        this.maxLives = lives;
        this.lives = lives;

        livesIcon.color = new Color(1, 0, 0);
    }
}
