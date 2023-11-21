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

    //DatabaseReference reference;

    public PokemonBase Base { get; set; }
    public int Level { get; set; }

    public int HP { get; set; }
    public int MaxHP { get; set; }

    public List<Move> Moves { get; set; }

    public BattleSystem battleSystem;

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        Base = pBase;
        Level = pLevel;
        HP = MaxHp;

        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.Base));

            if (Moves.Count >= 4)
                break;
        }
    }


    public int Attack
    {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
    }
    public int Defense
    {
        get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5; }
    }
    public int SpAttack
    {
        get { return Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5; }
    }
    public int SpDefense
    {
        get { return Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5; }
    }
    public int Speed
    {
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; }
    }
    public int MaxHp
    {
        get { return Mathf.FloorToInt((Base.MaxHp * Level) / 100f) + 10; }
    }

    public bool EnemyTakeDamage(Move move, Pokemon attacker)
    {
        print("怪物最大血量"+MaxHp);
        //DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        float modifiers = Random.Range(0.85f, 1f);
        float a = (2 * attacker.Level + 10) / 250f;
        float d =  a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);
        

        BattleSystem battleSystem = FindObjectOfType<BattleSystem>();
        bool answer = battleSystem.selectanswer;

        if (answer)
        {
            HP -= damage;
            print("Ememy受傷" + damage);
            print("Ememy血量"+HP);
        }

        if (HP <= 0)
        {
            HP = 0;
            return true;
        }

        return false;
    }





    public bool MeTakeDamage(Move move, Pokemon attacker)
    {
        print("人物最大血量" + MaxHp);
        float modifiers = Random.Range(0.85f, 1f);
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);
        
        BattleSystem battleSystem = FindObjectOfType<BattleSystem>();
        bool answer = battleSystem.selectanswer;

        if (answer)
        {
            HP -= 0;
            print("me受傷" + damage);
            print("me血量" + HP);
        }
        else
        {
            HP -= damage;
            print("me受傷" + damage);
            print("me血量" + HP);
        }
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
