
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip PlayGameSound;
    public AudioClip EndGameSound;

   public void LoadTutorial()
    {
        StartCoroutine(PlayGame());
    }
    public void LoadGame()
    {
        StartCoroutine (PlayGameSoundDelayed());
    }

    public void OnApplicationQuit()
    {
        StartCoroutine(PlayGameQuitGameSound());
        Application.Quit();
        Debug.Log("Game Quit");
    }

    IEnumerator PlayGameSoundDelayed()
    {
        AudioSource.PlayOneShot(PlayGameSound);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Level1");
    }
    IEnumerator PlayGameQuitGameSound()
    {
        AudioSource.PlayOneShot(EndGameSound);
        yield return new WaitForSeconds(1);
    }
    IEnumerator PlayGame()
    {
        AudioSource.PlayOneShot(PlayGameSound);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("LevelTutorial");
    }

}
