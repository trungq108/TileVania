using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    int playerLives = 3;
    int score = 0;
    [SerializeField] TextMeshProUGUI liveText;
    [SerializeField] TextMeshProUGUI scoreText;
    private void Awake()
    {
        int numberGameSession = FindObjectsOfType<GameSession>().Length;
        if(numberGameSession > 1)
        {
            Destroy(gameObject);
        }
        else { DontDestroyOnLoad(gameObject); }
    }

    private void Start()
    {
        liveText.text = "LIVE: " + playerLives.ToString();
        scoreText.text = "SCORE: " + score.ToString();
    }
    public void PlayerDeathSequence()
    {
        playerLives--;
        liveText.text = "LIVE: " + playerLives.ToString();
        if (playerLives > 0)
        {
            LoadLevel();
        }
        else
        {
            LoadGame();
        }
    }

    void LoadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadGame()
    {
        SceneManager.LoadScene(0);
        FindObjectOfType<ScenePersist>().DestroyScenePresist();
        Destroy(gameObject);

    }

    public void AddScore(int point)
    {
        score += point;
        scoreText.text = "SCORE: " + score.ToString();
    }


}
