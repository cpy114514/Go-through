using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;      // 玩家 Transform
    public float smoothSpeed = 0.125f; // 平滑速度
    public Vector3 offset;        // 摄像机与玩家的偏移

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
