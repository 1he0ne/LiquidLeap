using UnityEngine;

public class DestroyParticlesOnHit : MonoBehaviour
{
 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.collider.gameObject.tag == "Enemy" )
        {
            Destroy(gameObject,1f);
        }
    }
}