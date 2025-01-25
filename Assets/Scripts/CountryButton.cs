using System;
using UnityEngine;

public class CountryButton : MonoBehaviour
{
    private bool isObjective;
    private GameBoard scoreBoard;
    private LivesBoard livesBoard;
    private Animator animator;

    public void OnStart()
    {
        scoreBoard = GameObject.FindWithTag("Board").GetComponent<GameBoard>();
        livesBoard = GameObject.FindWithTag("Lives").GetComponent<LivesBoard>();
        animator = GetComponent<Animator>();
    }

    public CountryButton(bool isObjective, Sprite sprite)
    {
        this.isObjective = isObjective;
        GetComponent<SpriteRenderer>().sprite = sprite;
    }


    private void OnMouseDown()
    {
        SelectCountry();
    }

    private void SelectCountry()
    {
        if (isObjective)
        {
            scoreBoard.IncreaseScore();
        }
        else
        {
            livesBoard.DecreaseLives();
        }

        //todo: animation
        //animator.SetTrigger("Pop");
        Destroy(gameObject, 0.5f);
    }

    public void SetIsObjective(bool isObjective) {
        this.isObjective = isObjective;
    }
}