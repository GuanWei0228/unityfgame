using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs; // 存储不同种类的怪物预制体
    public Transform spawnPoint; // 生成怪物的位置
    public int yourMaxValue;
    private RoomGenerator roomGenerator;
    private Transform playerTransform; // 主角的Transform组件
    private float minDistance = 2.0f; // 生成怪物与主角的最小距离

    private void Start()
    {
        roomGenerator = FindObjectOfType<RoomGenerator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 通过标签找到主角
        SpawnMonsters();
    }

    private void SpawnMonsters()
    {
        int maxMonsters = Random.Range(1, yourMaxValue + 1); // 最大生成怪物数量
        int generatedRoomCount = roomGenerator.rooms.Count;

        for (int i = 0; i < maxMonsters; i++)
        {
            // 从怪物预制体数组中随机选择一种怪物
            int randomMonsterIndex = Random.Range(0, monsterPrefabs.Length);
            GameObject selectedMonsterPrefab = monsterPrefabs[randomMonsterIndex];

            // 随机生成怪物的位置，确保与主角的距离大于 minDistance
            Vector3 randomSpawnPosition = GetRandomSpawnPosition();

            // 实例化怪物并放置在生成位置
            GameObject spawnedMonster = Instantiate(selectedMonsterPrefab, randomSpawnPosition, Quaternion.identity);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // 取得房間的 Collider 范圍
        Collider2D roomCollider = GetComponent<Collider2D>();
        Vector2 roomMin = roomCollider.bounds.min;
        Vector2 roomMax = roomCollider.bounds.max;

        // 設定生成位置的緩衝區，避免怪物生成在邊界上
        float buffer = 1.0f;

        Vector3 randomSpawnPosition = Vector3.zero;

        // 循环直到找到满足条件的位置
        while (true)
        {
            // 隨機生成怪物的位置在房間內
            float randomX = Random.Range(roomMin.x + buffer, roomMax.x - buffer);
            float randomY = Random.Range(roomMin.y + buffer, roomMax.y - buffer);

            randomSpawnPosition = new Vector3(randomX, randomY, transform.position.z);

            // 检查与主角的距离
            float distanceToPlayer = Vector3.Distance(randomSpawnPosition, playerTransform.position);

            if (distanceToPlayer > minDistance)
            {
                // 距离满足条件，退出循环
                break;
            }
        }

        return randomSpawnPosition;
    }
}
