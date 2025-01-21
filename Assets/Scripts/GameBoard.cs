using System;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    private Clock clock;
    private GameObject[] remainingCountries;
    public List<Sprite> countries;
    private Sprite[] selectedSprites;
    public GameObject countryPrefab;
    public float padding = 0.01f;
    public int initialRows = 4;
    public int initialColumns = 3;
    public int initialObjectives = 2;
    public int initialSprites = 2;

    private readonly float imageRatio = 2000;
    private float screenWidth;
    private float screenHeight;
    private float countryWidth;
    private float countryHeight;

    void Start()
    {
        screenWidth = transform.localScale.x;
        screenHeight = transform.localScale.y;
        clock = GameObject.FindWithTag("Clock").GetComponent<Clock>();
        InitializeBoard(initialRows, initialColumns, initialObjectives, initialSprites);
    }

    private void InitializeBoard(int rows, int columns, int objectives, int totalCountries)
{
    Debug.Log("creating board");
    remainingCountries = new GameObject[rows * columns];

    // Scale of countries
    countryWidth = (screenWidth - padding * (columns + 1)) / columns;
    countryHeight = (screenHeight - padding * (rows + 1)) / rows;
    float countryScale = (countryWidth < countryHeight ? countryWidth : countryHeight) / imageRatio;

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

    // Start instantiation coroutine
    StartCoroutine(InstantiateObjects(positions, countryScale, objectiveSprite, objectives));
}

private System.Collections.IEnumerator InstantiateObjects(
    List<(int, int)> positions,
    float countryScale,
    Sprite objectiveSprite,
    int objectives
)
{
    int countryCounter = 0;
    int objectiveCount = 0;

    foreach (var (row, column) in positions)
    {
        GameObject country = Instantiate(countryPrefab, transform);
        country.transform.position = CalculatePosition(row, column);
        country.transform.localScale = new Vector3(countryScale, countryScale, 0);

        bool isObj = objectiveCount < objectives && UnityEngine.Random.Range(0, 2) == 0;
        if (isObj) objectiveCount++;

        country.GetComponent<Country>().OnStart();
        country.GetComponent<Country>().SetIsObjective(isObj);
        country.GetComponent<SpriteRenderer>().sprite = isObj
            ? objectiveSprite
            : selectedSprites[UnityEngine.Random.Range(0, selectedSprites.Length)];

        remainingCountries[countryCounter++] = country;

        yield return new WaitForSeconds(0.05f);
    }

    clock.StartClock();
}

    private Vector3 CalculatePosition(int row, int column)
    {
        float x = countryWidth/2 + padding + column * (countryWidth + padding) - screenWidth/2;
        float y = countryHeight/2 + padding + row * (countryHeight + padding) - screenHeight/2;

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
        //todo
    }
}