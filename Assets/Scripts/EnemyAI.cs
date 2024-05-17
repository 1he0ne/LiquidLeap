using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float Speed;
    public float ChaseSpeed;
    public float ChaseDistance;

    public GameObject GroundCheckEnemy;

    public GameObject PosA;
    public GameObject PosB;

    public Animator Animator;
    public Transform CurrentPos;

    public LayerMask GroundLayer;
    public bool PlayerInSight = false;

    private GameObject Player;
    // private Rigidbody2D EnemyRb;

    void Start()
    {
        // EnemyRb = GetComponent<Rigidbody2D>();
        CurrentPos = PosB.transform;

        Player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    void Update()
    {

        float distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);

        if ( distanceToPlayer > ChaseDistance )
        {
            Patrol();
            PlayerInSight = false;
        }
        else
        {
            PlayerInSight = true;
        }

        if ( PlayerInSight )
        {
            ChasePlayer();
        }
    }

    void Patrol()
    {
        Animator.SetTrigger("Idle");
        transform.position = Vector2.MoveTowards(transform.position, CurrentPos.position, ChaseSpeed * Time.deltaTime);

        if ( Vector2.Distance(transform.position, CurrentPos.position) < 0.5f )
        {
            if (CurrentPos == PosB.transform)
            {
                CurrentPos = PosA.transform;
            }
            else
            {
                CurrentPos = PosB.transform;
            }
        }

        // Optionally, you can add sprite flipping in patrol as well
        Vector3 direction = CurrentPos.position - transform.position;
        FlipSprite(direction);
    }

    void ChasePlayer()
    {
        Animator.SetTrigger("Idle");
        Vector3 direction = Player.transform.position - transform.position;
        direction.Normalize();

        // Flip the sprite to face the player
        FlipSprite(direction);

        // Move towards the player
        if ( transform.position.x > Player.transform.position.x )
        {
            transform.position += Vector3.left * ChaseSpeed * Time.deltaTime;
        }
        else if ( transform.position.x < Player.transform.position.x )
        {
            transform.position += Vector3.right * ChaseSpeed * Time.deltaTime;
        }
    }

    void FlipSprite(Vector3 direction)
    {
        if ( direction.x > 0 )
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if ( direction.x < 0 )
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}