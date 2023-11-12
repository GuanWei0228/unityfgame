using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {FreeRoam, Battle}

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Canvas menu;
    [SerializeField] VirtualJoystick virtualJoystick;
    [SerializeField] Canvas map;
    [SerializeField] Camera worldCamera;

    GameState state;

    private void Start()
    {
        playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
    }
    
    void StartBattle()
    {
        
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);
        menu.gameObject.SetActive(false);
        map.gameObject.SetActive(false);
        virtualJoystick.OnPointerUp(null);
        battleSystem.StartBattle();
    }

    void EndBattle(bool won)
    {
        Time.timeScale = 1;
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(true);
        virtualJoystick.gameObject.SetActive(true);
        map.gameObject.SetActive(true);
    }
    
    private void Update()
    {

        if(state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
    }
}
