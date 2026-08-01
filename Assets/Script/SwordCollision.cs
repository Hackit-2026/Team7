using UnityEngine;

public class SwordCollision : MonoBehaviour
{
    public int Damage = 0;

    private bool hasHit = false;
    private Collider swordCollider; // 【追加】自身のコライダーを保持する変数

    void Start()
    {
        // 自身のコライダーを取得
        swordCollider = GetComponent<Collider>();
    }

    // コライダーが他のコライダー（IsTrigger）に触れた瞬間に呼ばれる
    void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        // 触れたオブジェクトのTagが "Head" だった場合
        if (other.gameObject.CompareTag("Head"))
        {
            Debug.Log("CPUの【頭】に剣が当たりました！ (ダメージ２)");
            Damage = Damage + 2;
            Debug.Log(Damage);
            hasHit = true;

            // 【修正点】当たった瞬間にコライダーをオフにして多重ヒット・それ以降のブロックを防ぐ
            if (swordCollider != null) swordCollider.enabled = false;
        }
        // 触れたオブジェクトのTagが "Body" だった場合
        else if (other.gameObject.CompareTag("Body"))
        {
            Debug.Log("CPUの【体】に剣が当たりました！(ダメージ１)");
            Damage = Damage + 1;
            Debug.Log(Damage);
            hasHit = true;

            // 【修正点】当たった瞬間にコライダーをオフにする
            if (swordCollider != null) swordCollider.enabled = false;
        }
    }

    void OnEnable()
    {
        hasHit = false;
    }
}