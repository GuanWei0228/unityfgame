using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;
using Firebase;
using System.Threading.Tasks;
//using System;

public class Pokemon : MonoBehaviour
{

    DatabaseReference reference;

    

    public PokemonBase Base { get; set; }
    public int Level { get; set; }

    public int HP{ get; set;}
    public int MaxHP{ get; set;}

    public List<Move> Moves{ get;set;}

    public BattleSystem battleSystem;

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        Base = pBase;
        Level = pLevel;
        HP = MaxHp;

        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if(move.Level <= Level)
                Moves.Add(new Move(move.Base));

            if(Moves.Count >=4)
            break;    
        }
    }


    public int Attack{
        get{return Mathf.FloorToInt((Base.Attack*Level)/100f)+5;}
    }
    public int Defense{
        get{return Mathf.FloorToInt((Base.Defense*Level)/100f)+5;}
    }
    public int SpAttack{
        get{return Mathf.FloorToInt((Base.SpAttack*Level)/100f)+5;}
    }
    public int SpDefense{
        get{return Mathf.FloorToInt((Base.SpAttack*Level)/100f)+5;}
    }
    public int Speed{
        get{return Mathf.FloorToInt((Base.Speed * Level)/100f)+5;}
    }
    public int MaxHp{
        get{return Mathf.FloorToInt((Base.Speed * Level) / 100f)+10;}
    }


    public bool EnemyTakeDamage(Move move,Pokemon attacker)
    {
        
        BattleSystem battleSystem = FindObjectOfType<BattleSystem>();
        BattleDialogBox battleDialogBox = FindObjectOfType<BattleDialogBox>();
        string rann = battleSystem.rann;
        int cur = battleSystem.sel + 1;
        string sel = cur.ToString();
        float modifiers = Random.Range(0.85f, 1f);
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);
        bool dataLoaded = false;


        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            reference = FirebaseDatabase.DefaultInstance.RootReference;
            
            string answer = string.Empty;
            string select = string.Empty;


            // 比對邏輯
            reference.Child("QAQ").Child("Ans").Child(rann).GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted)
                {
                    // Handle the error...
                }
                if (task.IsCompletedSuccessfully)
                {
                    DataSnapshot snapshot = task.Result;
                    string answer = snapshot.Value.ToString();

                    reference.Child("QAQ").Child("A").Child(rann).Child(sel).GetValueAsync().ContinueWithOnMainThread(task => {
                        if (task.IsFaulted)
                        {
                            // Handle the error...
                        }
                        if (task.IsCompletedSuccessfully)
                        {
                            DataSnapshot snapshot = task.Result;
                            string select = snapshot.Value.ToString();
                            print("選項" + select);
                            //暫放計算damage

                            
                            // 將比較邏輯放在這裡
                            if (string.Equals(answer, select))
                            {
                                print("哭");
                                dataLoaded = true;
                                //HP -= damage;
                            }
                            else
                            {
                                print("沒料");
                            }

                            while (dataLoaded)
                            {
                                HP -= damage;
                                dataLoaded = false;
                                break;
                            }

                        }
                    });


                }
            });
            


        });



        if (HP <= 0)
        {
            HP = 0;
            return true;
        }

        return false;
    }

   



    public bool MeTakeDamage(Move move, Pokemon attacker)
    {

        float modifiers = Random.Range(0.85f, 1f);
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        //HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            return true;
        }

        return false;
    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
}
