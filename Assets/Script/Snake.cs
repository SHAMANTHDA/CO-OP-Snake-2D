using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;

    private List<Transform> _segments = new List<Transform>();

    public Transform segmentPrefab;

    public GameObject GameOverPanel;

    private bool isgameover = false;

    public Snake snake;

    public Text scoretext;
    private int score;

    public GameObject playbutton;

    public GameObject Walls;

    public int initialsize = 4;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Pause();
    }
    private void Start()
    {
        ResetState();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) )
        {
             _direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            _direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _direction = Vector2.right;
        }
    }
    private void FixedUpdate()
    {
        if (isgameover == true) //this function is used to stop player after gameover
        {
            return;
        }

        for (int i = _segments.Count - 1; i > 0;  i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

            this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y,
            0.0f
        );
    }
    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;

        _segments.Add(segment);
    }
    private void ResetState()
    {
        for(int i = 1;i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }

        _segments.Clear();
        _segments.Add(this.transform);

        for(int i = 1; i < this.initialsize; i++)
        {
            _segments.Add(Instantiate(this.segmentPrefab));
        }

        this.transform.position = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            Grow();
            IncreaseScore();
        }
        else if(other.tag == "Obstacle")
        {
            ResetState();
        }

        if(other.tag == "Obstacle")
        {
            GameOverPanel.SetActive(true);
            isgameover = true;
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // (or) SceneManager.LoadScene(0);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        snake.enabled = false;
    }
    public void PlayGame()
    {
        score = 0;
        scoretext.text = score.ToString();

        playbutton.SetActive(false);
        Time.timeScale = 1f;
        snake.enabled = true;
    }

    public void IncreaseScore()
    {
        score++;
        scoretext.text = score.ToString();
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false; // this code of line completely quits the game
        Application.Quit();
    }
}
