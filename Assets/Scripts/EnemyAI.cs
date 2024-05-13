using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float Speed;
    public float CircleRadius;
    public Rigidbody2D EnemyRb;
    public GameObject GroundCheckEnemy;
    public LayerMask GroundLayer;
    public bool IsGrounded;
    public bool FacingRight;
    void Start()
    {
        EnemyRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.collider.gameObject.tag == "Liquid" )
        {
            Debug.Log("Hit");
        }
    }
}
