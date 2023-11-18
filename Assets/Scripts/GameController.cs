using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum GameState {FreeRoam, Battle}

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerLife life;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Canvas menu;
    [SerializeField] VirtualJoystick virtualJoystick;
    [SerializeField] Canvas minimap;
    [SerializeField] Canvas mapbutton;
    [SerializeField] Camera worldCamera;

    GameState state;
    private bool hasLost = false;

    private void Start()
    {
        playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
        battleSystem.Onwingame += WinGame;
    }


    void StartBattle()
    {
        
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);
        menu.gameObject.SetActive(false);
        mapbutton.gameObject.SetActive(false);
        minimap.gameObject.SetActive(false);
        virtualJoystick.OnPointerUp(null);
        battleSystem.StartBattle();
        
    }

    void EndBattle(bool won)
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(true);
        virtualJoystick.gameObject.SetActive(true);
        mapbutton.gameObject.SetActive(true);
    }
    void WinGame(bool won) 
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(true);
        virtualJoystick.gameObject.SetActive(true);
        mapbutton.gameObject.SetActive(true);
        print("win");
    }
    void LoseGame()
    {
        if (hasLost)
        {
            return; // 如果已经输掉，不再执行以下代码
        }

        hasLost = true; // 设置为 true，表示已经输掉
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(true);
        virtualJoystick.gameObject.SetActive(true);
        mapbutton.gameObject.SetActive(true);
        print("lose");
    }


    private void Update()
    {

        if(state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        if (life.playerHP == 0) 
        {
            LoseGame();
            return;
        }
    }
}
