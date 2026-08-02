using UnityEngine;

public class SpearBehavior : MonoBehaviour
{
    private Rigidbody rb;
    [Header("当たらなかった場合にプールに戻す時間（秒）")]
    [SerializeField] private float lifeTime = 5f;
    private float lifeTimer;
    private bool isThrown = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // プールから取り出されたときに呼ばれる
    public void ResetSpear()
    {
        isThrown = true;
        lifeTimer = lifeTime;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // 親子関係を解除（マンモスの子になっていた場合を考慮）
        transform.SetParent(null);
    }

    void Update()
    {
        if (!isThrown) return;

        // 一定時間当たらなかったら自動でプールに戻す
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            ReturnToPool();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // マンモスや地面などに当たったときの処理
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Mammoth"))
        {
            isThrown = false;

            if (rb != null)
            {
                rb.isKinematic = true; // 止める
            }

            // マンモスに当たった場合は親にする（従来通り）
            if (collision.gameObject.CompareTag("Mammoth"))
            {
                transform.SetParent(collision.transform);
            }
        }
    }

    public void ReturnToPool()
    {
        isThrown = false;
        if (SpearPool.Instance != null)
        {
            SpearPool.Instance.ReturnSpear(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}