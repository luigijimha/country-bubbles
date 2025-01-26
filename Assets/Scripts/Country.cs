using System;
using UnityEngine;

public class Country : MonoBehaviour
{
    private bool isObjective;
    private GameBoard scoreBoard;
    private LivesBoard livesBoard;
    private static bool gameRunning = false;

    public void OnStart()
    {
        scoreBoard = GameObject.FindWithTag("Board").GetComponent<GameBoard>();
        livesBoard = GameObject.FindWithTag("Lives").GetComponent<LivesBoard>();
    }

    public Country(bool isObjective, Sprite sprite)
    {
        this.isObjective = isObjective;
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    private void OnMouseDown() {
        SelectCountry();
    }

    /*private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject == gameObject)
                    {
                        SelectCountry();
                    }
                }
            }
        }
    }*/

    private void SelectCountry()
    {
        if(gameRunning) {
            if (isObjective)
            {
                scoreBoard.IncreaseScore();
            }
            else
            {
                livesBoard.DecreaseLives();
            }

            Destroy(gameObject, 0.5f);
        }
    }

    public void SetIsObjective(bool isObjective) {
        this.isObjective = isObjective;
    }
}
