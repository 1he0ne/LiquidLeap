using UnityEngine;

public class DestroyParticleEffect : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip Clip;

    private void Start()
    {
        AudioSource.PlayOneShot(Clip);
    }
   
    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 02f);
    }
}
