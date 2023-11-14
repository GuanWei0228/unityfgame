using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed; // 敌人的移动速度
    public float detectionRange; // 玩家探测范围
    public float stoppingDistance; // 距离玩家多远时停止移动
    public float followDelay; // 停留在探测范围内一定时间后开始跟随
    public Transform playerTransform; // 玩家的Transform组件
    private float followTimer; // 跟随计时器
    private bool shouldMove = true; // 是否应该移动
    public Vector3 initialPosition; // 初始位置

    [SerializeField] PokemonBase monsterBase;

    public PokemonBase GetPokemonBase()
    {
        return monsterBase;
    }

    private void Start()
    {
        initialPosition = transform.position;
        // 使用FindObjectOfType查找玩家对象
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("找不到玩家对象，请确保玩家对象具有“Player”标签。");
        }

    }

    private void Update()
    {
        // 检测玩家是否在探测范围内
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (shouldMove && distanceToPlayer <= detectionRange)
        {


            // 开始计时
            followTimer += Time.deltaTime;

            // 如果跟随计时器达到设定时间
            if (followTimer >= followDelay)
            {
                // 如果玩家在探测范围内，就朝向玩家移动
                Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
                transform.Translate(directionToPlayer * moveSpeed * Time.deltaTime);

                // 如果距离玩家够近，停止移动
                if (distanceToPlayer <= stoppingDistance)
                {
                    moveSpeed = 0;
                    shouldMove = false;
                    return;
                }
            }
        }
        else
        {
            // 如果玩家不在探测范围内，重置计时器和移动标志
            moveSpeed = 2f;
            followTimer = 0f;
            shouldMove = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 碰撞到玩家时停止移动
        if (collision.gameObject.CompareTag("Player"))
        {

            Destroy(gameObject);

        }
        else if (collision.gameObject.CompareTag("Door"))
        {
            transform.position = initialPosition;

        }
    }


}
