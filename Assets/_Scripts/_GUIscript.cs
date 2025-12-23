using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Data.Common;


public class _GUIscript : MonoBehaviour
{

    public int score; // Player's score
    [SerializeField] float time; // Time in seconds

    public GameObject gameOverPanel, scoreText, timerText, pausePanel; // UI Elements
    public bool isPaused = false;
    bool GameOver = false;
    public static _GUIscript Instance
    {
        get; private set;
    }

    void Start()
    {
        GameOver = false;
        // timerText = gameObject.GetComponent<TextMeshPro>().gameObject;
        gameOverPanel.SetActive(false);
        score = 0;
        time = 0;
        Instance = this;
    }
    void Update()
    {
        UpdateTime();
        UpdateScore();
    }

    public void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
        }
    }
    public void ShowGameOverPanel(GameObject gameOverPanel)
    {
        gameOverPanel.SetActive(true);
    }
    public void UpdateTime()
    {
        time += Time.deltaTime;
        if (timerText != null)
        {
            var textComp = timerText.GetComponent<TMP_Text>();
            if (textComp != null)
            {
                // Format time as hours:minutes:seconds
                int totalSeconds = Mathf.FloorToInt(time);

                int hours = totalSeconds / 3600;
                int minutes = (totalSeconds % 3600) / 60;
                int seconds = totalSeconds % 60;
                if (hours > 0)
                {
                    textComp.text = string.Format("Time:\n{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
                }
                else if (minutes > 0)
                {
                    textComp.text = string.Format("Time:\n{0:00}:{1:00}", minutes, seconds);
                }
                else
                {
                    textComp.text = string.Format("Time:\n{0:00}", seconds);
                }
                // textComp.text = string.Format("Time:\n{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
            }
        }
    }
    public void gameOver()
    {
        GameOver = true;
        AudioManager.Instance.PlaySFX("GameOver");
        Time.timeScale = 0;
        isPaused = true;
        ShowGameOverPanel(gameOverPanel);
        if (HighScoreManager.Instance != null)
        {
            HighScoreManager.Instance.PrepareForSubmit(score, time);
        }
    }
    public void resumeGame()
    {
        if (isPaused && !GameOver || (Input.GetKey(KeyCode.Escape) && !isPaused && !GameOver))
        {
            isPaused = false;
            Time.timeScale = 1;
            pausePanel.SetActive(false);
        }
    }
    public void pauseGame()
    {
        if (!isPaused && !GameOver || (Input.GetKey(KeyCode.Escape) && !isPaused && !GameOver))
        {
            isPaused = true;
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
    }
}
