using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs; // 存储不同种类的怪物预制体
    public Transform spawnPoint; // 生成怪物的位置
    public int yourMaxValue = 1;
    private RoomGenerator roomGenerator;

    private void Start()
    {
        roomGenerator = FindObjectOfType<RoomGenerator>();

        for (int i = 0; i < yourMaxValue; i++)
        {
            // 从怪物预制体数组中随机选择一种怪物
            int randomMonsterIndex = Random.Range(0, monsterPrefabs.Length);
            GameObject selectedMonsterPrefab = monsterPrefabs[randomMonsterIndex];

            // 实例化怪物并放置在生成位置
            GameObject spawnedBoss = Instantiate(selectedMonsterPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
