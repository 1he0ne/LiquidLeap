
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip PlayGameSound;
    public AudioClip EndGameSound;

    private void Start()
    {
        Cursor.visible = true;
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
