using UnityEngine;
using System.Collections;

public class CPUController : MonoBehaviour
{
    [Header("移動・ターゲット設定")]
    public float moveSpeed = 3.0f;       // プレイヤーより少し遅めにするのがおすすめ
    public Transform target;             // 追いかける対象（プレイヤー）
    public float attackRange = 2.0f;     // 攻撃を始める距離
    public float stopRange = 1.5f;       // これ以上プレイヤーに近づかない距離

    [Header("武器アニメーション設定")]
    public Transform weaponPivot;
    public float swingDuration = 0.2f;
    public float swingAngle = 90f;

    [Header("剣の当たり判定設定")]
    [SerializeField] private Collider swordCollider;

    [Header("ガード設定")]
    public Vector3 guardAngle = new Vector3(0, 0, 90f);
    public bool isGuarding = false;

    private bool isSwinging = false;
    private Quaternion initialWeaponRotation;

    // CPU独自の思考用タイマー変数
    private float attackCooldown = 2.0f; // 攻撃と攻撃の間の待ち時間
    private float lastAttackTime = 0f;

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
    }

    void Update()
    {
        // ターゲットが設定されていなければ動かない
        if (target == null) return;

        // プレイヤーとの距離を計算
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        LookAtTarget();
        MoveToTarget(distanceToTarget);
        DecideAction(distanceToTarget);
    }

    // 常にプレイヤーの方向を向く
    void LookAtTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0; // 上下方向に傾かないようにする

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            // 滑らかに振り向かせる
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    // プレイヤーに近づく
    void MoveToTarget(float distance)
    {
        // 攻撃中でなければ移動
        if (!isSwinging)
        {
            if (distance > stopRange)
            {
                // プレイヤーに向かう方向ベクトルを直接計算（Y軸の高さは無視）
                Vector3 direction = (target.position - transform.position).normalized;
                direction.y = 0;

                // ワールド空間基準でプレイヤーの方向へ直線移動する
                transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
            }
        }
    }

    // 距離と時間に応じて攻撃かガードか決める
    void DecideAction(float distance)
    {
        if (isSwinging) return;

        // プレイヤーが攻撃範囲内にいて、かつクールダウンが終わっていたら攻撃！
        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            isGuarding = false; // 攻撃時はガードを解く
            StartCoroutine(SwingWeapon());
            lastAttackTime = Time.time;
        }
        // 攻撃待ち時間中で、なおかつプレイヤーが近い場合はガードして身を守る
        else if (distance <= attackRange)
        {
            HandleGuard(true);
        }
        // プレイヤーが遠い場合はガードを解いて近づく
        else
        {
            HandleGuard(false);
        }
    }

    // ガードのアニメーション処理（プレイヤーと同じ）
    void HandleGuard(bool shouldGuard)
    {
        if (isSwinging) return;

        isGuarding = shouldGuard;

        if (isGuarding)
        {
            Quaternion targetRotation = initialWeaponRotation * Quaternion.Euler(guardAngle);
            weaponPivot.localRotation = Quaternion.Lerp(weaponPivot.localRotation, targetRotation, Time.deltaTime * 15f);
        }
        else
        {
            weaponPivot.localRotation = Quaternion.Lerp(weaponPivot.localRotation, initialWeaponRotation, Time.deltaTime * 15f);
        }
    }

    // 攻撃処理（プレイヤーと全く同じ）
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