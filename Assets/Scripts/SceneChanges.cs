using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanges : MonoBehaviour
{
    private PlayerHealth Health;

    private void Start()
    {
        var tempPlayer = GameObject.FindGameObjectsWithTag("Player")[0];
        Health = tempPlayer.GetComponent<PlayerHealth>();

        if ( Health == null )
        {
            Debug.LogError("No PlayerHealth Object assigned to SceneChanges.cs!");
        }
    }

    private void Update()
    {
        if( Health == null || Health.Health <= 0 ) 
        {
            StartCoroutine(ReloadSceneAfterDeath());
        }
        // Reset immediately, when pressing the reset btn
        if ( Input.GetButtonDown("Reset") )
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public IEnumerator ReloadSceneAfterDeath()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
