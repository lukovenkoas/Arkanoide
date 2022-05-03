using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private bool m_GameOver = false;
    private int m_Points;

    private string playerName = "Player";
    private string bestName = "Unknown";
    private int bestScore = 0;

    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
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

        playerName = DataKeeper.Instance.playerName;
        GetScoreTableTop();
        UpdateBestScore();
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
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score [ {playerName} : {m_Points} ]";
        UpdateBestScore();
    }

    private void GetScoreTableTop()
    {
        if (DataKeeper.Instance.scoresTable.Count > 0)
        {
            var BestPlayer = DataKeeper.Instance.scoresTable.OrderByDescending(x => x.Value).First();
            bestName = BestPlayer.Key;
            bestScore = BestPlayer.Value;
        }
    }

    private void UpdateBestScore()
    {
        if (m_Points > bestScore)
            BestScoreText.text = $"Best Score - {playerName} : {m_Points}";
        else
            BestScoreText.text = $"Best Score - {bestName} : {bestScore}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        DataKeeper.Instance.SaveScore(m_Points);
    }
}