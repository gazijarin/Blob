using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [Header("Visual Feedback")]
    public Color normalColor = new Color(0.2f, 0.8f, 0.2f, 1f);
    public Color completeColor = new Color(0f, 1f, 0f, 1f);
    public Color rejectedColor = new Color(1f, 0.2f, 0.2f, 1f);
    public float colorChangeDuration = 0.5f;

    [Header("Completion Effect")]
    public bool addGlow = true;
    public float glowIntensity = 3f;

    private GameManager gameManager;
    private Renderer rend;
    private Material doorMaterial;
    private bool levelComplete = false;
    private Color currentColor;
    private Color targetColor;
    private float colorTransitionTime = 0f;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in scene!");
        }

        rend = GetComponent<Renderer>();
        if (rend != null)
        {

            doorMaterial = new Material(rend.material);
            doorMaterial.color = normalColor;
            rend.material = doorMaterial;
            currentColor = normalColor;
            targetColor = normalColor;
        }
        else
        {
            Debug.LogError("Exit Door has no Renderer component!");
        }
    }

    void Update()
    {

        if (colorTransitionTime < colorChangeDuration)
        {
            colorTransitionTime += Time.deltaTime;
            float t = colorTransitionTime / colorChangeDuration;
            if (doorMaterial != null)
            {
                doorMaterial.color = Color.Lerp(currentColor, targetColor, t);
            }
        }

        if (levelComplete && doorMaterial != null)
        {

            float pulse = Mathf.Sin(Time.time * 4f) * 0.3f + 1.0f;
            doorMaterial.color = completeColor * pulse;

            if (addGlow)
            {
                doorMaterial.EnableKeyword("_EMISSION");
                doorMaterial.SetColor("_EmissionColor", completeColor * glowIntensity * pulse);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (gameManager != null)
            {
                if (!gameManager.IsSplit())
                {

                    Debug.Log("✅✅✅ MERGED BLOB ENTERED EXIT DOOR - LEVEL COMPLETE! ✅✅✅");
                    Debug.Log("Door glowing BRIGHT GREEN!");

                    levelComplete = true;

                    ChangeColor(completeColor);

                    if (addGlow && doorMaterial != null)
                    {
                        doorMaterial.EnableKeyword("_EMISSION");
                        doorMaterial.SetColor("_EmissionColor", completeColor * glowIntensity);
                    }

                    gameManager.WinLevel();
                }
                else
                {

                    Debug.Log("❌ EXIT DOOR REJECTED: Both blobs must MERGE first!");
                    Debug.Log("Current state: Blobs are SPLIT. Need: MERGED blob.");

                    ChangeColor(rejectedColor);
                    Invoke("ResetColor", colorChangeDuration * 2);
                }
            }
            else
            {
                Debug.LogError("GameManager not found! Can't check if blob is merged.");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !levelComplete)
        {

            ResetColor();
        }
    }

    void ChangeColor(Color newColor)
    {
        currentColor = doorMaterial != null ? doorMaterial.color : normalColor;
        targetColor = newColor;
        colorTransitionTime = 0f;
    }

    void ResetColor()
    {
        if (!levelComplete)
        {
            ChangeColor(normalColor);

            if (doorMaterial != null)
            {
                doorMaterial.DisableKeyword("_EMISSION");
            }
        }
    }
}
