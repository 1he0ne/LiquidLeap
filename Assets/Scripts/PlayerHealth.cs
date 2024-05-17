using UnityEngine;


public class PlayerHealth : MonoBehaviour, IDamageable
{ 
    public int Health { get; set; }

    private AudioSource Audio;
    private Animator Animator;
    private ParticleSystem Particle;
    private AudioClip Clip;

    void Start()
    {
        Health = 100;

        Audio = gameObject.AddComponent<AudioSource>();
        Animator = GetComponent<Animator>();
        Particle = Resources.Load<ParticleSystem>("PlayerDeathParticles");
        Clip = Resources.Load<AudioClip>("SFX/PlayerHurt");
    }
    public void Damage(int value)
    {
        Health -= value;
    }

    // Update is called once per frame
    void Update()
    {
        if ( Health <= 0 )
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.collider.gameObject.tag == "Enemy" )
        {
            Damage(20);
            Audio.PlayOneShot(Clip);
            Animator.SetTrigger("PlayerHurt");
            Debug.Log("Ouch!");
        }
        
        if ( collision.collider.gameObject.tag == "Death" )
        {
            Die();
        }
    }

    public void Die()
    {
        Instantiate(Particle, transform.position, Quaternion.identity);
        Destroy(gameObject);
        //Sound
        //Destroy
        Debug.Log("Player Died");
    }
}
