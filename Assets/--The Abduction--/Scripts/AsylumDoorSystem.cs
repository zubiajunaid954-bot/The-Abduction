using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
public class AsylumDoorSystem : MonoBehaviour
{
    [System.Serializable]
    public class Door
    {
        public string riddle;
        public string answer;
        public Animator doorAnimator;
        public bool isUnlocked = false;
        public Transform doorTransform;

        public float interactDistance = 3f;  // Unique per door
    }

    public Door[] doors;

    public GameObject riddlePanel;
    public TMP_Text riddleText;
    public TMP_InputField answerInput;
    public Button submitButton;
    public TMP_Text feedbackText;
    public TMP_Text hintText;

    public AudioSource spookyAudio;
    public Light flickerLight;

    private Door currentDoor = null;
    private Transform player;
    private int unlockedDoorCount = 0;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        riddlePanel.SetActive(false);
        hintText.gameObject.SetActive(false);

        submitButton.onClick.AddListener(CheckAnswer);
    }

    private void Update()
    {
        currentDoor = null;
        hintText.gameObject.SetActive(false);

        float bestDistance = float.MaxValue;

        foreach (Door door in doors)
        {
            if (door.isUnlocked) continue;

            // Limit height difference (skip other floors)
            float heightDiff = Mathf.Abs(player.position.y - door.doorTransform.position.y);
            if (heightDiff > 6f)
                continue;

            // Horizontal-only distance
            Vector3 playerPos = player.position;
            Vector3 doorPos = door.doorTransform.position;

            playerPos.y = 0;
            doorPos.y = 0;

            float dist = Vector3.Distance(playerPos, doorPos);

            if (dist <= door.interactDistance && dist < bestDistance)
            {
                bestDistance = dist;
                currentDoor = door;
            }
        }

        if (currentDoor != null)
        {
            hintText.gameObject.SetActive(true);
            hintText.text = "Press E to interact";

            // PREVENT "E" WHILE TYPING IN ANSWER FIELD
            if (!answerInput.isFocused && Input.GetKeyDown(KeyCode.E))
            {
                ShowRiddlePanel();
            }
        }
    }

    private void ShowRiddlePanel()
    {
        riddlePanel.SetActive(true);
        riddleText.text = currentDoor.riddle;
        answerInput.text = "";
        feedbackText.text = "";
    }

    private void HideRiddlePanel()
    {
        riddlePanel.SetActive(false);
    }

    private void CheckAnswer()
    {
        if (currentDoor == null) return;

        // DO NOT ALLOW CHECK WHILE TYPING (safety check)
        if (answerInput.isFocused) return;

        string playerAnswer = answerInput.text.Trim().ToLower();
        string correctAnswer = currentDoor.answer.ToLower();

        if (playerAnswer == correctAnswer)
        {
            feedbackText.text = "Correct! Door unlocked!";
            currentDoor.isUnlocked = true;
            unlockedDoorCount++;
            if (unlockedDoorCount >= doors.Length)
            {
                GetComponent<GameManage>().MainKey.SetActive(true);
                GetComponent<GameManage>().maintrigger.SetActive(true);
                GetComponent<GameManage>().mainparticl.SetActive(true);
            }
            if (currentDoor.doorAnimator != null)
                currentDoor.doorAnimator.SetTrigger("Open");

            HideRiddlePanel();
        }
        else
        {
            feedbackText.text = "Wrong answer!";
            TriggerCreepyEffect();
        }

        StartCoroutine(ClearFeedback());
    }

    IEnumerator ClearFeedback()
    {
        yield return new WaitForSeconds(1f);
        feedbackText.text = "";
    }

    private void TriggerCreepyEffect()
    {
        if (spookyAudio) spookyAudio.Play();
        if (flickerLight) StartCoroutine(FlickerLight());
    }

    IEnumerator FlickerLight()
    {
        int flickers = Random.Range(2, 5);
        for (int i = 0; i < flickers; i++)
        {
            flickerLight.enabled = false;
            yield return new WaitForSeconds(0.1f);

            flickerLight.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }



    /* [System.Serializable]
    public class Door
    {
        public string riddle;
        public string answer;
        public Animator doorAnimator;
        public bool isUnlocked = false;
        public Transform doorTransform;

        public float interactDistance = 3f;  // <-- unique per door!
    }

    public Door[] doors;

    public GameObject riddlePanel;
    public TMP_Text riddleText;
    public TMP_InputField answerInput;
    public Button submitButton;
    public TMP_Text feedbackText;
    public TMP_Text hintText;

    public AudioSource spookyAudio;
    public Light flickerLight;

    private Door currentDoor = null;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        riddlePanel.SetActive(false);
        hintText.gameObject.SetActive(false);

        submitButton.onClick.AddListener(CheckAnswer);
    }

    private void Update()
    {
        currentDoor = null;
        hintText.gameObject.SetActive(false);

        float bestDistance = float.MaxValue;

        foreach (Door door in doors)
        {
            if (door.isUnlocked) continue;

            // --- Fix: limit detection to doors on same floor ---
            float heightDiff = Mathf.Abs(player.position.y - door.doorTransform.position.y);
            if (heightDiff > 6f)
                continue;  // skip doors on different floors

            // --- Fix: horizontal distance only (ignore Y) ---
            Vector3 playerPos = player.position;
            Vector3 doorPos = door.doorTransform.position;

            playerPos.y = 0;
            doorPos.y = 0;

            float dist = Vector3.Distance(playerPos, doorPos);

            // Check if inside that door's unique interact distance
            if (dist <= door.interactDistance && dist < bestDistance)
            {
                bestDistance = dist;
                currentDoor = door;
            }
        }

        // If we found a door in range
        if (currentDoor != null)
        {
            hintText.gameObject.SetActive(true);
            hintText.text = "Press E to interact";

            if (Input.GetKeyDown(KeyCode.E))
            {
                ShowRiddlePanel();
            }
        }
    }




    *//* private void Update()
    {
        currentDoor = null;
        hintText.gameObject.SetActive(false);

        float bestDistance = float.MaxValue;


        foreach (Door door in doors)
        {
            if (door.isUnlocked) continue;

            // Check height difference to avoid lower/upper floor doors
           // float heightDiff = Mathf.Abs(player.position.y - door.doorTransform.position.y);
           // if (heightDiff > 2f) continue;

            // Horizontal distance only
            Vector3 playerPos = player.position;
            Vector3 doorPos = door.doorTransform.position;
            playerPos.y = 0;
            doorPos.y = 0;

            float dist = Vector3.Distance(playerPos, doorPos);

            if (dist <= door.interactDistance && dist < bestDistance)
            {
                bestDistance = dist;
                currentDoor = door;
            }
        }



        *//*foreach (Door door in doors)
        {
            if (door.isUnlocked) continue;

            float dist = Vector3.Distance(player.position, door.doorTransform.position);

            // ONLY consider doors that the player is close enough to
            if (dist <= door.interactDistance && dist < bestDistance)
            {
                bestDistance = dist;
                currentDoor = door;
            }
        }

        if (currentDoor != null)
        {
            hintText.gameObject.SetActive(true);
            hintText.text = "Press E to interact";

            if (Input.GetKeyDown(KeyCode.E))
            {
                ShowRiddlePanel();
            }
        }*//*
    }*//*

    private void ShowRiddlePanel()
    {
        riddlePanel.SetActive(true);
        riddleText.text = currentDoor.riddle;
        answerInput.text = "";
        feedbackText.text = "";
    }

    private void HideRiddlePanel()
    {
        riddlePanel.SetActive(false);
    }

    private void CheckAnswer()
    {
        if (currentDoor == null) return;

        string playerAnswer = answerInput.text.Trim().ToLower();
        string correctAnswer = currentDoor.answer.ToLower();

        if (playerAnswer == correctAnswer)
        {
            feedbackText.text = "Correct! Door unlocked!";
            currentDoor.isUnlocked = true;

            if (currentDoor.doorAnimator != null)
                currentDoor.doorAnimator.SetTrigger("Open");

            HideRiddlePanel();
        }
        else
        {
            feedbackText.text = "Wrong answer!";
            TriggerCreepyEffect();

        }
        StartCoroutine(w());
    }
 
    IEnumerator w()
    {
        yield return new WaitForSeconds(1f);
        feedbackText.text = "";

    }
    private void TriggerCreepyEffect()
    {
        if (spookyAudio) spookyAudio.Play();
        if (flickerLight) StartCoroutine(FlickerLight());
    }

    IEnumerator FlickerLight()
    {
        int flickers = Random.Range(2, 5);
        for (int i = 0; i < flickers; i++)
        {
            flickerLight.enabled = false;
            yield return new WaitForSeconds(0.1f);
            flickerLight.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }*/
}
