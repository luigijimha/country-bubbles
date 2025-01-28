using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBoard : MonoBehaviour
{
    private Clock clock;
    private GameObject[] remainingCountries;
    public List<Sprite> countries;
    private Sprite[] selectedSprites;
    public GameObject countryPrefab;
    public float padding = 0.1f;
    private int remainingObjectives = 1;

    private readonly float imageRatio = 0.15f;
    private readonly float ScreenRatio = 8;
    private float screenWidth;
    private float screenHeight;
    private float countryWidth;
    private float countryHeight;

    private int level = 0;
    private SpriteRenderer targetImage;
    public AudioClip bubblesSound;
    private AudioSource audioSource;
    public int levelLimit = 20;

    void Start()
    {
        screenWidth = transform.localScale.x * ScreenRatio;
        screenHeight = transform.localScale.y * ScreenRatio;
        clock = GameObject.FindWithTag("Clock").GetComponent<Clock>();
        targetImage = GameObject.FindWithTag("Target").GetComponent<SpriteRenderer>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = bubblesSound;

        remainingCountries = new GameObject[0];

        IncreaseScore();
    }

    private void InitializeBoard(int rows, int columns, int objectives, int totalCountries, double timeLimit, int lives)
    {
        remainingCountries = new GameObject[rows * columns];

        // Scale of countries
        countryWidth = (screenWidth - 2 * padding) / columns;
        countryHeight = (screenHeight - 2 * padding - 0.5f) / rows;
        float countryScale = imageRatio * (countryWidth < countryHeight ? countryWidth : countryHeight);

        // List of positions
        List<(int, int)> positions = new();

        // Populate positions
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                positions.Add((i, j));
            }
        }

        // Shuffle sprites and positions
        List<Sprite> shuffledCountries = new(countries);
        ShuffleList(shuffledCountries);
        ShuffleList(positions);

        selectedSprites = shuffledCountries.GetRange(0, totalCountries - 1).ToArray();
        Sprite objectiveSprite = shuffledCountries[totalCountries - 1];

        int loopCount = rows*columns / 2;

        // Play a bubble sound loop
        StartCoroutine(PlayBubbleSoundLoop(loopCount));

        // Start instantiation coroutine
        StartCoroutine(InstantiateObjects(positions, countryScale, objectiveSprite, objectives, timeLimit));

        GameObject.FindWithTag("Lives").GetComponent<LivesBoard>().SetLives(lives);
        targetImage.sprite = objectiveSprite;
    }

    private IEnumerator PlayBubbleSoundLoop(int loopCount)
    {
        if (bubblesSound == null || audioSource == null)
        {
            yield break;
        }

        float clipDuration = bubblesSound.length/4;

        for (int i = 0; i < loopCount; i++)
        {
            audioSource.PlayOneShot(bubblesSound);
            yield return new WaitForSeconds(clipDuration);
        }
    }

    private IEnumerator InstantiateObjects(
        List<(int, int)> positions,
        float countryScale,
        Sprite objectiveSprite,
        int objectives,
        double timeLimit
    )
    {
        int countryCounter = 0;
        int objectiveCount = 0;

        foreach (var (row, column) in positions)
        {
            GameObject country = Instantiate(countryPrefab, transform);
            country.transform.position = CalculatePosition(row, column);
            country.transform.localScale = new Vector3(countryScale, countryScale, 0);

            bool isObj = objectiveCount++ < objectives;

            country.GetComponent<Country>().SetIsObjective(isObj);
            country.GetComponent<SpriteRenderer>().sprite = isObj
                ? objectiveSprite
                : selectedSprites[UnityEngine.Random.Range(0, selectedSprites.Length)];

            remainingCountries[countryCounter++] = country;

            yield return new WaitForSeconds(0.05f);
        }

        clock.StartClock(timeLimit);
        Country.StartGame();
    }

    private Vector3 CalculatePosition(int row, int column)
    {
        float x = padding + column * (countryWidth + padding) - screenWidth/2 + countryWidth/2;
        float y = padding - row * (countryHeight + padding) + 0.8f;

        return new Vector3(x, y, 0);
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
        }
    }

    public void IncreaseScore()
    {

        if (--remainingObjectives <= 0)
        {
            clock.StopClock();
            
            if(level++ > levelLimit) {
                SceneManager.LoadScene("Win Screen");
            }

            // Calculate the next starting board values based on the level
            int columns = Mathf.FloorToInt((float)(2.5 * Math.Tanh((level - 6.0) / 4.0) + 6.0));
            int rows = Mathf.FloorToInt((float)(Math.Tanh((level - 8.0) / 4.0) + 5.5));

            // Calculate the next starting objectives
            remainingObjectives = Mathf.FloorToInt((float)(4.0 * Math.Tanh((level - 10.0) / 5.0) + 7.0));

            // Calculate the next starting time
            float timeLimit = Mathf.FloorToInt((float)(40.0 / (level + 2.0) + 14.0));

            // Calculate the next starting lives
            int lives = Mathf.FloorToInt((float)(15.0 / (level + 5.0) + 2.0));

            // Calculate next total countries
            int maxCountries = level + 1 > countries.Count ? countries.Count : level + 1;

            Country.StopGame();

            // Reset board and advance to the next level
            StartCoroutine(DestroyRemainingCountries());

            // Reinitialize the board
            InitializeBoard(rows, columns, remainingObjectives, maxCountries, timeLimit, lives);
        }
    }

    private IEnumerator DestroyRemainingCountries()
    {
        foreach (GameObject country in remainingCountries)
        {
            if (country == null)
                continue;

            Destroy(country);
            yield return new WaitForSeconds(0.05f);
        }
    }
}