using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;

    [Header("VR Camera Setup")]
    [SerializeField] private Transform centerEyeAnchor; // Assign CenterEyeAnchor

    [Header("Weapon Animation Settings")]
    public Transform weaponPivot;
    public float swingDuration = 0.2f;
    public float swingAngle = 90f;

    [Header("Sword Collision Setup")]
    [SerializeField] private Collider swordCollider;

    [Header("Guard Settings")]
    public Vector3 guardAngle = new Vector3(-45f, 0, 90f);
    public bool isGuarding = false;

    [Header("VR Swing Detection")]
    [Tooltip("Speed threshold to detect a controller swing (higher = needs harder swing)")]
    [SerializeField] private float swingThresholdSpeed = 2.5f;

    private bool isSwinging = false;
    private Quaternion initialWeaponRotation;

    void Start()
    {
        if (weaponPivot != null)
        {
            initialWeaponRotation = weaponPivot.localRotation;
        }

        if (swordCollider != null)
        {
            swordCollider.enabled = false;
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
    }

    void Update()
    {
        Move();
        HandleGuard();
        Attack();
    }

    void Move()
    {
        // Get Left Joystick input from Meta Quest Controller
        Vector2 inputDir = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);

        if (inputDir != Vector2.zero)
        {
            // Calculate movement direction relative to the VR Camera (CenterEyeAnchor)
            Vector3 forward = centerEyeAnchor != null ? centerEyeAnchor.forward : transform.forward;
            Vector3 right = centerEyeAnchor != null ? centerEyeAnchor.right : transform.right;

            forward.y = 0f; // Disable vertical movement
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 moveDirection = (forward * inputDir.y + right * inputDir.x).normalized;

            // Move the player object
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    void HandleGuard()
    {
        if (isSwinging) return;

        // Check if Right Index Trigger (or Left) is pressed
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) || OVRInput.Get(OVRInput.RawButton.LIndexTrigger))
        {
            isGuarding = true;

            Quaternion targetRotation = initialWeaponRotation * Quaternion.Euler(guardAngle);
            weaponPivot.localRotation = Quaternion.Lerp(weaponPivot.localRotation, targetRotation, Time.deltaTime * 15f);
        }
        else
        {
            isGuarding = false;

            // Return to initial rotation smoothly
            weaponPivot.localRotation = Quaternion.Lerp(weaponPivot.localRotation, initialWeaponRotation, Time.deltaTime * 15f);
        }
    }

    void Attack()
    {
        if (isSwinging || isGuarding) return;

        // Get the current velocity (speed) of the Right and Left Controllers
        Vector3 rightControllerVelocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
        Vector3 leftControllerVelocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);

        // Calculate total speed of the physical swing
        float rightSwingSpeed = rightControllerVelocity.magnitude;
        float leftSwingSpeed = leftControllerVelocity.magnitude;

        // Trigger attack animation if player swings either controller hard enough
        if (rightSwingSpeed > swingThresholdSpeed || leftSwingSpeed > swingThresholdSpeed)
        {
            if (weaponPivot != null)
            {
                StartCoroutine(SwingWeapon());
            }
        }
    }

    IEnumerator SwingWeapon()
    {
        isSwinging = true;

        if (swordCollider != null)
        {
            swordCollider.enabled = true;

            SwordCollision collisionScript = swordCollider.GetComponent<SwordCollision>();
            if (collisionScript != null)
            {
                collisionScript.ResetHit();
            }
        }

        float elapsedTime = 0f;
        Quaternion targetRotation = initialWeaponRotation * Quaternion.Euler(swingAngle, 0, 0);

        while (elapsedTime < swingDuration / 2)
        {
            weaponPivot.localRotation = Quaternion.Slerp(initialWeaponRotation, targetRotation, elapsedTime / (swingDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < swingDuration / 2)
        {
            weaponPivot.localRotation = Quaternion.Slerp(targetRotation, initialWeaponRotation, elapsedTime / (swingDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        weaponPivot.localRotation = initialWeaponRotation;

        if (swordCollider != null)
        {
            swordCollider.enabled = false;
        }

        isSwinging = false;
    }
}
