using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clock : MonoBehaviour
{
    private double startTimer = 0.0;
    private double currentTimer = 0.0;
    public double timeLimit = 75.0;
    private SpriteRenderer clock;
    private bool clockRunning = false;

    void Start()
    {
        clock = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (clockRunning)
        {
            currentTimer = Time.time - startTimer;

            double green = currentTimer > timeLimit / 2 ? 2 - 2 * currentTimer / timeLimit : 1;
            double red = currentTimer > timeLimit / 2 ? 1 : 2 * currentTimer / timeLimit;
            clock.color = new Color((float)red, (float)green, 0);

            if (currentTimer / timeLimit >= 0.8)
            {
                float rotation = Mathf.Floor((float)currentTimer) % 2 == 0 ? 15f : -15f;
                transform.rotation = Quaternion.Euler(0, 0, rotation);
            }

            if (currentTimer >= timeLimit)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        clockRunning = false;
        SceneManager.LoadScene("Game Over Screen");
    }

    public void StartClock(double newTimeLimit)
    {
        clockRunning = true;
        startTimer = Time.time;
        currentTimer = 0.0;
        timeLimit = newTimeLimit;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void StopClock()
    {
        clockRunning = false;
    }
}
