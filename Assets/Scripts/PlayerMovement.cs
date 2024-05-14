using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D Rb;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float JumpForce;
    [SerializeField] private SpriteRenderer SpriteRenderer;
    public Transform GroundCheck; 
    public LayerMask GroundLayers;

    public bool Jumping = false;
    public bool isGrounded = false;
    public bool Moving = false;

    private static int JumpCooldown = 0;

    float Move;
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Moving = false;

        if ( Input.GetKey(KeyCode.D) )
        {
            Move = Input.GetAxis("Horizontal");
            Rb.velocity = new Vector2(Move * MovementSpeed, Rb.velocity.y);
            Moving = true;
            SpriteRenderer.flipX = false;
        }
        
        if ( Input.GetKey(KeyCode.A) )
        {
            Move = Input.GetAxis("Horizontal");
            Rb.velocity = new Vector2(Move * MovementSpeed, Rb.velocity.y);
            Moving = true;
            SpriteRenderer.flipX = true;
           
        }

        isGrounded = Physics2D.Raycast(GroundCheck.position, -transform.up, 0.15f, GroundLayers);

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
