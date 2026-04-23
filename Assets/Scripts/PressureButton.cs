using UnityEngine;

public class PressureButton : MonoBehaviour
{
    public GameObject gate;
    public bool isPressed = false;
    private int blobsOnButton = 0;

    private static int totalButtonsPressed = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            blobsOnButton++;
            if (blobsOnButton == 1)
            {
                totalButtonsPressed++;
                Debug.Log("Button pressed! Total buttons active: " + totalButtonsPressed);
            }
            UpdateButton();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            blobsOnButton--;
            if (blobsOnButton < 0) blobsOnButton = 0;

            if (blobsOnButton == 0)
            {
                totalButtonsPressed--;
                if (totalButtonsPressed < 0) totalButtonsPressed = 0;
                Debug.Log("Button released! Total buttons active: " + totalButtonsPressed);
            }
            UpdateButton();
        }
    }

    void UpdateButton()
    {
        isPressed = blobsOnButton > 0;

        if (gate != null)
        {
            gate.SetActive(totalButtonsPressed == 0);
        }

        float yScale = isPressed ? 0.08f : 0.1f;
        transform.localScale = new Vector3(1f, yScale, 1f);
    }
}
