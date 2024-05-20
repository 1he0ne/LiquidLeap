using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanges : MonoBehaviour
{
    private PlayerHealth Health;
    public GameObject Image;
    public AudioSource AudioSource;
    public AudioClip PauseClip;
    public AudioClip ResumeClip;

    private bool isPaused = false;

    public PlayerShoot PlayerShoot;

    private void Start()
    {
        AudioSource.volume = 0.5f;
        var tempPlayer = GameObject.FindGameObjectsWithTag("Player")[0];
        Health = tempPlayer.GetComponent<PlayerHealth>();

        if ( Health == null )
        {
            Debug.LogError("No PlayerHealth Object assigned to SceneChanges.cs!");
        }
    }

    private void Update()
    {
        if ( Health == null || Health.Health <= 0 )
        {
            StartCoroutine(ReloadSceneAfterDeath());
        }
        // Reset immediately, when pressing the reset btn
        if ( Input.GetButtonDown("Reset") )
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if ( Input.GetKeyDown(KeyCode.Escape) )
        {
            if ( isPaused )
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }

        }

    }

    public IEnumerator ReloadSceneAfterDeath()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void PauseGame()
    {
        Cursor.visible = true;
        Image.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
        PlayerShoot.enabled = false;
        
        AudioSource.PlayOneShot(PauseClip);
    }

    void ResumeGame()
    {
        Cursor.visible = false;
        Image.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        PlayerShoot.enabled = true;

        AudioSource.PlayOneShot(ResumeClip);
    }
}
