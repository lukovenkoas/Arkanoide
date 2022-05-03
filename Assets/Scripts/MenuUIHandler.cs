using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUIHandler : MonoBehaviour
{
    public GameObject PlayerNameField;

    public void StartNew()
    {
        var name = PlayerNameField.GetComponent<TMPro.TMP_InputField>().text;
        if (name == "") name = "Player";
        DataKeeper.Instance.playerName = name;
        Debug.Log(name + " start a game");
        SceneManager.LoadScene(1);
    }

    public void ShowScores()
    {
        SceneManager.LoadScene(2);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}