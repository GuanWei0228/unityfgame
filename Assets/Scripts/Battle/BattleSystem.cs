using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy }

public class BattleSystem : MonoBehaviour
{
    //DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
    [SerializeField] PlayerController player;
    [SerializeField] PlayerControllerPhone playerphone;
    [SerializeField] PlayerLife life;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;


    [SerializeField] Text questionTextUI;
    [SerializeField] Text answerTextUI;

    public event Action<bool> OnBattleOver;
    public event Action<bool> Onwingame;

    public event Action OnActionComplete; // 定義一個事件

    public int maxnumber;
    public string rann;
    public int sel;

    HashSet<string> usedRannSet = new HashSet<string>();//存rann
    List<string> wonQuestions = new List<string>(); //存正確回答題號
    List<string> lostQuestions = new List<string>();//存錯誤回答題號

    public List<Tuple<string, string>> questionAnswerPairs = new List<Tuple<string, string>>();//存取題目及答案

    BattleState state;
    int currentAction;
    int currentMove;
    int currentBack;

    public string answer;

    public string panelanswer;
    public string panelquestion;
    public bool selectanswer;
    string selectedMoveText;

    public void StartBattle()
    {
        StartCoroutine(SetupBattle());
        player.speed = 0f;
        playerphone.speed = 0f;
        //life.hpBar.GetComponent<Image>().fillAmount -= 0.55f;
        //print(life.playerHP);
    }

    public IEnumerator SetupBattle()
    {

        GetRann();
        //PrintResults(); //輸出wonQuestions lostQuestions
        enemyUnit.ChangeBase(player.test);
        playerUnit.Setup();
        enemyUnit.Setup();
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);
        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);


        yield return dialogBox.TypeDialog($"遇見{enemyUnit.Pokemon.Base.Name}!");
        yield return new WaitForSeconds(1f);
        yield return dialogBox.TypeDialog($"請根據問題選擇正確選項!");
        yield return new WaitForSeconds(1f);


        PlayerAction();
    }





    void PlayerAction()
    {
        string level = PlayerPrefs.GetString("PlayerName", "DefaultName");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        //rann = UnityEngine.Random.Range(1, 5).ToString();
        state = BattleState.PlayerAction;

        //dialogBox.TypeDialog("");

        reference.Child(level).Child("Ans").Child(rann).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            if (task.IsCompletedSuccessfully)
            {
                DataSnapshot snapshot = task.Result;
                answer = snapshot.Value.ToString();
            }
        });

        reference.Child(level).Child("Q").Child(rann).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            if (task.IsCompletedSuccessfully)
            {
                DataSnapshot snapshot = task.Result;
                panelquestion = snapshot.Value.ToString();

                string[] lines = panelquestion.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.None);

                StartCoroutine(ShowDialogLines(lines));
                //StartCoroutine(dialogBox.TypeDialog(dialogMessage));
                //dialogBox.EnableActionSelector(true);
            }
        });



    }


    public string GetRann()
    {
        // 生成新的 rann，確保不重複
        string newRann;

        if (usedRannSet.Count == maxnumber - 1 || player.Monstertag == "Boss")
        {
            // 如果所有數字都已經使用過，重新初始化集合
            usedRannSet.Clear();
        }
        do
        {
            newRann = UnityEngine.Random.Range(1, maxnumber + 1).ToString();
        } while (usedRannSet.Contains(newRann));

        // 將新的 rann 加入到使用過的 set 中
        usedRannSet.Add(newRann);

        rann = newRann;

        return rann;
    }

    IEnumerator ShowDialogLines(string[] lines)
    {
        int t = 0;
        int numLines = lines.Length;
        foreach (string line in lines)
        {
            yield return StartCoroutine(dialogBox.TypeDialog(line));
            t++;
        }
        if (t == numLines)
        {
            dialogBox.EnableActionSelector(true);
        }
        OnActionComplete?.Invoke();
    }






    public void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    public void BackSelection()
    {
        dialogBox.UpdateBackSelection(currentAction);
        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogText(true);
        PlayerAction();


    }

    IEnumerator PerformPlayerMove() //人物動作
    {
        state = BattleState.Busy;

        var move = playerUnit.Pokemon.Moves[currentMove];
        //yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} used {move.Base.Name}");
        if (selectanswer == true)
        {
            yield return dialogBox.TypeDialog($"答對，進行攻擊！");
            yield return new WaitForSeconds(0.6f);
        }
        else
        {
            yield return dialogBox.TypeDialog($"答錯了！");
            yield return new WaitForSeconds(0.6f);
        }
        yield return new WaitForSeconds(1f);
        questionAnswerPairs.Add(new Tuple<string, string>(panelquestion, answer));

        bool isFainted = enemyUnit.Pokemon.EnemyTakeDamage(move, playerUnit.Pokemon);
        yield return enemyHud.UpdateHP();

        if (isFainted == true && player.Monstertag == "Monster")
        {
            //yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} Fainted");
            yield return dialogBox.TypeDialog($"擊倒{enemyUnit.Pokemon.Base.Name}！");
            yield return new WaitForSeconds(1.5f);

            OnBattleOver(true);
            player.speed = 5f;
            playerphone.speed = 5f;
        }
        else if (isFainted == false && player.Monstertag == "Monster")
        {
            StartCoroutine(EnemyMove());
        }
        if (isFainted == true && player.Monstertag == "Boss")
        {
            //yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} Fainted");
            yield return dialogBox.TypeDialog($"擊倒{enemyUnit.Pokemon.Base.Name}！");
            yield return new WaitForSeconds(1.5f);

            Onwingame(true);
            player.speed = 5f;
            playerphone.speed = 5f;
        }
        else if (isFainted == false && player.Monstertag == "Boss")
        {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove() //怪物動作
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        state = BattleState.EnemyMove;

        var move = enemyUnit.Pokemon.GetRandomMove();
        //yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} used {move.Base.Name}");
        if (selectanswer == true)
        {
            
        }
        else
        {
            //yield return dialogBox.TypeDialog($"答錯！{enemyUnit.Pokemon.Base.Name}攻擊");
            //yield return new WaitForSeconds(1f);
        }

        bool isFainted = playerUnit.Pokemon.MeTakeDamage(move, playerUnit.Pokemon);
        yield return playerHud.UpdateHP();

        if (isFainted == true && player.Monstertag =="Monster")
        {
            yield return dialogBox.TypeDialog($"你輸了");
            yield return new WaitForSeconds(1.5f);

            life.playerHP -= 10f;
            life.hpBar.GetComponent<Image>().fillAmount -= 0.10f;
            OnBattleOver(false);
            player.speed = 5f;
            playerphone.speed = 5f;
        }
        else if(isFainted == false && player.Monstertag == "Monster")
        {
            //yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name}沒死亡，換一個題目！");
            //yield return new WaitForSeconds(0.8f);
            GetRann();
            dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);


            PlayerAction();
        }

        if (isFainted == true && player.Monstertag == "Boss")
        {
            yield return dialogBox.TypeDialog($"你輸了");
            yield return new WaitForSeconds(2f);

            life.playerHP -= 100f;
            life.hpBar.GetComponent<Image>().fillAmount -= 1f;
            OnBattleOver(false);
            player.speed = 5f;
            playerphone.speed = 5f;
        }
        else if (isFainted == false && player.Monstertag == "Boss")
        {
            //yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name}沒死亡，換一個題目！");
            //yield return new WaitForSeconds(2f);
            GetRann();
            dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);


            PlayerAction();
        }
    }


    void PrintResults()
    {
        Debug.Log("Won Questions:");
        foreach (var question in wonQuestions)
        {
            print(question);
        }

        Debug.Log("Lost Questions:");
        foreach (var question in lostQuestions)
        {
            print(question);
        }

        Debug.Log("Used Rann Set:");
        foreach (var usedRann in usedRannSet)
        {
            print(usedRann);
        }
    }

    public void HandleUpdate()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove)
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

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                PlayerMove();
            }
            else if (currentAction == 1)
            {

            }
        }
    }

    public void ButtonSelect1()
    {
        sel = 0;
        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogText(true);

        // 获取选择的招式的文本信息
        Text selectedMoveTextObject = dialogBox.MoveTexts[sel];
        string selectedMoveText = selectedMoveTextObject.text;

        // 在这里使用 selectedMoveText，你可以将其存储、打印或进行其他操作
        if (answer == selectedMoveText)
        {
            selectanswer = true;
            wonQuestions.Add(rann);
        }
        else
        {
            selectanswer = false;
            lostQuestions.Add(rann);
        }
        StartCoroutine(PerformPlayerMove());
    }

    public void ButtonSelect2()
    {
        sel = 1;
        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogText(true);

        // 获取选择的招式的文本信息
        Text selectedMoveTextObject = dialogBox.MoveTexts[sel];
        string selectedMoveText = selectedMoveTextObject.text;

        // 在这里使用 selectedMoveText，你可以将其存储、打印或进行其他操作
        if (answer == selectedMoveText)
        {
            selectanswer = true;
            wonQuestions.Add(rann);
        }
        else
        {
            selectanswer = false;
            lostQuestions.Add(rann);
        }

        StartCoroutine(PerformPlayerMove());
    }

    public void ButtonSelect3()
    {
        sel = 2;
        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogText(true);

        // 获取选择的招式的文本信息
        Text selectedMoveTextObject = dialogBox.MoveTexts[sel];
        string selectedMoveText = selectedMoveTextObject.text;

        // 在这里使用 selectedMoveText，你可以将其存储、打印或进行其他操作
        if (answer == selectedMoveText)
        {
            selectanswer = true;
            wonQuestions.Add(rann);
        }
        else
        {
            selectanswer = false;
            lostQuestions.Add(rann);
        }
        StartCoroutine(PerformPlayerMove());
    }

    public void ButtonSelect4()
    {
        sel = 3;
        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogText(true);

        // 获取选择的招式的文本信息
        Text selectedMoveTextObject = dialogBox.MoveTexts[sel];
        string selectedMoveText = selectedMoveTextObject.text;

        // 在这里使用 selectedMoveText，你可以将其存储、打印或进行其他操作
        if (answer == selectedMoveText)
        {
            selectanswer = true;
            wonQuestions.Add(rann);
        }
        else
        {
            selectanswer = false;
            lostQuestions.Add(rann);
        }
        StartCoroutine(PerformPlayerMove());
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

        if (Input.GetKeyDown(KeyCode.Z)) //選擇選項
        {
            sel = currentMove;
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);

            // 获取选择的招式的文本信息
            Text selectedMoveTextObject = dialogBox.MoveTexts[sel];
            string selectedMoveText = selectedMoveTextObject.text;

            // 在这里使用 selectedMoveText，你可以将其存储、打印或进行其他操作
            Debug.Log("Selected Move Text: " + selectedMoveText);
            if (answer == selectedMoveText)
            {
                selectanswer = true;
            }
            else
            {
                selectanswer = false;
            }
            //print("boo1" + selectanswer);
            StartCoroutine(PerformPlayerMove());
        }
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
            else
            {

            }

        }
    }

}
