using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator Animator;
    private PlayerMovement PlayerMovement;

    private void Start()
    {
        Animator = GetComponent<Animator>();
        PlayerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if ( PlayerMovement.isMoving )
        {
            Animator.SetTrigger("Run");
        }

        if ( !PlayerMovement.isMoving )
        {
            Animator.SetTrigger("Idle");
        }

        if ( !PlayerMovement.isGrounded )
        {
            Animator.SetTrigger("Jump");
        }

        // if ( PlayerMovement.isGrounded )
        // {
        // 
        // }
    }
}
