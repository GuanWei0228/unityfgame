using System.Collections;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public Transform playerTransform; // 玩家的Transform组件
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
