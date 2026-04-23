using UnityEngine;

public class PushableStone : MonoBehaviour
{
    [Header("Physics Settings")]
    public float mass = 50f;
    public float drag = 5f;
    public float pushForceMultiplier = 3f;

    [Header("Gravity Settings")]
    public bool useGravity = true;

    [Header("Constraints")]
    public bool freezeRotation = true;

    [Header("Ground Check")]
    public bool onlyPushWhenGrounded = true;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;

    [Header("Audio (Optional)")]
    public AudioClip pushSound;
    public AudioClip slideSound;

    private Rigidbody rb;
    private bool isBeingPushed = false;
    private bool isGrounded = false;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            Debug.Log("Added Rigidbody to " + gameObject.name);
        }

        rb.mass = mass;
        rb.linearDamping = drag;
        rb.angularDamping = 10f;
        rb.useGravity = useGravity;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        if (freezeRotation)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        if (groundLayer == 0)
        {
            groundLayer = LayerMask.GetMask("Ground");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && (pushSound != null || slideSound != null))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f;
        }

        Debug.Log("🪨 " + gameObject.name + " ready! Push Force: " + pushForceMultiplier);
    }

    void FixedUpdate()
    {

        CheckGrounded();

        float horizontalVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z).magnitude;

        if (horizontalVelocity > 0.1f && slideSound != null && audioSource != null)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = slideSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else if (audioSource != null && audioSource.isPlaying && audioSource.clip == slideSound)
        {
            audioSource.Stop();
        }
    }

    void CheckGrounded()
    {

        float checkDistance = GetComponent<Collider>().bounds.extents.y + groundCheckDistance;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, checkDistance, groundLayer);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isBeingPushed = true;

            if (pushSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(pushSound);
            }

            Debug.Log("💥 " + gameObject.name + " pushed by " + collision.gameObject.name);
        }
    }

    void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {

            if (onlyPushWhenGrounded && !isGrounded)
            {
                Debug.Log("⚠️ Can't push - stone is airborne!");
                return;
            }

            Rigidbody blobRb = collision.gameObject.GetComponent<Rigidbody>();

            if (blobRb != null)
            {

                Vector3 pushDirection = transform.position - collision.transform.position;
                pushDirection.y = 0;
                pushDirection.Normalize();

                Vector3 blobVelocity = blobRb.linearVelocity;
                blobVelocity.y = 0;

                float pushForce = blobVelocity.magnitude * pushForceMultiplier;

                if (pushForce > 0.1f)
                {
                    rb.AddForce(pushDirection * pushForce, ForceMode.Force);
                    Debug.Log("⬆️ PUSHING! Force: " + pushForce.ToString("F1"));
                }
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isBeingPushed = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Button"))
        {
            Debug.Log(gameObject.name + " is on a button!");
        }
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying && rb != null)
        {
            if (rb.linearVelocity.magnitude > 0.1f)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(transform.position, rb.linearVelocity);
            }

            if (GetComponent<Collider>() != null)
            {
                float checkDistance = GetComponent<Collider>().bounds.extents.y + groundCheckDistance;
                Gizmos.color = isGrounded ? Color.green : Color.red;
                Gizmos.DrawRay(transform.position, Vector3.down * checkDistance);
            }
        }
    }
}
