using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D Rb;
    [SerializeField] private SpriteRenderer PlayerRenderer;
    [SerializeField] private SpriteRenderer ParachuteRendererL;
    [SerializeField] private SpriteRenderer ParachuteRendererR;

    [SerializeField] private float MovementSpeed;
    [SerializeField] private float JumpForce;
    [SerializeField] private LayerMask GroundLayers;

    [SerializeField] private Transform GroundCheck1;
    [SerializeField] private Transform GroundCheck2;


    public bool isGrounded = false;
    public bool Moving = false;
    [SerializeField] private bool isParachuteActive = false;

    private float MoveDir;

    private AudioSource JumpSound;
    private AudioSource WalkSound;
    private AudioSource LandFloorSound;
    private AudioSource WingFlapSound;
    private AudioSource SteamBoostSound;


    private const float parachuteCooldownMax = 1.0f;
    private float parachuteCooldown = 0.0f;
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();

        var AudioSource = GetComponents<AudioSource>();
        JumpSound = AudioSource[0];
        WalkSound = AudioSource[1];
        LandFloorSound = AudioSource[2];
        WingFlapSound = AudioSource[3];
        SteamBoostSound = AudioSource[4];
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isParachuteActive && collision.gameObject.layer == 10) // steam is layer 10
        {
            // oof make sure we only ever trigger on steam!
            Destroy(collision.gameObject);
            Rb.AddForce(new Vector2(0, JumpForce / 5.0f), ForceMode2D.Impulse);
            if(!SteamBoostSound.isPlaying)
            {
                SteamBoostSound.pitch = Random.Range(0.98f, 1.02f);
                SteamBoostSound.volume = Random.Range(0.3f, 0.5f);
                SteamBoostSound.Play();
            }

        }
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
            PlayerRenderer.flipX = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            MoveDir = Input.GetAxis("Horizontal");
            Rb.velocity = new Vector2(MoveDir * MovementSpeed, Rb.velocity.y);
            Moving = true;
            PlayerRenderer.flipX = true;

        }

        bool wasGrounded = isGrounded;

        isGrounded = Physics2D.Raycast(GroundCheck1.position, -transform.up, 0.15f, GroundLayers) || Physics2D.Raycast(GroundCheck2.position, -transform.up, 0.15f, GroundLayers);
        if (!wasGrounded && isGrounded)
        {
            LandFloorSound.pitch = Random.Range(0.95f, 1.05f);
            LandFloorSound.Play();
        }

        bool wasParachuteActive = parachuteCooldown <= 0;

        if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded)
            {
                parachuteCooldown = parachuteCooldownMax;
                if (Rb.velocity.y <= 0.01)
                {
                    Rb.velocity = new Vector2(Rb.velocity.x, JumpForce);
                    //Rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
                    JumpSound.pitch = Random.Range(0.85f, 0.9f);
                    JumpSound.Play();
                }

                Moving = false;
            }
            else
            {
                parachuteCooldown -= Time.deltaTime;
            }
        }
        else
        {
            parachuteCooldown = parachuteCooldownMax;

        }

        isParachuteActive = !isGrounded && parachuteCooldown <= 0;
        ParachuteRendererL.enabled = ParachuteRendererR.enabled = isParachuteActive;


        Rb.gravityScale = isParachuteActive ? 0.5f : 1.0f;

        if (!wasParachuteActive && isParachuteActive)
        {
            WalkSound.pitch = Random.Range(0.95f, 1.05f);
            WingFlapSound.Play();
            Rb.velocity = new Vector2(Rb.velocity.x, JumpForce / 5.0f); // add a small upward boost
        }


        if (!Moving || !isGrounded)
        {
            WalkSound.Stop();
        }
        else if (!WalkSound.isPlaying && isGrounded && Moving)
        {
            WalkSound.pitch = Random.Range(0.95f, 1.05f);
            WalkSound.Play();
        }
    }
}
