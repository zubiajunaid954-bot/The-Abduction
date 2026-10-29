using UnityEngine;
using TMPro;

public class RiddleInteraction : MonoBehaviour
{
    [Header("UI References")]
    public GameObject riddlePanel;
    public TMP_Text riddleText;
    public GameObject doorToOpen;

    private bool isPlayerNearby = false;
    private bool isRiddleActive = false;

    void Start()
    {
        if (riddlePanel != null)
            riddlePanel.SetActive(false);
    }

    void Update()
    {
        // Show riddle when player presses E
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isRiddleActive)
        {
            ShowRiddle();
        }

        // Check for answer only when riddle is active
        if (isRiddleActive)
        {
            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                SolveRiddle();
            }
        }
    }

    void ShowRiddle()
    {
        isRiddleActive = true;
        if (riddlePanel != null)
            riddlePanel.SetActive(true);

        if (riddleText != null)
            riddleText.text = "\"2 + 2 = ?\"";
    }

    void SolveRiddle()
    {
        if (riddleText != null)
            riddleText.text = "Correct! The door is now open.";

        Invoke(nameof(OpenDoor), 1f);
    }

    void OpenDoor()
    {
        if (doorToOpen != null)
        {
            Animator doorAnim = doorToOpen.GetComponent<Animator>();
            if (doorAnim != null)
                doorAnim.SetTrigger("Open");
            else
                doorToOpen.transform.Rotate(0, -90f, 0); // simple open
        }

        if (riddlePanel != null)
            riddlePanel.SetActive(false);

        isRiddleActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (riddlePanel != null)
                riddlePanel.SetActive(false);
        }
    }
}
