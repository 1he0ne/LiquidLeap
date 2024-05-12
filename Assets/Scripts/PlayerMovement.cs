using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D Rb;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float JumpForce;
    [SerializeField] private SpriteRenderer SpriteRenderer;
    public Transform GroundCheck1; 
    public LayerMask GroundLayer;

    public bool Jumping = false;
    public bool isGrounded = false;
    public bool Moving = false;
 
           float Move;
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck1.position, 0.15f, GroundLayer); 
        
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
        if ( Input.GetKeyUp(KeyCode.D) )
        {
            Moving = false;
        }
        if ( Input.GetKeyUp(KeyCode.A) )
        {
            Moving = false;
        }

        if ( Input.GetKey(KeyCode.Space) && isGrounded )
        {
             Rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
             Moving = false;
             
        }
    }  
}
