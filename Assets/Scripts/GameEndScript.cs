using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndScript : MonoBehaviour
{
    [SerializeField] private string SceneToLoad;

    private AudioClip NextLevelSFX;
    private AudioSource Source;

    private void Start()
    {
        Source = gameObject.AddComponent<AudioSource>();
        NextLevelSFX = Resources.Load<AudioClip>("SFX/next_level");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.tag == "Player" )
        {
            Source.PlayOneShot(NextLevelSFX, 0.5f);

            PlayerMovement mov = collision.gameObject.transform.GetComponent<PlayerMovement>();
            mov.MovementSpeed = 0;
            
            
            StartCoroutine(LoadLinkedScene());
        }
    }

    private IEnumerator LoadLinkedScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneToLoad);
    }
}
