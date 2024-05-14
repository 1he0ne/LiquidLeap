using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float Speed;
    public float CircleRadius;
    public float Radius;
    public Rigidbody2D EnemyRb;
    public GameObject GroundCheckEnemy;
    public Transform PlayerPosition;
    public LayerMask GroundLayer;
    public bool IsGrounded;
    public bool FacingRight;
    public bool PlayerInSight = false;
    void Start()
    {
        EnemyRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var raycastHit = Physics2D.Raycast(transform.position, Vector2.left, Radius);
        if ( raycastHit )
        {
           
            Debug.Log("PlayerSighted");
        }


        //Patrol();


    }

    void Patrol()
    {
        PlayerInSight = false;
        EnemyRb.velocity = Vector2.right * Speed * Time.deltaTime;
        IsGrounded = Physics2D.OverlapCircle(GroundCheckEnemy.transform.position, CircleRadius, GroundLayer);

        if ( !IsGrounded && FacingRight )
        {
            Flip();
        }
        else if ( !IsGrounded && !FacingRight )
        {
            Flip();
        }
    }

    private void Flip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(new Vector3(0, 180, 0));
        Speed = -Speed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GroundCheckEnemy.transform.position, CircleRadius);
        Gizmos.DrawLine(transform.position, Vector2.left * Radius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.collider.gameObject.tag == "Liquid" )
        {
            Debug.Log("Hit");
        }
    }

    void AttackPlayer()
    {
        
    }
}

