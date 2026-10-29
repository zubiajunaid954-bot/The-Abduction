using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2.5f;
    public float runSpeed = 5f;
    public float crouchSpeed = 1.5f;
    public float lyingSpeed = 1.0f;
    public float rotationSpeed = 10f;

    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public float gravity = 20f;

    [Header("Character Controller Settings")]
    public float standingHeight = 2f;
    public float crouchingHeight = 1f;
    public float lyingHeight = 0.5f;

    [Header("Camera Reference")]
    public Transform cameraTransform;

    private CharacterController controller;
    private Animator animator;
    private float currentSpeed;
    private Vector3 verticalVelocity;
    private bool isCrouching = false;
    private bool isLying = false;
    private bool isPerformingAction = false;
    private bool isJumping = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        controller.height = standingHeight;
        controller.center = new Vector3(0, standingHeight / 2, 0);

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (!isPerformingAction)
            HandleMovement();

        HandleActions();
        ApplyGravityAndJump();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * vertical + camRight * horizontal).normalized;

        if (moveDir.magnitude >= 0.1f)
        {
            // 👇 Movement speed based on stance
            if (isLying)
                currentSpeed = lyingSpeed;
            else if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
                currentSpeed = runSpeed;
            else if (isCrouching)
                currentSpeed = crouchSpeed;
            else
                currentSpeed = walkSpeed;

            controller.Move(moveDir * currentSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            float normalizedSpeed = currentSpeed / runSpeed;
            animator.SetFloat("Speed", normalizedSpeed);
            animator.SetBool("IsCrawling", isLying);
        }
        else
        {
            animator.SetFloat("Speed", 0);
            animator.SetBool("IsCrawling", false);
        }
    }

    void HandleActions()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isLying && !isPerformingAction && !isJumping)
            ToggleCrouch();

        if (Input.GetKeyDown(KeyCode.X) && isCrouching && !isPerformingAction)
            ToggleLying();

        if (Input.GetKeyDown(KeyCode.Space) && !isCrouching && !isLying && controller.isGrounded && !isPerformingAction && !isJumping)
            PerformJump();

        if (Input.GetMouseButtonDown(0) && !isPerformingAction && !isJumping)
            PerformAttack();

        // ✅ Can pick up from ANY state (standing, crouch, prone)
        if (Input.GetKeyDown(KeyCode.F) && !isPerformingAction)
            PerformPickup();

        // ✅ E = open animation only (door opens via riddle/door script)
        if (Input.GetKeyDown(KeyCode.E) && !isPerformingAction)
            PerformOpen();
    }

    void PerformJump()
    {
        animator.SetTrigger("Jump");
        verticalVelocity.y = jumpForce;
        isJumping = true;
        Invoke(nameof(ResetJump), 0.8f);
    }

    void ResetJump() => isJumping = false;

    void ToggleCrouch()
    {
        isCrouching = !isCrouching;
        animator.SetBool("IsCrouching", isCrouching);

        if (isCrouching)
        {
            controller.height = crouchingHeight;
            controller.center = new Vector3(0, crouchingHeight / 2, 0);
        }
        else
        {
            controller.height = standingHeight;
            controller.center = new Vector3(0, standingHeight / 2, 0);
        }
    }

    void ToggleLying()
    {
        isLying = !isLying;
        animator.SetBool("IsLying", isLying);

        if (isLying)
        {
            controller.height = lyingHeight;
            controller.center = new Vector3(0, lyingHeight / 2, 0);
        }
        else
        {
            controller.height = crouchingHeight;
            controller.center = new Vector3(0, crouchingHeight / 2, 0);
        }
    }

    void PerformAttack()
    {
        animator.SetTrigger("Attack");
        isPerformingAction = true;
        Invoke(nameof(ResetAction), 1f);
    }

    void PerformPickup()
    {
        // ✅ Works in any stance (standing, crouching, or prone)
        animator.SetTrigger("PickUp");
        isPerformingAction = true;
        Invoke(nameof(ResetAction), 1.2f);
    }

    void PerformOpen()
    {
        animator.SetTrigger("Open"); // 🎥 just plays open animation
        isPerformingAction = true;
        Invoke(nameof(ResetAction), 1.2f);
    }

    void ResetAction() => isPerformingAction = false;

    void ApplyGravityAndJump()
    {
        if (controller.isGrounded && verticalVelocity.y < 0)
            verticalVelocity.y = -2f;
        else
            verticalVelocity.y -= gravity * Time.deltaTime;

        controller.Move(verticalVelocity * Time.deltaTime);
    }

    public void TriggerPickup()
    {
        if (!isPerformingAction)
            PerformPickup();
    }
}
