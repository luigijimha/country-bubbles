using UnityEngine;

public class Country : MonoBehaviour
{
    private bool isObjective;
    private GameBoard scoreBoard;
    private LivesBoard livesBoard;
    public static bool gameRunning = false;

    public AudioClip popSound;
    private AudioSource audioSource;

    public void Start()
    {
        scoreBoard = GameObject.FindWithTag("Board").GetComponent<GameBoard>();
        livesBoard = GameObject.FindWithTag("Lives").GetComponent<LivesBoard>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = popSound;
    }

    public Country(bool isObjective, Sprite sprite)
    {
        this.isObjective = isObjective;
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    private void OnMouseDown() {
        SelectCountry();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider != null && hit.collider.gameObject == gameObject)
                    {
                        SelectCountry();
                    }
                }
            }
        }
    }

    public static void StartGame() {
        gameRunning = true;
    }

    public static void StopGame() {
        gameRunning = false;
    }

    private void SelectCountry()
    {
        if (gameRunning)
        {
            if (audioSource != null && popSound != null)
            {
                audioSource.Play();
            }

            if (isObjective)
            {
                scoreBoard.IncreaseScore();
            }
            else
            {
                livesBoard.DecreaseLives();
            }

            Destroy(gameObject, popSound != null ? popSound.length : 0);
        }
    }

    public void SetIsObjective(bool isObjective) {
        this.isObjective = isObjective;
    }
}
