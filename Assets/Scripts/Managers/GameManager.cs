using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    public List<Dummy> deadTargets;

    public GameObject[] cakes;

    [SerializeField] private Dummy[] targets;
    [SerializeField] private GameObject wall;
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private StressReceiver receiver;
    [SerializeField] private int playerScore;
    [SerializeField] private float startingTime = 30f;
    [SerializeField] private float fallingTime = 0f;

    private MyShaderBehavior wallShader;
    private bool levelIsFinished = false;
    private bool timeIsSlowed = false;
    private float headshotTimer = 0.05f;
    private float slowMotionDelay = 0.3f;
    private float currentTime = 0f;

    public int CakeCount { get; set; } = 3;
    public bool GameIsFinished { get; private set; } = false;
    public bool LevelIsPaused { get; private set; } = false;
    private bool SlowMotion { get; set; } = false;

    void Awake()
    {
        if (manager != null)
            GameObject.Destroy(manager);
        else
            manager = this;

        ResetLevelScore();
        cakes = new GameObject[1];
        currentTime = startingTime;
        deadTargets = new List<Dummy>();
        wallShader = wall.GetComponent<MyShaderBehavior>();
    }

    public void UpdatePlayerScore(int score)
    {
        string levelScore = $"Level{SceneManager.GetActiveScene().buildIndex}Score";
        int currentScore = PlayerPrefs.GetInt(levelScore, 0);
        currentScore += score;
        PlayerPrefs.SetInt(levelScore, currentScore);
        UIManager.manager.UpdateScoreText(currentScore);
    }

    public void EnableSlowMotion()
    {
        SlowMotion = true;
    }
    public void ChangeWallParticleSystem(ParticleSystem partSystem)
    {
        wallShader.ChangeParticleSystem(partSystem);
    }

    # region Public Level Funtions
    public void EndGame()
    {
        UIManager.manager.SetActiveMenu("lose", true);
        UIManager.manager.SetActiveMenu("timer", false);
        UIManager.manager.SetActiveMenu("pausebutton", false);
        UIManager.manager.SetActiveMenu("border", false);
        GameIsFinished = true;
    }

    public void LoadLevel(int next = 0)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + next);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        LevelIsPaused = !LevelIsPaused;
        UIManager.manager.TogglePauseMenu();
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        LevelIsPaused = !LevelIsPaused;
        UIManager.manager.TogglePauseMenu();
    }

    # endregion

    public void PlayExplosion(Vector3 bombPosition)
    {
        ps.transform.position = bombPosition;
        receiver.InduceStress(10f);
        ps.Play();
    }
    
    private void Update()
    {
        if (!levelIsFinished)
        {
            if (PlayerHasWon() && !GameIsFinished)
            {
                UIManager.manager.SetActiveMenu("level", true);
                UIManager.manager.SetActiveMenu("timer", false);
                UIManager.manager.SetActiveMenu("pausebutton", false);
                UIManager.manager.SetActiveMenu("border", false);
                // UIManager.manager.SetActiveMenu("confetti", true);
                GameIsFinished = true;
                levelIsFinished = true;
                AddScores();
            }
        }

        if (!GameIsFinished)
        {
/*            currentTime -= 1 * Time.deltaTime;
            UIManager.manager.UpdateTimerText(currentTime);
            if (currentTime <= 0)
            {
                EndGame();
            }*/
        }

        if (CakeCount <= 0 && cakes[0] == null && !GameIsFinished)
        {
            fallingTime += Time.deltaTime;
            if (fallingTime >= 5.0f)
            {
                EndGame();
            }
        }

        if (SlowMotion)
        {
            ManagerSlowMotion();
        }
    }

    private bool PlayerHasWon()
    {
        return deadTargets.Count == targets.Length;
    }

    private int GetRoundScore()
    {
        int timeIsSlowedScore = SceneManager.GetActiveScene().buildIndex * 100;
        return timeIsSlowedScore;
    }

    private int GetActiveTargetsCount()
    {
        return targets.Where(t => t != null).Count();
    }

    private void ResetLevelScore()
    {
        string levelScore = $"Level{SceneManager.GetActiveScene().buildIndex}Score";
        PlayerPrefs.SetInt(levelScore, 0);
    }

    private void AddScores()
    {
        string levelScore = $"Level{SceneManager.GetActiveScene().buildIndex}Score";
        int currentScore = PlayerPrefs.GetInt(levelScore, 0);
        // int playerScore = PlayerPrefs.GetInt("PlayerScore", 0);
        // playerScore += currentScore;
        // PlayerPrefs.SetInt("PlayerScore", playerScore);
        UIManager.manager.UpdatePlayerScore(currentScore);
    }

    private void ManagerSlowMotion()
    {
        slowMotionDelay -= Time.deltaTime;
        if (slowMotionDelay < 0)
        {
            if (!timeIsSlowed)
            {
                Time.timeScale /= 7;
                timeIsSlowed = true;
            }

            headshotTimer -= Time.deltaTime;
            if (headshotTimer < 0)
            {
                Time.timeScale *= 7;
                headshotTimer = 0.1f;
                slowMotionDelay = 0.1f;
                SlowMotion = false;
                timeIsSlowed = false;
            }
        }
    }   
}
