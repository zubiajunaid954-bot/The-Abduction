using UnityEngine;

public enum ItemType
{
    Weapon,
    Tool,
    Key,
    Other
}

public class PickupItem : MonoBehaviour
{
    [Header("Item Settings")]
    public string itemName = "Hammer";
    public ItemType itemType = ItemType.Weapon;

    [Header("Pickup Settings")]
    public float pickupRange = 2f;
    public KeyCode pickupKey = KeyCode.F;

    [Header("Hand Position Adjustments")]
    public Vector3 handPositionOffset = new Vector3(0, 0.05f, 0.1f);
    public Vector3 handRotationOffset = new Vector3(-90, 0, 0);

    [Header("UI Prompt")]
    public bool showPickupPrompt = true;
    public string promptText = "Press F to pick up";

    private Transform player;
    private PlayerController playerController;
    private bool playerInRange = false;
    private bool isPickedUp = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        if (isPickedUp || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= pickupRange)
        {
            playerInRange = true;

            if (Input.GetKeyDown(pickupKey))
            {
                PickupThisItem();
            }
        }
        else
        {
            playerInRange = false;
        }
    }

    void PickupThisItem()
    {
        if (playerController != null)
        {
            playerController.TriggerPickup();
        }

        Invoke("AttachToHand", 0.6f);
    }

    void AttachToHand()
    {
        Transform handHolder = FindHandHolder(player);

        if (handHolder != null)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;

            transform.SetParent(handHolder);
            transform.localPosition = handPositionOffset;
            transform.localRotation = Quaternion.Euler(handRotationOffset);

            isPickedUp = true;
            Debug.Log(itemName + " picked up and attached to hand!");
        }
        else
        {
            Debug.LogError("HandHolder not found! Create empty object named 'HandHolder' under RightHand bone.");
        }
    }

    Transform FindHandHolder(Transform parent)
    {
        Transform[] allChildren = parent.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.name == "HandHolder")
            {
                return child;
            }
        }
        return null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }

    void OnGUI()
    {
        if (playerInRange && showPickupPrompt && !isPickedUp)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 0.5f);

            if (screenPos.z > 0)
            {
                GUIStyle style = new GUIStyle();
                style.alignment = TextAnchor.MiddleCenter;
                style.fontSize = 16;
                style.normal.textColor = Color.white;

                GUI.Label(new Rect(screenPos.x - 75, Screen.height - screenPos.y - 25, 150, 50), promptText, style);
            }
        }
    }
}