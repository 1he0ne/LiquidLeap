
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip PlayGameSound;
    public AudioClip EndGameSound;
    public TextMeshProUGUI ProgressText;

    private void Start()
    {
        Cursor.visible = true;

        ProgressText.text = string.Format("Progress: {0}%\n\n"
            +"Tutorial completed: {1}\n"
            +"Tutorial ruby found: {2}\n\n"
            +"L1 completed: {3}\n"
            +"L1 ruby found: {4}\n\n"
            +"L2 completed: {5}\n"
            +"L2 first ruby found: {6}\n"
            +"L2 second ruby found: {7}\n", MeasureProgress(), PersistentStorage.LevelFinished[0], PersistentStorage.RubiesFound[0],
            PersistentStorage.LevelFinished[1], PersistentStorage.RubiesFound[1],
            PersistentStorage.LevelFinished[2], PersistentStorage.RubiesFound[2], PersistentStorage.RubiesFound[3]);

    }

    private int MeasureProgress()
    {
        float rubyProgress = 100.0f * PersistentStorage.NumRubiesFound() / PersistentStorage.MaxRubies();
        float levelProgress = 100.0f * PersistentStorage.NumLevelsDone() / PersistentStorage.MaxLevels();
        return (int)(rubyProgress + levelProgress) / 2;
    }
    public void LoadTutorial()
    {
        StartCoroutine(PlayGame());
        Debug.Log("Tutorial Loaded");
    }
    public void LoadGame()
    {
        StartCoroutine (PlayGameSoundDelayed());
        Debug.Log("Game Loaded");
    }

    public void OnApplicationQuit()
    {
        StartCoroutine(PlayGameQuitGameSound());
        Application.Quit();
        Debug.Log("Game Quit");
    }

    IEnumerator PlayGameSoundDelayed()
    {
        PersistentStorage.NumPlayerLives = 4;
        AudioSource.PlayOneShot(PlayGameSound, 0.5f);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Level1");
    }
    IEnumerator PlayGameQuitGameSound()
    {
        AudioSource.PlayOneShot(EndGameSound, 0.5f);
        yield return new WaitForSeconds(1);
    }
    IEnumerator PlayGame()
    {
        PersistentStorage.NumPlayerLives = 4;
        AudioSource.PlayOneShot(PlayGameSound, 0.5f);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("LevelTutorial");
    }

}
