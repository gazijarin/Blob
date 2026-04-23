using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Camera Style")]
    public CameraMode cameraMode = CameraMode.BehindBlob;

    [Header("Behind Blob Settings")]
    public Vector3 behindOffset = new Vector3(0, 8, -10);
    public float lookDownAngle = -6f;

    [Header("Follow Settings")]
    public float followSpeed = 4f;
    public float rotationSpeed = 3f;

    [Header("Controls")]
    public KeyCode switchTargetKey = KeyCode.Tab;

    [Header("Zoom")]
    public float defaultFOV = 60f;
    public float zoomOutFOV = 75f;
    public float zoomSpeed = 3f;

    private Transform currentTarget;
    private Transform blobA;
    private Transform blobB;
    private Transform mergedBlob;
    private GameManager gameManager;
    private Camera cam;
    private bool followingBlobA = true;
    private Vector3 velocity = Vector3.zero;

    public enum CameraMode
    {
        BehindBlob,
        Overhead,
        Isometric
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        cam = GetComponent<Camera>();

        if (cam == null)
        {
            Debug.LogError("CameraController needs to be on an object with a Camera component!");
        }

        if (cam != null)
        {
            cam.fieldOfView = defaultFOV;
        }

        FindTarget();

        Debug.Log("📷 Camera setup: Positioned behind blob, looking straight toward gates. Press Tab to switch views when split.");
    }

    void LateUpdate()
    {

        if (currentTarget == null)
        {
            FindTarget();
            if (currentTarget == null) return;
        }

        if (Input.GetKeyDown(switchTargetKey))
        {
            SwitchTarget();
        }

        FollowTarget();

        AdjustZoom();
    }

    void FindTarget()
    {

        if (gameManager != null && gameManager.IsSplit())
        {

            GameObject blobAObj = GameObject.Find("BlobA");
            GameObject blobBObj = GameObject.Find("BlobB");

            if (blobAObj != null) blobA = blobAObj.transform;
            if (blobBObj != null) blobB = blobBObj.transform;

            currentTarget = followingBlobA ? blobA : blobB;

            if (currentTarget != null)
            {
                Debug.Log("📷 Camera following: " + currentTarget.name + " (Press Tab to switch)");
            }
        }
        else
        {

            GameObject mergedBlobObj = GameObject.FindGameObjectWithTag("Player");
            if (mergedBlobObj != null)
            {
                mergedBlob = mergedBlobObj.transform;
                currentTarget = mergedBlob;
                followingBlobA = true;
                Debug.Log("📷 Camera following merged blob - straight view down platform");
            }
        }
    }

    void SwitchTarget()
    {
        if (gameManager != null && gameManager.IsSplit())
        {
            followingBlobA = !followingBlobA;

            if (followingBlobA && blobA != null)
            {
                currentTarget = blobA;
                Debug.Log("📷 Camera switched to GREEN BLOB (BlobA)");
            }
            else if (!followingBlobA && blobB != null)
            {
                currentTarget = blobB;
                Debug.Log("📷 Camera switched to CYAN BLOB (BlobB)");
            }
        }
        else
        {
            Debug.Log("📷 Cannot switch camera - blobs are merged");
        }
    }

    void FollowTarget()
    {
        if (currentTarget == null) return;

        Vector3 targetPosition;
        Quaternion targetRotation;

        switch (cameraMode)
        {
            case CameraMode.BehindBlob:

                targetPosition = currentTarget.position + behindOffset;

                Vector3 lookAtPoint = currentTarget.position + new Vector3(0, 0.5f, 5f);
                Vector3 direction = lookAtPoint - targetPosition;
                targetRotation = Quaternion.LookRotation(direction);

                targetRotation *= Quaternion.Euler(lookDownAngle, 0, 0);
                break;

            case CameraMode.Overhead:

                Vector3 overheadOffset = new Vector3(0, 15, -10);
                targetPosition = currentTarget.position + overheadOffset;
                direction = currentTarget.position - targetPosition;
                targetRotation = Quaternion.LookRotation(direction);
                break;

            case CameraMode.Isometric:

                Vector3 isoOffset = new Vector3(10, 14.14f, -10);
                targetPosition = currentTarget.position + isoOffset;
                direction = currentTarget.position - targetPosition;
                targetRotation = Quaternion.LookRotation(direction);
                break;

            default:
                targetPosition = currentTarget.position + behindOffset;
                lookAtPoint = currentTarget.position + new Vector3(0, 0.5f, 5f);
                direction = lookAtPoint - targetPosition;
                targetRotation = Quaternion.LookRotation(direction);
                break;
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 1f / followSpeed);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    void AdjustZoom()
    {
        if (cam == null) return;

        float targetFOV;

        if (gameManager != null && gameManager.IsSplit())
        {
            targetFOV = zoomOutFOV;
        }
        else
        {
            targetFOV = defaultFOV;
        }

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }

    public void SetTarget(Transform newTarget)
    {
        currentTarget = newTarget;
    }
}
