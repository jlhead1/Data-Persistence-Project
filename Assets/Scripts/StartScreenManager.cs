#if UNITY_EDITOR

using System.IO;
using TMPro;
using UnityEditor;
#endif

using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;


public class StartScreenManager : MonoBehaviour

{

    public TextMeshProUGUI highScoreFieldText;
    // Start is called before the first frame update
    void Start()
    {
        loadBestScore();
        PlayerNameFieldChanged("New Player");   
        highScoreFieldText.text = $"High Score\n{DataManager.instance.bestScorePlayer} - {DataManager.instance.bestScore}";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void EndGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit()
        #endif
    }

    public void PlayerNameFieldChanged(string newName)
    {
        DataManager.instance.PlayerName = newName;
    }

    public class ScoreData
    {
        public string Name;
        public int Score;
    }

    public void saveBestScore()
    {
        ScoreData data = new ScoreData();
        data.Name = DataManager.instance.bestScorePlayer;
        data.Score = DataManager.instance.bestScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void loadBestScore()
    {
        string dataFilePath = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(dataFilePath)) 
        {
            string json = File.ReadAllText(dataFilePath);
            ScoreData data = JsonUtility.FromJson<ScoreData>(json);
            
            DataManager.instance.bestScore = data.Score;
            DataManager.instance.bestScorePlayer = data.Name;          

        }
        else
        {
            DataManager.instance.bestScore = 0;
            DataManager.instance.bestScorePlayer = "ABCDE";
        }
    }

}
