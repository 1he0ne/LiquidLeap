using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public int Health { get; set; }


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
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.collider.gameObject.tag == "Water" )
        {
            //TODO: Animations!
            Damage(1);
            Debug.Log("Enemy takes hit");
            //PlayHurt Animation
        }
    }
    public void Die()
    {
         //PlayAnimation
         Destroy(gameObject,02f);

        
    }
}
