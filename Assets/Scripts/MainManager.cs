using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    /// <summary>
    /// main manager handles everything that goes on with the gameplay
    /// </summary>
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public int BestScoreText;
    public GameObject GameOverText;
    public Text playerNameText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;
    public GameObject canvas; //reference to our canvas gameobject

    public static MainManager instance;//accessible everywhere in the game and persistent


    //private void Awake()
    //{
    //    if(instance != null)
    //    {
    //        Destroy(gameObject);
    //    }
    //    instance = this;
    //    DontDestroyOnLoad(gameObject);
    //}
    // Start is called before the first frame update
    void Start()
    {
       
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
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


        BestScored();
        playerNameText = GetComponent<Text>();
        //if(m_Points > PlayerPrefs.GetInt("Best Score", 0))
        //{
        //   int BestScored = PlayerPrefs.SetInt("Best Score", m_Points);//persistant score
        //    BestScore.text = m_Points.ToString();
        //}
        
       // playerName.text = PlayerPrefs.GetString("Player Name").ToString();//persistent name

    }

    private void Update()
    {
        if (!m_Started)//game not started
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
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";


        //high score
        //PlayerPrefs.SetInt("Best Score", 0);
    }
    //start new game
    public void StartNew()
    {
        SceneManager.LoadScene(0); //load scene at index 1
    }
    
    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    //serialized data
    [System.Serializable]

    class SaveData 
    {
        public string playerName;
        public int BestScore;

        MenuHandler handler = new MenuHandler();
    }
    //save best score
    public void BestScored()
    {
       SaveData bScore = new SaveData();
        bScore.BestScore = m_Points;

        //JSON
        string json = JsonUtility.ToJson(bScore);
        File.WriteAllText(Application.persistentDataPath + "/saveBestScore.json", json);
        //if (m_Points > PlayerPrefs.GetInt("Best Score", 0))
        //{
        //      PlayerPrefs.SetInt("Best Score", m_Points);//persistant score
        //    BestScore.text = m_Points.ToString();
        //}
    }
    public void PlayerName()
    {
        SaveData saveName = new SaveData();
        saveName.playerName = MenuHandler.userName.ToString(); //set the plaey name

        //JSON
        string json = JsonUtility.ToJson(saveName);
        File.WriteAllText(Application.persistentDataPath + "/savePlayerName", json);

    }

    public void LoadData()
    {
        string ScorePath = Application.persistentDataPath + "/saveBestScore.json";
        if (File.Exists(ScorePath))
        {
            string json = File.ReadAllText(ScorePath);
            SaveData data = JsonUtility.FromJson < SaveData > (json);

            BestScoreText = data.BestScore; //assign the returned value to best score
        }

        string NamePath = Application.persistentDataPath + "/savePlayerName.json";
        if (File.Exists(NamePath))
        {
            string json = File.ReadAllText(NamePath); //read the name path if exist
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            //assign the returned values
            playerNameText.text = saveData.playerName.ToString();
            playerNameText.text = $"Name: {playerNameText}";
        }

    }
    //void Name()
    //{
    //    SaveData playerName = new SaveData();

    //    foreach(char letter in Input.inputString)
    //    {
    //        if(playerName != 0)
    //        playerName.Text = playerName.text.Substring(0, playerName.le)
    //    }
        
    //}
}
