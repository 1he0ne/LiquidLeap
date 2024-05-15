using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float Speed;
    public float ChaseSpeed;
    public float ChaseDistance;
    public Rigidbody2D EnemyRb;
    public GameObject GroundCheckEnemy;
    
    public GameObject PosA;  
    public GameObject PosB;
    
    public SpriteRenderer SpriteRenderer;
    public Transform CurrentPos;
    public Transform PlayerPosition;
    public LayerMask GroundLayer;
    public bool PlayerInSight = false;

    void Start()
    {
        EnemyRb = GetComponent<Rigidbody2D>();
        CurrentPos = PosB.transform;
    }

    void Update()
    {
        if ( Vector2.Distance(transform.position, PlayerPosition.position) > ChaseDistance )
        {
            Patrol();
        }
        if ( Vector2.Distance(transform.position, PlayerPosition.position) < ChaseDistance )
        {
            PlayerInSight = true;
        }
        if ( PlayerInSight )
        {
            if ( transform.position.x > PlayerPosition.position.x )
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.position += Vector3.left * ChaseSpeed * Time.deltaTime;

            }
            if ( transform.position.x < PlayerPosition.position.x )
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.position += Vector3.right * ChaseSpeed * Time.deltaTime;

            }
        }
    }

    void Patrol()
    {
       Vector2 point = CurrentPos.position - transform.position;
        if ( CurrentPos == PosB.transform )
        {
            EnemyRb.velocity = new Vector2(Speed, 0);
        }
        else
        {
            EnemyRb.velocity = new Vector2(-Speed, 0);
        }
        if ( Vector2.Distance(transform.position, CurrentPos.position)  < 0.5f && CurrentPos == PosB.transform )
        {
            CurrentPos = PosA.transform;
            SpriteRenderer.flipX = true;
        }
        if ( Vector2.Distance(transform.position, CurrentPos.position) < 0.5f && CurrentPos == PosA.transform )
        {
            CurrentPos = PosB.transform;
            SpriteRenderer.flipX = false; 
        }
    }

    private void Flip()
    {
       
        SpriteRenderer.flipX = true;
       
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
       
    }
}

