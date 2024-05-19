using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float Speed;
    public float ChaseSpeed;
    public float ChaseDistance;

    public GameObject GroundCheckEnemy;

    public GameObject PosA;
    public GameObject PosB;

    private Animator Animator;


    [SerializeField] private LayerMask RayTestLayers;
    [SerializeField] private bool PlayerInSight = false;

    private GameObject Player;
    private Transform CurrentPos;



    void Start()
    {
        CurrentPos = PosB.transform;

        Player = GameObject.Find("Player");

        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        if( !Player ) return; // early exit, if there's no player

        Vector2 enemyToPlayerVec = Player.transform.position - transform.position;
        
        
        if ( enemyToPlayerVec.magnitude > ChaseDistance )
        {
            PlayerInSight = false;
        }
        else
        {

            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, enemyToPlayerVec, ChaseDistance, RayTestLayers);
            if (raycastHit && raycastHit.collider.tag == "Player")
            {
                Debug.DrawRay(transform.position, enemyToPlayerVec, Color.green);
                PlayerInSight = true;
            }
            else
            {
                Debug.DrawRay(transform.position, enemyToPlayerVec, Color.red);
                PlayerInSight = false;
            }
        }

        if ( PlayerInSight )
        {
            ChasePlayer();
        } else
        {
            Patrol();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer != 9) return;

        // flip direction on ice
        if (CurrentPos == PosB.transform)
        {
            CurrentPos = PosA.transform;
        }
        else
        {
            CurrentPos = PosB.transform;
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
