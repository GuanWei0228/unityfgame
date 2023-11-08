using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{

    DatabaseReference reference;

    [SerializeField] int lettersPerSecond;
    [SerializeField] Color highlightedColor;

    [SerializeField] Text dialogText;
    [SerializeField] GameObject answerSelector;
    [SerializeField] GameObject backSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    [SerializeField] Button btn1;
    [SerializeField] Button btn2;
    [SerializeField] Button btn3;
    [SerializeField] Button btn4;

    [SerializeField] List<Text> actionTexts;
    [SerializeField] List<Text> backTexts;
    [SerializeField] List<Text> moveTexts;
    public List<Text> MoveTexts => moveTexts;


    [SerializeField] Text ppText;
    [SerializeField] Text typeText;

    public BattleSystem battleSystem;

    public event Action OnDialogComplete; // 定義一個事件


    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;   
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f/lettersPerSecond);
        }

        OnDialogComplete?.Invoke();

    }
    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }
    public void EnableActionSelector(bool enabled)
    {
       answerSelector.SetActive(enabled);
    }
    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        backSelector.SetActive(enabled);
        //moveDetails.SetActive(enabled);
    }

    public void UpdateActionSelection(int selectedAction)
    {
        for(int i=0; i<actionTexts.Count; ++i)
        {
            if(i == selectedAction)
                actionTexts[i].color = highlightedColor;
            else
                actionTexts[i].color = Color.black;

        }
    }

    public void UpdateMoveSelection(int selectedMove, Move move)
    {
        for(int i=0;i<moveTexts.Count; ++i)
        {
            if(i == selectedMove)
                moveTexts[i].color = highlightedColor;
            else
                moveTexts[i].color = Color.black;
        }

        //ppText.text = $"PP {move.PP}/{move.Base.PP}";
        //typeText.text = move.Base.Type.ToString();
    }

    public void UpdateBackSelection(int selectedAction)
    {
        for (int i = 0; i < backTexts.Count; ++i)
        {
            if (i == selectedAction)
                backTexts[i].color = highlightedColor;
            else
                backTexts[i].color = Color.black;

        }
    }

    public void SetMoveNames(List<Move> moves)
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        BattleSystem battleSystem = FindObjectOfType<BattleSystem>();
        string rann = battleSystem.rann;

        //dialogBox.TypeDialog("");
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            int currentIndex = i;
            if (i < 1) {
                reference.Child("QAQ").Child("A").Child(rann).Child("1").GetValueAsync().ContinueWithOnMainThread(task => {
                    if (task.IsFaulted)
                    {
                        print("00000");
                    }
                    if (task.IsCompletedSuccessfully)
                    {
                        DataSnapshot snapshot = task.Result;
                        string dialogMessage = snapshot.Value.ToString();
                        
                        moveTexts[currentIndex].text = dialogMessage;
                        
                        Text buttonText = btn1.GetComponentInChildren<Text>();
                        buttonText.text = dialogMessage;
                    }
                });
                
            }
            else if (i < 2)
            {
                reference.Child("QAQ").Child("A").Child(rann).Child("2").GetValueAsync().ContinueWithOnMainThread(task => {
                    if (task.IsFaulted)
                    {
                        print("00000");
                    }
                    if (task.IsCompletedSuccessfully)
                    {
                        DataSnapshot snapshot = task.Result;
                        string dialogMessage = snapshot.Value.ToString();
                        
                        moveTexts[currentIndex].text = dialogMessage;

                        Text buttonText = btn2.GetComponentInChildren<Text>();
                        buttonText.text = dialogMessage;
                    }
                });
            }
            else if (i < 3)
            {
                reference.Child("QAQ").Child("A").Child(rann).Child("3").GetValueAsync().ContinueWithOnMainThread(task => {
                    if (task.IsFaulted)
                    {
                        print("00000");
                    }
                    if (task.IsCompletedSuccessfully)
                    {
                        DataSnapshot snapshot = task.Result;
                        string dialogMessage = snapshot.Value.ToString();
                        
                        moveTexts[currentIndex].text = dialogMessage;

                        Text buttonText = btn3.GetComponentInChildren<Text>();
                        buttonText.text = dialogMessage;
                    }
                });
            }
            else
            {
                reference.Child("QAQ").Child("A").Child(rann).Child("4").GetValueAsync().ContinueWithOnMainThread(task => {
                    if (task.IsFaulted)
                    {
                        print("00000");
                    }
                    if (task.IsCompletedSuccessfully)
                    {
                        DataSnapshot snapshot = task.Result;
                        string dialogMessage = snapshot.Value.ToString();
                        
                        moveTexts[currentIndex].text = dialogMessage;

                        Text buttonText = btn4.GetComponentInChildren<Text>();
                        buttonText.text = dialogMessage;
                    }
                });

            }
        }
        

        /*for (int i = 0; i < moveTexts.Count; ++i)
        {
            if (i < 1)
                moveTexts[i].text = "A.solitary";
            else if (i < 2)
                moveTexts[i].text = "B.symphony";
            else if (i < 3)
                moveTexts[i].text = "C.slump";
            else
                moveTexts[i].text = "D.knack";
        }*/
    }

    
}
