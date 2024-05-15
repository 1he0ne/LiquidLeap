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

    public bool isGrounded = false;
    public bool Moving = false;

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

        isGrounded = Physics2D.Raycast(GroundCheck1.position, -transform.up, 0.15f, GroundLayers) || Physics2D.Raycast(GroundCheck2.position, -transform.up, 0.15f, GroundLayers);
        if (!wasGrounded && isGrounded)
        {
            LandFloorSound.pitch = Random.Range(0.95f, 1.05f);
            LandFloorSound.Play();
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            if (Rb.velocity.y <= 0.01)
            {
                Rb.velocity = new Vector2(Rb.velocity.x, JumpForce);
                //Rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
                JumpSound.pitch = Random.Range(0.85f, 0.9f);
                JumpSound.Play();
            }

            Moving = false;
        }

        if (!Moving || !isGrounded)
        {
            WalkSound.Stop();
        }
        else if (!WalkSound.isPlaying && isGrounded && Moving)
        {
            LandFloorSound.pitch = Random.Range(0.95f, 1.05f);
            WalkSound.Play();
        }
    }
}
