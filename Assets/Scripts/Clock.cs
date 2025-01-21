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
        currentTimer = Time.time - startTimer;
        clockLabel.text = "Time: " + Mathf.CeilToInt((float)(timeLimit - currentTimer)).ToString();

        if (currentTimer >= timeLimit)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void StartClock() {
        clockRunning = true;
        startTimer = Time.time;
    }

    public void StopClock() {
        clockRunning = false;
    }
}