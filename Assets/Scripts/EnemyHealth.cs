using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public int Health { get; set; }

    private ParticleSystem Particle;

    private AudioClip ClipHurt;

    private Animator Animator;
    private AudioSource AudioSource;

    void Start()
    {
        Health = 100;

        Animator = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>();
        ClipHurt = Resources.Load<AudioClip>("SFX/EnemyHurt");

        Particle = Resources.Load<ParticleSystem>("EnemyDeathParticles");
    }
    public void Damage(int value)
    {
        Health -= value;
    }

    void Update()
    {
        if ( Health <= 0 )
        {
            // Debug.Log("Enemy Died");
            Animator.SetTrigger("Die");
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.collider.gameObject.tag == "Water" )
        {
            
            Damage(15);
            // Debug.Log("Enemy takes hit");
            Animator.SetTrigger("Hurt");
            AudioSource.PlayOneShot(ClipHurt, 0.8f);
            // StartCoroutine(PlayHurtSound());
        }

        if ( collision.collider.gameObject.tag == "Death" )
        {
            Die();
        }
    }
    public void Die()
    {
        // Spawn particles
        Instantiate(Particle,transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    //IEnumerator PlayHurtSound()
    //{
    //    // yield return new WaitForSeconds(0.1f);
    //    AudioSource.PlayOneShot(ClipHurt, 0.8f);
    //}
}
