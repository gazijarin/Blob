using UnityEngine;

public class BlobController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [Header("Controls")]
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backKey = KeyCode.S;
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Animation")]
    public Animator animator;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;
    private float moveInputX;
    private float moveInputZ;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
            if (animator != null)
            {
                Debug.Log("✅ Animator found: " + animator.gameObject.name);
            }
        }
    }

    void Update()
    {

        moveInputX = 0f;
        if (Input.GetKey(leftKey)) moveInputX = -1f;
        if (Input.GetKey(rightKey)) moveInputX = 1f;

        moveInputZ = 0f;
        if (Input.GetKey(forwardKey)) moveInputZ = 1f;
        if (Input.GetKey(backKey)) moveInputZ = -1f;

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }

        UpdateAnimation();
    }

    void FixedUpdate()
    {

        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        }

        Vector3 input = new Vector3(moveInputX, 0f, moveInputZ).normalized;
        Vector3 targetVel = input * moveSpeed;
        Vector3 velChange = targetVel - rb.linearVelocity;
        velChange.y = 0f;
        rb.AddForce(velChange, ForceMode.VelocityChange);

        if (input.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(input);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    void UpdateAnimation()
    {
        if (animator == null) return;
        float horizontalSpeed = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z).magnitude;
        bool isWalking = horizontalSpeed > 0.1f;

        animator.SetBool("Walk_Anim", isWalking);
    }
}
