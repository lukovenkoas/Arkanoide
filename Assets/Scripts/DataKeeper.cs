using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class DataKeeper : MonoBehaviour
{
    public static DataKeeper Instance;

    public string playerName;
    public Dictionary<string, int> scoresTable = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadScores();
    }

    private void LoadScores()
    {
        string path = Application.persistentDataPath + "/scores.txt";
        if (File.Exists(path))
        {
            var records = File.ReadAllLines(path);
            foreach (var record in records)
            {
                var data = record.Split('^');
                var name = data[0];
                var score = int.Parse(data[1]);
                scoresTable.Add(name, score);
            }
        }
    }

    public void SaveScore(int score)
    {
        // Update score if player already exist and get highter score or add new record
        if (scoresTable.ContainsKey(playerName))
        {
            if (scoresTable[playerName] < score)
                scoresTable[playerName] = score;
        }
        else
        {
            scoresTable.Add(playerName, score);
        }

        // Order records from highter to lowest score and keep only TOP 10 from them
        scoresTable = scoresTable.OrderByDescending(x => x.Value).Take(10).ToDictionary(x => x.Key, x => x.Value);

        // Pack records to strings
        var records = new List<string>();
        foreach (var pair in scoresTable)
            records.Add($"{pair.Key}^{pair.Value}");

        // Save packed records
        string path = Application.persistentDataPath + "/scores.txt";
        File.WriteAllLines(path, records);

        Debug.Log("Scores saved");
    }
}