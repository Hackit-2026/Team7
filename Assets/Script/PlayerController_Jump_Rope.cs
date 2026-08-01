using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_Jump_Rope : MonoBehaviour
{
    private Rigidbody rb;
    [Header("ジャンプの強さ")]
    public float jumpForce = 5f;

    [Header("接地判定用（オプション）")]
    public bool isGrounded = true;

    [Header("視点操作・カメラ設定")]
    [SerializeField] private Transform playerCamera; // プレイヤーの子階層にあるカメラ
    [SerializeField] private float mouseSensitivity = 2f; // マウス感度
    [SerializeField] private float lookXLimit = 80f; // 上下の視点制限角度
    private float rotationX = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // カメラが未設定の場合、自分自身の子オブジェクトから自動検索する
        if (playerCamera == null && GetComponentInChildren<Camera>() != null)
        {
            playerCamera = GetComponentInChildren<Camera>().transform;
        }

        // ゲーム開始時にマウスカーソルを画面中央にロックして非表示にする
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            Jump();
        }

        HandleLook();
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        isGrounded = false;
    }

    void HandleLook()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null) return;

        // マウスの移動量を取得
        Vector2 mouseDelta = mouse.delta.ReadValue() * mouseSensitivity * 0.1f;

        // 1. 上下の視点移動（カメラのみを上下に回転）
        rotationX -= mouseDelta.y;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        }

        // 2. 左右の視点移動（プレイヤー本体を左右に回転させることでカメラも一緒に追従する）
        transform.Rotate(Vector3.up * mouseDelta.x);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 床についたオブジェクトのタグが "Ground" の場合
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}