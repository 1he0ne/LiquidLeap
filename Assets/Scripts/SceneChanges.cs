using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanges : MonoBehaviour
{
    public PlayerHealth Health;

    private void Update()
    {
        if (Health == null )
        {
            StartCoroutine(ReloadSceneAfterDeath());
        }
    }


    public IEnumerator ReloadSceneAfterDeath()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Level1");
    }
}
