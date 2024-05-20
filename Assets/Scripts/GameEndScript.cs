using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndScript : MonoBehaviour
{
    [SerializeField] private string SceneToLoad;
    [SerializeField] private int LevelId;

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
            mov.SetRBEnabled(false);

            PlayerHealth health = collision.gameObject.transform.GetComponent<PlayerHealth>();
            PersistentStorage.NumPlayerLives = health.Health;
                        
            StartCoroutine(LoadLinkedScene());
        }
    }

    private IEnumerator LoadLinkedScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneToLoad);
    }
}
