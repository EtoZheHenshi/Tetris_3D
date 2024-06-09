using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject menu;
    [SerializeField] TMP_Text finalScoreText;
    [SerializeField] GameObject gameOverScreen;
    private int score;
    public int Score {  get { return score; } }
    void Start()
    {
        Time.timeScale = 0;
        score = 0;
    }

    void Update()
    {
        timeText.text = Time.timeSinceLevelLoad.ToString("F0");
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Pause();
        }
        if (Input.GetKeyUp(KeyCode.Return))
        {
            Play();
        }
    }

    public void UpdateScore(int score)
    {
        scoreText.text = (this.score += score).ToString();
    }

    public void Play()
    {
        menu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        menu.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        finalScoreText.text = $"Your score: {score}";
        gameOverScreen.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
