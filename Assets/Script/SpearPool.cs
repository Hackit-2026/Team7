using System.Collections.Generic;
using UnityEngine;

public class SpearPool : MonoBehaviour
{
    public static SpearPool Instance;

    [Header("プールの設定")]
    [SerializeField] private GameObject spearPrefab;
    [SerializeField] private int initialPoolSize = 10; // 最初から用意しておく数

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject spear = Instantiate(spearPrefab);
            spear.SetActive(false);
            pool.Enqueue(spear);
        }
    }

    // 槍をプールから取り出す
    public GameObject GetSpear(Vector3 position, Quaternion rotation)
    {
        GameObject spear;

        if (pool.Count > 0)
        {
            spear = pool.Dequeue();
        }
        else
        {
            // 足りなくなった場合は新しく生成する
            spear = Instantiate(spearPrefab);
        }

        spear.transform.position = position;
        spear.transform.rotation = rotation;
        spear.SetActive(true);

        // 槍側のリセット処理を呼び出す
        SpearBehavior behavior = spear.GetComponent<SpearBehavior>();
        if (behavior != null)
        {
            behavior.ResetSpear();
        }

        return spear;
    }

    // 槍をプールに戻す
    public void ReturnSpear(GameObject spear)
    {
        spear.SetActive(false);
        pool.Enqueue(spear);
    }
}