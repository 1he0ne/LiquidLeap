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
        if( Health.Health <= 0 ) 
        {
            StartCoroutine(ReloadSceneAfterDeath());
        }
    }

    public IEnumerator ReloadSceneAfterDeath()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
