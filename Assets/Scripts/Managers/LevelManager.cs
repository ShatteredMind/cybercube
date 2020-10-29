using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public TextMeshProUGUI levelText;

    [SerializeField] private GameObject[] levelButtons;
    [SerializeField] private GameObject levelMenu;

    public void PlayGame(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadLevels()
    {
        int lastLevel = PlayerPrefs.GetInt("ActiveLevel", 1);
        for (int i = lastLevel; i < levelButtons.Length; i++)
        {
            levelButtons[i].SetActive(false);
        }
        levelMenu.SetActive(true);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
