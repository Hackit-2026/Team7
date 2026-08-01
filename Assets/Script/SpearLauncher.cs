using UnityEngine;
using UnityEngine.InputSystem; // 新しいInput Systemを使うために追加

public class SpearLauncher : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private GameObject spearPrefab; // 槍のプレハブ
    [SerializeField] private Transform spawnPoint;   // 発射位置
    [SerializeField] private float throwPower = 20f; // 投擲の初速（威力）
    [SerializeField] private float throwAngle = 45f; // 発射角度（度）

    void Update()
    {
        // 新しいInput Systemでスペースキーが押されたかを判定
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ThrowSpear();
        }
    }

    void ThrowSpear()
    {
        if (spearPrefab == null || spawnPoint == null) return;

        // 1. 槍の生成
        GameObject spear = Instantiate(spearPrefab, spawnPoint.position, spawnPoint.rotation);

        // 2. Rigidbodyを取得して物理演算を有効化
        Rigidbody rb = spear.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            // 3. 発射方向と威力を計算
            Vector3 throwDirection = (spawnPoint.forward + Vector3.up * Mathf.Tan(throwAngle * Mathf.Deg2Rad)).normalized;

            // 4. 力を加える
            rb.AddForce(throwDirection * throwPower, ForceMode.Impulse);

            // 5. 槍の向きを進行方向に向かせる
            // ※Unity 6未満の場合は rb.velocity に書き換えてください
            rb.linearVelocity = throwDirection * throwPower; // 速度を直接設定する場合
            spear.transform.rotation = Quaternion.LookRotation(rb.linearVelocity);
        }
    }
}