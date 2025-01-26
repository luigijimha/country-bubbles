using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Clock : MonoBehaviour
{
    private double startTimer;
    private double currentTimer;
    public double timeLimit;
    public TextMeshProUGUI clockLabel;
    private bool clockRunning = false;

    void Start()
    {
        clockLabel = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if(clockRunning) {
            currentTimer = Time.time - startTimer;
            clockLabel.text = Mathf.CeilToInt((float)(timeLimit - currentTimer)).ToString();

            double red = currentTimer / timeLimit;
            double green = (timeLimit - currentTimer) / timeLimit;
            clockLabel.color = new Color((float) red, (float) green, 0);

            if (currentTimer >= timeLimit)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        clockRunning = false;
        SceneManager.LoadScene("GameOver");
    }

    public void StartClock() {
        clockRunning = true;
        startTimer = Time.time;
    }

    public void StopClock() {
        clockRunning = false;
    }

    public void SetTimeLimit(double timeLimit) {
        this.timeLimit = timeLimit;
    }
}