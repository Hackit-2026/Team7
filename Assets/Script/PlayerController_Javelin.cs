using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController_Javelin : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("VR Camera Setup")]
    [SerializeField] private Transform centerEyeAnchor;

    [Header("Javelin Throw Settings")]
    [SerializeField] private float throwPower = 20f;

    [Header("Hand Tracking Setup (Right Hand)")]
    [Tooltip("Drag and drop the RightHandAnchor or hand object from the Hierarchy.")]
    [SerializeField] private Transform rightHandTransform;

    [Tooltip("Speed threshold to trigger a throw when pushing the hand forward.")]
    [SerializeField] private float throwThresholdSpeed = 2.5f; // Slightly lowered for easier tracking

    [Tooltip("Cooldown time (seconds) between throws to prevent continuous shooting.")]
    [SerializeField] private float throwCooldown = 1.0f;

    private CharacterController controller;
    private float nextThrowTime = 0f;

    // Variables to calculate physical hand speed manually
    private Vector3 lastHandPosition;
    private bool isFirstFrame = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (centerEyeAnchor == null)
        {
            GameObject centerEye = GameObject.Find("CenterEyeAnchor");
            if (centerEye != null)
            {
                centerEyeAnchor = centerEye.transform;
            }
        }
    }

    void Update()
    {
        HandleMovement();
        HandleHandTrackingThrow();
    }

    void HandleMovement()
    {
        Vector2 inputDir = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);

        Vector3 forward = centerEyeAnchor != null ? centerEyeAnchor.forward : transform.forward;
        Vector3 right = centerEyeAnchor != null ? centerEyeAnchor.right : transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * inputDir.y + right * inputDir.x).normalized;

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        if (!controller.isGrounded)
        {
            controller.Move(Vector3.down * 9.81f * Time.deltaTime);
        }
    }

    // New: Calculate hand velocity manually by comparing frame positions
    void HandleHandTrackingThrow()
    {
        if (rightHandTransform == null) return;

        // Skip the first frame to establish a baseline position
        if (isFirstFrame)
        {
            lastHandPosition = rightHandTransform.position;
            isFirstFrame = false;
            return;
        }

        // Calculate the physical distance the hand moved in this single frame
        Vector3 handMovementThisFrame = rightHandTransform.position - lastHandPosition;

        // Convert to speed per second (meters per second)
        Vector3 handVelocity = handMovementThisFrame / Time.deltaTime;

        // Save current position for the next frame calculation
        lastHandPosition = rightHandTransform.position;

        if (Time.time < nextThrowTime) return;

        // Calculate how fast the hand is moving FORWARD relative to where the camera is looking
        Vector3 forwardDirection = centerEyeAnchor != null ? centerEyeAnchor.forward : transform.forward;
        float forwardSpeed = Vector3.Dot(handVelocity, forwardDirection);

        // If the calculated forward push speed is faster than the threshold, FIRE!
        if (forwardSpeed > throwThresholdSpeed)
        {
            ThrowSpear(rightHandTransform.position);
            nextThrowTime = Time.time + throwCooldown;
        }
    }

    void ThrowSpear(Vector3 spawnPos)
    {
        Vector3 throwDirection = centerEyeAnchor != null ? centerEyeAnchor.forward : transform.forward;
        throwDirection.y += 0.15f; // Slight realistic throw arc angle
        throwDirection.Normalize();

        Quaternion spawnRot = Quaternion.identity;
        if (throwDirection != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(throwDirection);
            spawnRot = lookRot * Quaternion.Euler(90, 0, 0);
        }

        if (SpearPool.Instance != null)
        {
            GameObject spear = SpearPool.Instance.GetSpear(spawnPos, spawnRot);

            Rigidbody rb = spear.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.AddForce(throwDirection * throwPower, ForceMode.Impulse);
            }
        }
        else
        {
            // Fallback: If SpearPool is missing in the scene, directly instantiate a spear temporary to prevent blocking the game!
            Debug.LogWarning("SpearPool.Instance is missing! Spawning a fallback javelin directly.");
            GameObject fallbackSpear = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            fallbackSpear.transform.position = spawnPos;
            fallbackSpear.transform.rotation = spawnRot;
            fallbackSpear.transform.localScale = new Vector3(0.05f, 1f, 0.05f);
            Rigidbody rb = fallbackSpear.AddComponent<Rigidbody>();
            rb.AddForce(throwDirection * throwPower, ForceMode.Impulse);
            Destroy(fallbackSpear, 5.0f); // Auto destroy after 5 seconds
        }
    }
}
