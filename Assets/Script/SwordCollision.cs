using UnityEngine;

public class SwordCollision : MonoBehaviour
{
    public int Damage = 0;

    // コライダーが他のコライダー（IsTrigger）に触れた瞬間に呼ばれる
    void OnTriggerEnter(Collider other)
    {
        // 触れたオブジェクトのTagが "Head" だった場合
        if (other.gameObject.CompareTag("Head"))
        {
            Debug.Log("CPUの【頭】に剣が当たりました！ (ダメージ２)");
            Damage = Damage + 2;
            Debug.Log(Damage);
        }
        // 触れたオブジェクトのTagが "Body" だった場合
        else if (other.gameObject.CompareTag("Body"))
        {
            Debug.Log("CPUの【体】に剣が当たりました！(ダメージ１)");
            Damage = Damage + 1;
            Debug.Log(Damage);
        }
    }
}