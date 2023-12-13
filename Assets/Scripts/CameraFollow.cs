using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private Transform playerTransform;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // 在这里获取玩家的Transform，你可以根据实际情况获取
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            // 保持相机的z轴位置
            Vector3 desiredPosition = new Vector3(playerTransform.position.x + offset.x, playerTransform.position.y + offset.y, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(playerTransform.position);
        }
    }
}
