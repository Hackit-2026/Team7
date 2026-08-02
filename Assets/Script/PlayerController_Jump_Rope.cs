using UnityEngine;

public class PlayerController_Jump_Rope : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Jump Settings")]
    public float jumpForce = 5f;
    public bool isGrounded = true;

    [Header("VR Camera Setup")]
    [SerializeField] private Transform centerEyeAnchor;

    [Header("Jump Detection Settings")]
    [Tooltip("Required upward speed (meters per second) to trigger a jump.")]
    [SerializeField] private float jumpThresholdSpeed = 1.8f;

    [Tooltip("Minimum upward movement (meters) in a single frame to filter out tiny shakes.")]
    [SerializeField] private float minDeltaY = 0.015f;

    [Header("Game Manager Link")]
    [SerializeField] private RopeJumpGameManager gameManager;

    private float lastCameraY;

    void Start()
    {
        // Get Rigidbody from this object directly
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("This object does not have a Rigidbody component!");
        }

        // Auto-find CenterEyeAnchor
        if (centerEyeAnchor == null)
        {
            GameObject centerEye = GameObject.Find("CenterEyeAnchor");
            if (centerEye != null)
            {
                centerEyeAnchor = centerEye.transform;
            }
            else if (GetComponentInChildren<Camera>() != null)
            {
                centerEyeAnchor = GetComponentInChildren<Camera>().transform;
            }
        }

        if (centerEyeAnchor != null)
        {
            lastCameraY = centerEyeAnchor.localPosition.y;
        }
    }

    void Update()
    {
        if (centerEyeAnchor == null || rb == null) return;

        float currentCameraY = centerEyeAnchor.localPosition.y;

        // 1. Calculate the movement in this frame
        float deltaY = currentCameraY - lastCameraY;

        // 2. Convert to speed per second (meters per second)
        float upwardSpeed = deltaY / Time.deltaTime;

        // 3. Trigger jump only if conditions are met
        if (isGrounded && deltaY > minDeltaY && upwardSpeed > jumpThresholdSpeed)
        {
            Jump();
        }

        lastCameraY = currentCameraY;
    }

    void Jump()
    {
        // Reset vertical velocity of this object and apply force
        // Compatible with both rb.velocity and rb.linearVelocity (Unity 6)
#if UNITY_6_0_OR_NEWER
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
#else
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
#endif
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        isGrounded = false;

        // Notify GameManager to decrease remaining jumps
        if (gameManager != null)
        {
            gameManager.OnPlayerJumped();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ground detection via physical collision
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
