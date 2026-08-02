using UnityEngine;

public class MammothTarget : MonoBehaviour
{
    [Header("獲得ポイント")]
    [SerializeField] private int scoreValue = 100; // このマンモスを倒したときのポイント

    [Header("耐久値（ヒットポイント）")]
    [SerializeField] private int maxHitCount = 5; // 何回当てたら倒せるか
    private int currentHitCount;

    void Start()
    {
        // 初期状態のヒットポイントを設定
        currentHitCount = maxHitCount;
    }

    // 槍（Collider）が当たったときの処理
    private void OnCollisionEnter(Collision collision)
    {
        // ぶつかってきたオブジェクトが「Spear（槍）」かどうかを判定
        if (collision.gameObject.CompareTag("Spear"))
        {
            currentHitCount--;
            Debug.Log($"マンモスに槍が当たった！ 残り耐久値: {currentHitCount}/{maxHitCount}");

            // 当たった槍の動きを止めてマンモスの子供にする（刺さる演出）
            Rigidbody spearRb = collision.gameObject.GetComponent<Rigidbody>();
            if (spearRb != null)
            {
                spearRb.isKinematic = true;
                collision.gameObject.transform.SetParent(transform);
            }

            // 耐久値が0以下になったらポイント加算してマンモスを消す
            if (currentHitCount <= 0)
            {
                Debug.Log($"マンモスを倒した！ +{scoreValue}ポイント");

                // TODO: スコアマネージャー等にポイントを加算する処理
                // if (ScoreManager.Instance != null) { ScoreManager.Instance.AddScore(scoreValue); }

                // マンモスを消去（子オブジェクトとして刺さっている槍も一緒に消えます）
                Destroy(gameObject);
            }
        }

        // マンモスに当たったときの処理内
        SpearBehavior spearBehavior = collision.gameObject.GetComponent<SpearBehavior>();
        if (spearBehavior != null)
        {
            // 刺さった演出にする場合は親を変えてからタイマーを止める等、
            // あるいはそのままプールに戻す場合は以下のように呼び出せます
            // spearBehavior.ReturnToPool();
        }
    }
}