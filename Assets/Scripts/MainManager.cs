using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static StartScreenManager;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText, bestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    private int bestScore;
    private string bestScorePlayer;
    
    private bool m_GameOver = false;
    private string playName;

    
    // Start is called before the first frame update
    void Start()
    {
        
        bestScore = DataManager.instance.bestScore;
        bestScorePlayer = DataManager.instance.bestScorePlayer;
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        playName = DataManager.instance.PlayerName;

        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        ScoreText.text = $"{playName}'s Score \n{m_Points}";
        bestScoreText.text = $"Best Score\n{bestScorePlayer} - {bestScore}";
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"{playName}'s Score \n{m_Points}";
    }

    public void saveBestScore()
    {
        ScoreData data = new ScoreData();
        data.Name = DataManager.instance.bestScorePlayer;
        data.Score = DataManager.instance.bestScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void GameOver()
    {
        
        if (m_Points > bestScore)
        {
            
            bestScore = m_Points;
            bestScorePlayer = playName;
            bestScoreText.text = $"Best Score\n{bestScorePlayer} - {bestScore}";
            DataManager.instance.bestScore = m_Points;
            DataManager.instance.bestScorePlayer = playName;
            saveBestScore();
            
        }
        m_GameOver = true;
        GameOverText.SetActive(true);

    }
}
