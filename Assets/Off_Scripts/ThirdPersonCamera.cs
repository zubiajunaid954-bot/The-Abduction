using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // Ethan
    public Transform targetLookAt; // Optional: look at specific point on character

    [Header("Camera Position")]
    public Vector3 offset = new Vector3(0, 2.5f, -4f); // Behind and above player
    public float distance = 4f;
    public float height = 2.5f;

    [Header("Camera Smoothing")]
    public float smoothSpeed = 10f; // How fast camera follows
    public float rotationSpeed = 5f; // How fast camera rotates with player

    [Header("Mouse Look (Optional)")]
    public bool enableMouseLook = false;
    public float mouseSensitivity = 2f;
    public float minYAngle = -20f;
    public float maxYAngle = 60f;

    [Header("Collision")]
    public bool avoidObstacles = true;
    public float collisionOffset = 0.3f;
    public LayerMask collisionLayers;

    private float currentX = 0f;
    private float currentY = 0f;
    private Vector3 currentVelocity;

    void Start()
    {
        // Lock cursor if mouse look enabled
        if (enableMouseLook)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Initialize camera angles
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("ThirdPersonCamera: No target assigned!");
            return;
        }

        HandleCameraMovement();
    }

    void HandleCameraMovement()
    {
        // Get mouse input if enabled
        if (enableMouseLook)
        {
            currentX += Input.GetAxis("Mouse X") * mouseSensitivity;
            currentY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
        }
        else
        {
            // Follow player rotation
            currentX = Mathf.LerpAngle(currentX, target.eulerAngles.y, rotationSpeed * Time.deltaTime);
        }

        // Calculate desired camera position
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * distance);
        desiredPosition.y = target.position.y + height;

        // Smooth camera movement
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, 1f / smoothSpeed);

        // Check for obstacles
        if (avoidObstacles)
        {
            RaycastHit hit;
            Vector3 direction = smoothedPosition - target.position;

            if (Physics.Raycast(target.position, direction.normalized, out hit, direction.magnitude, collisionLayers))
            {
                smoothedPosition = hit.point - direction.normalized * collisionOffset;
            }
        }

        // Apply position
        transform.position = smoothedPosition;

        // Look at target
        Vector3 lookPosition = targetLookAt != null ? targetLookAt.position : target.position + Vector3.up * 1.5f;
        transform.LookAt(lookPosition);
    }

    // Toggle mouse look at runtime
    public void ToggleMouseLook()
    {
        enableMouseLook = !enableMouseLook;

        if (enableMouseLook)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}