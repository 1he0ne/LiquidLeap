using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator Animator;
    public PlayerMovement PlayerMovement;


    private void Update()
    {
        if ( PlayerMovement.Moving )
        {
            Animator.SetTrigger("Run");
        }

        if ( !PlayerMovement.Moving )
        {
            Animator.SetTrigger("Idle");
        }

        if ( !PlayerMovement.isGrounded )
        {
            Animator.SetTrigger("Jump");
        }

        if ( PlayerMovement.isGrounded )
        {

        }
        
    }
}

