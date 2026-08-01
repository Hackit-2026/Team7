using UnityEngine;

public class RopeController : MonoBehaviour
{
    [Header("縄の回転速度（度/秒）")]
    public float rotationSpeed = 360f;

    [Header("回転軸（通常はX軸かZ軸）")]
    public Vector3 rotationAxis = Vector3.right;

    void Update()
    {
        // 常に一定の速度で縄を回転させる
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
