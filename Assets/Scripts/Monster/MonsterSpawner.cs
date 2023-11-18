using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs; // 存储不同种类的怪物预制体
    public Transform spawnPoint; // 生成怪物的位置
    public int yourMaxValue;
    private RoomGenerator roomGenerator;
    private void Start()
    {
        roomGenerator = FindObjectOfType<RoomGenerator>();
        int maxMonsters = Random.Range(1, yourMaxValue + 1); // 最大生成怪物数量
        int generatedRoomCount = roomGenerator.rooms.Count;

            for (int i = 0; i < maxMonsters; i++)
            {
                // 从怪物预制体数组中随机选择一种怪物
                int randomMonsterIndex = Random.Range(0, monsterPrefabs.Length);
                GameObject selectedMonsterPrefab = monsterPrefabs[randomMonsterIndex];

                // 随机生成怪物的位置
                Vector3 randomSpawnPosition = GetRandomSpawnPosition();

                // 实例化怪物并放置在生成位置
                GameObject spawnedMonster = Instantiate(selectedMonsterPrefab, randomSpawnPosition, Quaternion.identity);
            }

        //print(monsterPrefabs.Length);


    }

    private Vector3 GetRandomSpawnPosition()
    {
        // 取得房間的 Collider 范圍
        Collider2D roomCollider = GetComponent<Collider2D>();
        Vector2 roomMin = roomCollider.bounds.min;
        Vector2 roomMax = roomCollider.bounds.max;

        // 設定生成位置的緩衝區，避免怪物生成在邊界上
        float buffer = 1.0f;

        // 隨機生成怪物的位置在房間內
        float randomX = Random.Range(roomMin.x + buffer, roomMax.x - buffer);
        float randomY = Random.Range(roomMin.y + buffer, roomMax.y - buffer);

        // 使用房間的 z 軸作為生成怪物的 z 軸
        float spawnZ = transform.position.z;

        return new Vector3(randomX, randomY, spawnZ);
    }

}
