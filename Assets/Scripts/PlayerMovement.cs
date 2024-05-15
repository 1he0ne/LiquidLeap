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

    private AudioSource JumpSound;
    private AudioSource WalkSound;
    private AudioSource LandFloorSound;
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();

        var AudioSource = GetComponents<AudioSource>();
        JumpSound = AudioSource[0];
        WalkSound = AudioSource[1];
        LandFloorSound = AudioSource[2];
    }

    // Update is called once per frame
    void Update()
    {
        Moving = false;

        if (Input.GetKey(KeyCode.D))
        {
            MoveDir = Input.GetAxis("Horizontal");
            Rb.velocity = new Vector2(MoveDir * MovementSpeed, Rb.velocity.y);
            Moving = true;
            SpriteRenderer.flipX = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            MoveDir = Input.GetAxis("Horizontal");
            Rb.velocity = new Vector2(MoveDir * MovementSpeed, Rb.velocity.y);
            Moving = true;
            SpriteRenderer.flipX = true;

        }

        bool wasGrounded = isGrounded;

        isGrounded = Physics2D.Raycast(GroundCheck1.position, -transform.up, 0.1f, GroundLayers) || Physics2D.Raycast(GroundCheck2.position, -transform.up, 0.1f, GroundLayers);
        if (!wasGrounded && isGrounded)
        {
            LandFloorSound.Play();
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded && JumpCooldown == 0)
        {
            JumpCooldown += 3;
            Rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            Moving = false;
            JumpSound.Play();
        }
        else if (isGrounded)
        {
            JumpCooldown = Mathf.Max(JumpCooldown - 1, 0);
        }

        if (!Moving)
        {
            WalkSound.Stop();
        }
        else if (!WalkSound.isPlaying && isGrounded)
        {
            WalkSound.Play();
        }
    }
}
