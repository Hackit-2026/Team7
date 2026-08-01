using UnityEngine;
using UnityEngine.InputSystem; // 新しいInput Systemを使うために必要
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 5.0f;

    [Header("武器アニメーション設定")]
    public Transform weaponPivot;
    public float swingDuration = 0.2f;
    public float swingAngle = 90f;

    private bool isSwinging = false;
    private Quaternion initialWeaponRotation;

    void Start()
    {
        if (weaponPivot != null)
        {
            initialWeaponRotation = weaponPivot.localRotation;
        }
    }

    void Update()
    {
        Move();
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

        Vector3 moveDirection = new Vector3(x, 0, z).normalized;

        if (moveDirection != Vector3.zero)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
            transform.forward = moveDirection;
        }
    }

    void Attack()
    {
        // スペースキーが押された瞬間を判定
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("スペース");
            if (!isSwinging && weaponPivot != null)
            {
                StartCoroutine(SwingWeapon());
            }
        }
    }

    IEnumerator SwingWeapon()
    {
        isSwinging = true;

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
        isSwinging = false;
    }
}