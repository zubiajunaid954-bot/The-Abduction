using UnityEngine;

public class HiddenClue : MonoBehaviour
{
    [TextArea]
    public string clueText;
    private bool canRead = false;

    void Update()
    {
        if (canRead && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Clue Read: " + clueText);
            // TODO: Show clue on UI
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            canRead = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            canRead = false;
    }
}
