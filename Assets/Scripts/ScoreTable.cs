using TMPro;
using UnityEngine;
using System.Linq;

public class ScoreTable : MonoBehaviour
{
    public GameObject[] textFields;

    void Start()
    {
        var scores = DataKeeper.Instance.scoresTable.ToArray();
        for (var i = 0; i < scores.Length; i++)
            textFields[i].GetComponent<TextMeshProUGUI>().text = scores[i].Key + " - " + scores[i].Value;
    }
}