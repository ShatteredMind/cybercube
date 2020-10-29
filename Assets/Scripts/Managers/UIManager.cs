using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager manager;

    [SerializeField] private GameObject countdownText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text cakeCountText;
    [SerializeField] private Text scoreTextMenu;
    [SerializeField] private GameObject headshotText;
    [SerializeField] private GameObject levelMenu;
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject confettiWindow;
    [SerializeField] private GameObject borderLine;

    private Dictionary<string, GameObject> menus;

    void Awake()
    {
        if (manager != null)
            Destroy(manager);
        else
            manager = this;

        menus = new Dictionary<string, GameObject>
        {
            ["pause"] = pauseMenu,
            ["lose"] =  loseMenu,
            ["level"] = levelMenu,
            ["confetti"] = confettiWindow,
            ["pausebutton"] = pauseButton,
            ["timer"] = countdownText,
            ["border"] = borderLine,
        };
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        pauseButton.SetActive(!pauseButton.activeSelf);
    }
    
    public void SetActiveMenu(string menuName, bool state)
    {
        menus[menuName].SetActive(state);
    }

    public void DisplayHeadshotText()
    {

    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    public void UpdateCakeCount()
    {
        cakeCountText.text = $"X{GameManager.manager.CakeCount}";
    }

    public void UpdatePlayerScore(int score)
    {
        scoreTextMenu.text = string.Format("SCORE/.{0}", score);
    }

    public void UpdateTimerText(float currentTime)
    {
        countdownText.GetComponent<Text>().text = currentTime.ToString("0");
    }

    private void AddScores(int score)
    {
        int playerScore = PlayerPrefs.GetInt("PlayerScore", 0);
        playerScore += score;

        PlayerPrefs.SetInt("PlayerScore", playerScore);
        scoreText.text = string.Format("Score: {0}", playerScore);
    }

    private void SetPlayerPrefs()
    {
        if (SceneManager.GetActiveScene().name.Contains("Level"))
        {
            PlayerPrefs.SetInt("ActiveLevel", SceneManager.GetActiveScene().buildIndex + 1);
        }

        int playerScore = PlayerPrefs.GetInt("PlayerScore", 0);
        scoreText.text = string.Format("Score: {0}", playerScore);
    }

}
