using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public GameObject blobPrefab;
    public Transform spawnPoint;

    [Header("Split Settings")]
    public float splitTimer = 30f;
    public float mergeDistance = 3.0f;
    public KeyCode splitKey = KeyCode.E;

    [Header("Movement Settings")]
    public float moveSpeed = 3.5f;
    public float splitJumpForce = 9f;
    public float mergedJumpForce = 3f;

    [Header("Merge Visual Feedback")]
    public bool showMergeZone = true;
    public bool showMergeMessages = true;

    private GameObject blobA;
    private GameObject blobB;
    private bool isSplit = false;
    private float currentSplitTimer;
    private bool hasShownCloseMessage = false;

    void Start()
    {

        if (blobPrefab != null && spawnPoint != null)
        {
            GameObject initialBlob = Instantiate(blobPrefab, spawnPoint.position, Quaternion.identity);
            initialBlob.name = "Blob";

            BlobController controller = initialBlob.GetComponent<BlobController>();
            if (controller != null)
            {
                controller.leftKey = KeyCode.A;
                controller.rightKey = KeyCode.D;
                controller.forwardKey = KeyCode.W;
                controller.backKey = KeyCode.S;
                controller.jumpKey = KeyCode.Space;
                controller.moveSpeed = moveSpeed;
                controller.jumpForce = mergedJumpForce;
            }

            Debug.Log("🎮 Blob spawned! WASD to move, SPACE to jump, E to split");
        }
        else
        {
            Debug.LogError("Blob Prefab or Spawn Point not assigned!");
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(splitKey) && !isSplit)
        {
            SplitBlob();
        }

        if (isSplit)
        {
            currentSplitTimer -= Time.deltaTime;

            if (blobA == null || blobB == null)
            {
                Debug.LogWarning("One blob was destroyed! Resetting split state.");
                isSplit = false;
                hasShownCloseMessage = false;
                return;
            }

            float distance = Vector3.Distance(blobA.transform.position, blobB.transform.position);

            if (showMergeMessages && distance < mergeDistance * 1.5f && distance >= mergeDistance)
            {
                if (!hasShownCloseMessage)
                {
                    Debug.Log("💚 Blobs getting close! Distance: " + distance.ToString("F1") + " units (will merge at " + mergeDistance + ")");
                    hasShownCloseMessage = true;
                }
            }

            if (distance >= mergeDistance * 1.5f)
            {
                hasShownCloseMessage = false;
            }

            if (distance < mergeDistance)
            {
                Debug.Log("✅ MERGE TRIGGERED! Distance: " + distance.ToString("F1") + " units");
                MergeBlobs();
            }
        }
    }

    void SplitBlob()
    {
        GameObject originalBlob = GameObject.FindGameObjectWithTag("Player");
        if (originalBlob == null)
        {
            Debug.LogError("No blob found with Player tag!");
            return;
        }

        Vector3 pos = originalBlob.transform.position;

        Destroy(originalBlob);

        blobA = Instantiate(blobPrefab, pos + Vector3.left * 2.5f, Quaternion.identity);
        blobA.name = "BlobA";
        blobA.transform.localScale = Vector3.one * 0.7f;

        BlobController controllerA = blobA.GetComponent<BlobController>();
        controllerA.leftKey = KeyCode.A;
        controllerA.rightKey = KeyCode.D;
        controllerA.forwardKey = KeyCode.W;
        controllerA.backKey = KeyCode.S;
        controllerA.jumpKey = KeyCode.Space;
        controllerA.moveSpeed = moveSpeed;
        controllerA.jumpForce = splitJumpForce;

        Renderer rendA = blobA.GetComponent<Renderer>();
        if (rendA != null)
        {
            Material matA = new Material(rendA.material);
            matA.color = Color.green;
            rendA.material = matA;
        }

        blobB = Instantiate(blobPrefab, pos + Vector3.right * 2.5f, Quaternion.identity);
        blobB.name = "BlobB";
        blobB.transform.localScale = Vector3.one * 0.7f;

        BlobController controllerB = blobB.GetComponent<BlobController>();
        controllerB.leftKey = KeyCode.LeftArrow;
        controllerB.rightKey = KeyCode.RightArrow;
        controllerB.forwardKey = KeyCode.UpArrow;
        controllerB.backKey = KeyCode.DownArrow;
        controllerB.jumpKey = KeyCode.RightShift;
        controllerB.moveSpeed = moveSpeed;
        controllerB.jumpForce = splitJumpForce;

        Renderer rendB = blobB.GetComponent<Renderer>();
        if (rendB != null)
        {
            Material matB = new Material(rendB.material);
            matB.color = Color.cyan;
            rendB.material = matB;
        }

        isSplit = true;
        currentSplitTimer = splitTimer;
        hasShownCloseMessage = false;

        Debug.Log("════════════════════════════════");
        Debug.Log("🎯 BLOB SPLIT!");
        Debug.Log("════════════════════════════════");
        Debug.Log("⏱️  Timer: " + splitTimer + " seconds");
        Debug.Log("🤝 Merge Distance: " + mergeDistance + " units (VERY LENIENT!)");
        Debug.Log("🟢 GREEN BLOB (BlobA): WASD to move, SPACE to jump");
        Debug.Log("🔵 CYAN BLOB (BlobB): Arrow keys to move, RIGHT SHIFT to jump");
        Debug.Log("💡 TIP: Move blobs within " + mergeDistance + " units to auto-merge!");
        Debug.Log("════════════════════════════════");
    }

    void MergeBlobs()
    {
        if (blobA == null || blobB == null)
        {
            Debug.LogWarning("Can't merge - one or both blobs missing");
            isSplit = false;
            hasShownCloseMessage = false;
            return;
        }

        Vector3 midPoint = (blobA.transform.position + blobB.transform.position) / 2f;

        Destroy(blobA);
        Destroy(blobB);

        GameObject mergedBlob = Instantiate(blobPrefab, midPoint, Quaternion.identity);
        mergedBlob.name = "Blob";
        mergedBlob.transform.localScale = Vector3.one;

        BlobController controller = mergedBlob.GetComponent<BlobController>();
        if (controller != null)
        {
            controller.leftKey = KeyCode.A;
            controller.rightKey = KeyCode.D;
            controller.forwardKey = KeyCode.W;
            controller.backKey = KeyCode.S;
            controller.jumpKey = KeyCode.Space;
            controller.moveSpeed = moveSpeed;
            controller.jumpForce = mergedJumpForce;
        }

        Renderer rend = mergedBlob.GetComponent<Renderer>();
        if (rend != null)
        {
            Material mat = new Material(rend.material);
            mat.color = Color.green;
            rend.material = mat;
        }

        isSplit = false;
        hasShownCloseMessage = false;

        Debug.Log("════════════════════════════════");
        Debug.Log("✅ BLOBS MERGED!");
        Debug.Log("════════════════════════════════");
        Debug.Log("🟢 Merged blob position: " + midPoint.ToString("F1"));
        Debug.Log("🎮 Controls: WASD to move, SPACE to jump");
        Debug.Log("💡 Press E to split again, or find the exit door!");
        Debug.Log("════════════════════════════════");
    }

    public bool IsSplit()
    {
        return isSplit;
    }

    public void WinLevel()
    {
        Debug.Log("════════════════════════════════");
        Debug.Log("🎉 LEVEL COMPLETE! 🎉");
        Debug.Log("════════════════════════════════");
        Debug.Log("Scene will reload in 2 seconds...");
        Invoke("ReloadScene", 2f);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
