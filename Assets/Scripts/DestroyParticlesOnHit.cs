using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticlesOnHit : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.collider.gameObject.tag == "Enemy" )
        {
            Destroy(gameObject);
        }
    }
}
