using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D Rb;
    private SpriteRenderer SpriteRenderer;

    [SerializeField] private float MovementSpeed;
    [SerializeField] private float JumpForce;
    [SerializeField] private LayerMask GroundLayers;

    [SerializeField] private Transform GroundCheck1;
    [SerializeField] private Transform GroundCheck2;

    public bool Jumping = false;
    public bool isGrounded = false;
    public bool Moving = false;

    private static int JumpCooldown = 0;

    private float MoveDir;
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Moving = false;

        if ( Input.GetKey(KeyCode.D) )
        {
            MoveDir = Input.GetAxis("Horizontal");
            Rb.velocity = new Vector2(MoveDir * MovementSpeed, Rb.velocity.y);
            Moving = true;
            SpriteRenderer.flipX = false;
        }
        
        if ( Input.GetKey(KeyCode.A) )
        {
            MoveDir = Input.GetAxis("Horizontal");
            Rb.velocity = new Vector2(MoveDir * MovementSpeed, Rb.velocity.y);
            Moving = true;
            SpriteRenderer.flipX = true;
           
        }

        isGrounded = Physics2D.Raycast(GroundCheck1.position, -transform.up, 0.1f, GroundLayers) || Physics2D.Raycast(GroundCheck2.position, -transform.up, 0.1f, GroundLayers);

        if (Input.GetKey(KeyCode.Space) && isGrounded && JumpCooldown == 0)
        {
            JumpCooldown += 3;
            Rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            Moving = false;
        }
        else if (isGrounded)
        {
            JumpCooldown = Mathf.Max(JumpCooldown - 1, 0);
        }
    }  
}
