using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D Rb;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float JumpForce;
    
    public Transform GroundCheck1; 
    public LayerMask GroundLayer;
    
    bool isGrounded = false;
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
        }
        
        if ( Input.GetKey(KeyCode.A) )
        {

            Move = Input.GetAxis("Horizontal");
            Rb.velocity = new Vector2(Move * MovementSpeed, Rb.velocity.y);
        }
        
        if ( Input.GetKey(KeyCode.Space) && isGrounded )
        {
           
            Rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        }
    }  
}
