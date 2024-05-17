using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public int Health { get; set; }

    public Animator Animator;
    public ParticleSystem Particle;
    public AudioSource AudioSource;
    public AudioClip Clip;
    void Start()
    {
        Health = 100;
    }
    public void Damage(int value)
    {
        Health -= value;
    }

    void Update()
    {
        if ( Health <= 0 )
        {
            //Die();
            Debug.Log("Enemy Died");
            Animator.SetTrigger("Die");
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.collider.gameObject.tag == "Water" )
        {
            
            Damage(15);
            Debug.Log("Enemy takes hit");
            Animator.SetTrigger("Hurt");
            StartCoroutine(PlayHurtSound());
        }

        if ( collision.collider.gameObject.tag == "Death" )
        {
            Die();
        }
    }
    public void Die()
    {
         //PlayAnimation
       Instantiate(Particle,transform.position, Quaternion.identity);
       Destroy(gameObject);

        
    }

    IEnumerator PlayHurtSound()
    {
        yield return new WaitForSeconds(0.1f);
        AudioSource.PlayOneShot(Clip);

    }
}
