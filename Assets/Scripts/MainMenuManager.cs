
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

   public void LoadTutorial()
    {
        SceneManager.LoadScene("LevelTutorial");
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
