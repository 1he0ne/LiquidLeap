using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float Speed;
    public float CircleRadius;
    private Rigidbody2D EnemyRb;
    public GameObject GroundCheckEnemy;
    public LayerMask GroundLayer;
    public bool IsGrounded;
    public bool FacingRight;
    void Start()
    {
        EnemyRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Patrol()
    {
        if (IsGrounded)
        {
            EnemyRb.velocity = Vector2.right * Speed * Time.deltaTime;
            IsGrounded = Physics2D.OverlapCircle(GroundCheckEnemy.transform.position, CircleRadius, GroundLayer);

            if ( !IsGrounded && FacingRight )
            {
                Flip();
            }
            else if (!IsGrounded && !FacingRight )
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        EnemyRb.transform.Rotate(0, 180, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GroundCheckEnemy.transform.position, CircleRadius);
    }
}
