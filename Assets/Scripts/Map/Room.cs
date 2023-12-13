using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public GameObject doorLeft, doorRight, doorUp, doorDown;//用来存放各个位置的门

    public bool roomLeft, roomRight, roomUp, roomDown;//判断上下左右是否有房间

    public Text textnumber;
    public int stepToStart;//距离初始点的网格距离
    public int doorNumber;

    private bool playerEntered = false; // 用于追踪玩家是否进入房间
    private float yourRadius = 1f;

    void Start()
    {
    }

    void Update()
    {

        // 如果玩家进入房间
        if (playerEntered)
        {
                //bool hasMonsters = HasMonsters();
                //print(hasMonsters);
                doorLeft.SetActive(roomLeft);
                doorRight.SetActive(roomRight);
                doorUp.SetActive(roomUp);
                doorDown.SetActive(roomDown);

        }
        else
        {
            // 如果玩家离开房间，关闭门
            doorLeft.SetActive(false);
            doorRight.SetActive(false);
            doorUp.SetActive(false);
            doorDown.SetActive(false);
        }
    }

    public void UpdateRoom(float xOffset, float yOffset)
    {
        //计算距离初始点的网格距离
        stepToStart = (int)(Mathf.Abs(transform.position.x / xOffset) + Mathf.Abs(transform.position.y / yOffset));

        string text = stepToStart.ToString();
        textnumber.text = text;

        if (roomUp)
            doorNumber++;
        if (roomDown)
            doorNumber++;
        if (roomLeft)
            doorNumber++;
        if (roomRight)
            doorNumber++;
    }

    // 检查房间内是否有怪物
    private bool HasMonsters()
    {
        Collider2D roomCollider = GetComponent<Collider2D>();

        if (roomCollider != null)
        {
            // 取得房間的大小
            Vector2 roomSize = roomCollider.bounds.size;

            // 設定矩形的中心為房間的位置，大小為房間的大小
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, roomSize, 0f);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Monster"))
                {
                    return true;
                }
            }
        }

        return false;
    }


    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerEntered = true;
            CamerController.instance.ChangeTarget(transform);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerEntered = false; // 玩家离开房间
        }
    }
}
