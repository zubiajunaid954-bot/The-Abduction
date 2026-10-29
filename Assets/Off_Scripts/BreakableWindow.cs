using UnityEngine;

public class BreakableWindow : MonoBehaviour
{
    [Header("Break Settings")]
    public float destroyDelay = 0f;  // 0 = instant
    public float detectRange = 2.5f; // how close player must be
    public string playerTag = "Player";

    private bool isBroken = false;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag);
    }

    void Update()
    {
        if (player == null || isBroken) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        // If player is close and attacking with hammer
        if (distance <= detectRange && IsPlayerAttacking())
        {
            BreakWindow();
        }
    }

    bool IsPlayerAttacking()
    {
        // Check if player pressed left mouse button or attack key
        // (you can modify this based on your actual attack key)
        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F);
    }

    public void BreakWindow()
    {
        if (isBroken) return;
        isBroken = true;

        Debug.Log("💥 Window shattered!");

        // Destroy the window (with optional delay)
        Destroy(gameObject, destroyDelay);
    }
}
