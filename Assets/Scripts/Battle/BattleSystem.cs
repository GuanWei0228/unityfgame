using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEditor.Search;
using Firebase.Extensions;
using UnityEngine.UI;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy}

public class BattleSystem : MonoBehaviour
{
    DatabaseReference reference;

    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;

    public event Action<bool> OnBattleOver;

    public string rann;
    public int sel;

    BattleState state;
    int currentAction;
    int currentMove;
    int currentBack;

    public void StartBattle()
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        rann = UnityEngine.Random.Range(1, 100).ToString();
        playerUnit.Setup();
        enemyUnit.Setup();
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);

        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);

        //yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appeared.");
        yield return dialogBox.TypeDialog($"�J���Ǫ�!");
        yield return new WaitForSeconds(1f);

        PlayerAction();
    }

    void PlayerAction()
    {
       
        //rann = UnityEngine.Random.Range(1, 5).ToString();
        state = BattleState.PlayerAction;

        //dialogBox.TypeDialog("");
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            reference = FirebaseDatabase.DefaultInstance.RootReference;

            reference.Child("QAQ").Child("Q").Child(rann).GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted)
                {
                    // Handle the error...
                }
                if (task.IsCompletedSuccessfully)
                {
                    DataSnapshot snapshot = task.Result;
                    string dialogMessage = snapshot.Value.ToString();

                    string[] lines = dialogMessage.Split('\n');

                    StartCoroutine(ShowDialogLines(lines));
                    //StartCoroutine(dialogBox.TypeDialog(dialogMessage));
                    //dialogBox.EnableActionSelector(true);
                }
            });

        });
        

    }

    public string GetRann()
    {
        return rann;
    }

    IEnumerator ShowDialogLines(string[] lines)
    {
        int t = 0;
        foreach (string line in lines)
        {
            yield return StartCoroutine(dialogBox.TypeDialog(line));
            t++;
        }
        if (t > 0) {
            dialogBox.EnableActionSelector(true);
        }

    }

    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;

        var move = playerUnit.Pokemon.Moves[currentMove];
        //yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} used {move.Base.Name}");
        yield return dialogBox.TypeDialog("����A�i������I");
        yield return new WaitForSeconds(1f);

        bool isFainted = enemyUnit.Pokemon.EnemyTakeDamage(move, playerUnit.Pokemon);
       yield return enemyHud.UpdateHP();

        if (isFainted)
        {
            //yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} Fainted");
            yield return dialogBox.TypeDialog("���˩��~�I");
            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;

        var move = enemyUnit.Pokemon.GetRandomMove();
        //yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} used {move.Base.Name}");
        yield return dialogBox.TypeDialog("�Ǫ�����");
        yield return new WaitForSeconds(1f);

        bool isFainted = playerUnit.Pokemon.MeTakeDamage(move, playerUnit.Pokemon);
        yield return playerHud.UpdateHP();

        if (isFainted)
        {
            yield return dialogBox.TypeDialog("�A��F");
            yield return new WaitForSeconds(2f);
            OnBattleOver(false);
        }
        else
        {
            PlayerAction();
        }
    }

   
    public void HandleUpdate()
    {
        if(state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if(state == BattleState.PlayerMove)
        {
            HandleMoveSelection();  
            HandleBackSelection();
        }
    }
    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < 1)
                ++currentAction;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction > 0)
                --currentAction;
        }

        dialogBox.UpdateActionSelection(currentAction);

        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(currentAction == 0)
            {
                PlayerMove();
            }
            else if(currentAction == 1)
            {

            }
        }
    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove < playerUnit.Pokemon.Moves.Count - 1)
                ++currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove > 0)
                --currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMove < playerUnit.Pokemon.Moves.Count - 2)
                currentMove += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMove > 1)
                currentMove -= 2;
        }

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            sel = currentMove;
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }

        /*if (Input.GetKeyDown(KeyCode.X))
        {
            sel = currentMove;
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            PlayerAction();
        }*/
    }

    void HandleBackSelection()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            currentBack++;
        }
        else if (currentBack > 0)
        {
            if (Input.GetKeyDown(KeyCode.T))
                --currentBack;
        }

        dialogBox.UpdateBackSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (currentBack == 0)
            {
                dialogBox.EnableMoveSelector(false);
                dialogBox.EnableDialogText(true);
                PlayerAction();
            }
            else {
            
            }
            
        }
    }

}
