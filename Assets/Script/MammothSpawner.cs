using System.Collections;
using UnityEngine;

public class MammothSpawner : MonoBehaviour
{
    [Header("生成するプレハブ")]
    [SerializeField] private GameObject mammothPrefab;

    [Header("基準となる中心位置（未設定の場合はこのオブジェクトの位置）")]
    [SerializeField] private Transform centerTarget;

    [Header("スポーン間隔（秒）")]
    [SerializeField] private float spawnInterval = 10f;

    [Header("距離の範囲（15〜20）")]
    [SerializeField] private float minDistance = 15f;
    [SerializeField] private float maxDistance = 20f;

    [Header("高さ（Y座標）の調整")]
    [SerializeField] private float spawnYPosition = 0f;

    void Start()
    {
        // 基準ターゲットが未設定なら自分自身を基準にする
        if (centerTarget == null)
        {
            centerTarget = transform;
        }

        // 一定時間ごとに生成を繰り返すコルーチンを開始
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // 指定した秒数待つ
            yield return new WaitForSeconds(spawnInterval);

            // マンモスを生成
            SpawnMammoth();
        }
    }

    void SpawnMammoth()
    {
        if (mammothPrefab == null)
        {
            Debug.LogWarning("Mammoth Prefabが設定されていません。");
            return;
        }

        // ランダムな位置を計算
        Vector3 spawnPos = GetRandomSpawnPosition();

        // マンモスを生成（回転はデフォルト）
        Instantiate(mammothPrefab, spawnPos, Quaternion.identity);
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 centerPos = centerTarget.position;

        // XとZのオフセット（距離15〜20）をランダムに決定
        float offsetX = Random.Range(minDistance, maxDistance);
        float offsetZ = Random.Range(minDistance, maxDistance);

        // ランダムでプラスマイナスを反転させる（全方位に散らばらせるため）
        if (Random.value > 0.5f) offsetX *= -1f;
        if (Random.value > 0.5f) offsetZ *= -1f;

        float targetX = centerPos.x + offsetX;
        float targetZ = centerPos.z + offsetZ;

        // 高さは基準位置、または固定のY座標を使用
        return new Vector3(targetX, centerPos.y + spawnYPosition, targetZ);
    }
}