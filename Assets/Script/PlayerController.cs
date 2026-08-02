using UnityEngine;
using UnityEngine.InputSystem; // 新しいInput Systemを使うために必要
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 5.0f;

    [Header("カメラ・視点設定")]
    [SerializeField] private Transform playerCamera; // プレイヤーの子階層にあるカメラ
    [SerializeField] private float mouseSensitivity = 2f; // マウスの感度
    [SerializeField] private float lookXLimit = 80f; // 上下の視点制限角度
    private float rotationX = 0f;

    [Header("武器アニメーション設定")]
    public Transform weaponPivot;
    public float swingDuration = 0.2f;
    public float swingAngle = 90f;

    [Header("剣の当たり判定設定")]
    [SerializeField] private Collider swordCollider;

    [Header("ガード設定")]
    public Vector3 guardAngle = new Vector3(-45f, 0, 90f); // 枝を横向きにする角度
    public bool isGuarding = false;

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
        Move();
        Look();
        HandleGuard();
        Attack();
    }

    void Move()
    {
        float x = 0f;
        float z = 0f;

        // キーボード（WASD）の入力を直接取得
        if (Keyboard.current != null)
        {
            if (Keyboard.current.dKey.isPressed) x += 1f;
            if (Keyboard.current.aKey.isPressed) x -= 1f;
            if (Keyboard.current.wKey.isPressed) z += 1f;
            if (Keyboard.current.sKey.isPressed) z -= 1f;
        }

        Vector3 inputDir = new Vector3(x, 0, z).normalized;

        if (inputDir != Vector3.zero)
        {
            // カメラの向きを基準にした移動方向を計算（カメラが向いている水平方向に進む）
            Vector3 forward = playerCamera != null ? playerCamera.forward : transform.forward;
            Vector3 right = playerCamera != null ? playerCamera.right : transform.right;
            forward.y = 0f; // 上下方向の移動を無効化
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 moveDirection = (forward * inputDir.z + right * inputDir.x).normalized;

            // 移動
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    void Look()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null) return;

        // マウスの移動量を取得
        Vector2 mouseDelta = mouse.delta.ReadValue() * mouseSensitivity * 0.1f;

        // 上下の視点移動（カメラのみを回転）
        rotationX -= mouseDelta.y;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        }

        // 左右の視点移動（プレイヤー本体を回転させる）
        transform.Rotate(Vector3.up * mouseDelta.x);
    }

    void HandleGuard()
    {
        // 攻撃中（剣を振っている最中）はガードの動作をしない
        if (isSwinging) return;

        // 右クリックが押されているか判定
        if (Mouse.current != null && Mouse.current.rightButton.isPressed)
        {
            isGuarding = true;

            // 指定した角度（横向き）に滑らかに回転させる
            Quaternion targetRotation = initialWeaponRotation * Quaternion.Euler(guardAngle);
            weaponPivot.localRotation = Quaternion.Lerp(weaponPivot.localRotation, targetRotation, Time.deltaTime * 15f);
        }
        else
        {
            isGuarding = false;

            // 攻撃していない＆ガードしていない時は、滑らかに元の角度に戻す
            weaponPivot.localRotation = Quaternion.Lerp(weaponPivot.localRotation, initialWeaponRotation, Time.deltaTime * 15f);
        }
    }

    void Attack()
    {
        // スペースキーが押された瞬間を判定[cite: 1]
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            //Debug.Log("スペース");
            if (!isSwinging && weaponPivot != null)
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

        // 振り下ろす
        while (elapsedTime < swingDuration / 2)
        {
            weaponPivot.localRotation = Quaternion.Slerp(initialWeaponRotation, targetRotation, elapsedTime / (swingDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 元に戻す
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