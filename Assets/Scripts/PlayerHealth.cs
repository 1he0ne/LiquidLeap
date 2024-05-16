using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int Health { get; set; }

    public ParticleSystem Particle;

    void Start()
    {
        Health = 100;
    }
    public void Damage(int value)
    {
        Health -= value;
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.collider.gameObject.tag == "Enemy" )
        {
            Damage(20);
            //PlaySound;
            //Animation;
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
