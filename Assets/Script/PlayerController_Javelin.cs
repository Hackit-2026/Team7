using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController_Javelin : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("視点操作設定")]
    [SerializeField] private Transform playerCamera; // プレイヤーの子にあるカメラを指定[cite: 2]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float lookXLimit = 80f; // 見上げる/見下ろすの最大角度[cite: 2]

    [Header("槍の投擲設定")]
    [SerializeField] private GameObject spearPrefab; // 槍のプレハブ
    [SerializeField] private Transform spawnPoint;   // 槍の生成位置（手元など）
    [SerializeField] private float throwPower = 20f; // 投擲の初速（威力）

    private CharacterController controller;
    private float rotationX = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // カメラが未設定なら、子オブジェクトから自動取得[cite: 2]
        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }

        // ゲーム開始時にマウスカーソルを画面中央にロックして非表示にする[cite: 2]
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleLook();
        HandleThrow(); // 槍を投げる処理を追加
    }

    // 移動処理（WASD / 矢印キー）[cite: 2]
    void HandleMovement()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        float x = 0f;
        float z = 0f;

        if (keyboard.wKey.isPressed) z += 1f;
        if (keyboard.sKey.isPressed) z -= 1f;
        if (keyboard.aKey.isPressed) x -= 1f;
        if (keyboard.dKey.isPressed) x += 1f;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded == false)
        {
            controller.Move(Vector3.down * 9.81f * Time.deltaTime);
        }
    }

    // 視点操作処理（マウス移動によるカメラとキャラクターの回転）[cite: 2]
    void HandleLook()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null) return;

        Vector2 mouseDelta = mouse.delta.ReadValue() * mouseSensitivity * 0.1f;

        rotationX -= mouseDelta.y;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        }

        transform.Rotate(Vector3.up * mouseDelta.x);
    }

    // スペースキーで槍を投げる処理
    void HandleThrow()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            ThrowSpear();
        }
    }

    // 槍の生成と発射
    void ThrowSpear()
    {
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position + transform.forward;

        // カメラの水平方向の向きを計算
        Vector3 flatForward = playerCamera != null ? playerCamera.forward : transform.forward;
        flatForward.y = 0f;
        flatForward.Normalize();

        Quaternion spawnRot = Quaternion.identity;
        if (flatForward != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(flatForward);
            spawnRot = lookRot * Quaternion.Euler(90, 0, 0); // プレハブのオフセット（例: X90度）を考慮
        }

        // 【変更】Instantiate の代わりにオブジェクトプールから取得する
        GameObject spear = SpearPool.Instance.GetSpear(spawnPos, spawnRot);

        Rigidbody rb = spear.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 throwDirection = playerCamera != null ? playerCamera.forward : transform.forward;
            rb.AddForce(throwDirection * throwPower, ForceMode.Impulse);
            rb.linearVelocity = throwDirection * throwPower;
        }
    }
}