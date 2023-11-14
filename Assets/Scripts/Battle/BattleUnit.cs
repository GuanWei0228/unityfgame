using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] public PokemonBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Pokemon Pokemon { get; set; }
    public void ChangeBase(PokemonBase newBase)
    {
        _base = newBase;
        // 其他相應的初始化邏輯
    }
    public void Setup()
    {

        Pokemon = new Pokemon(_base, level);
        if (isPlayerUnit)
            GetComponent<Image>().sprite = Pokemon.Base.BackSprite;

        else
            GetComponent<Image>().sprite = Pokemon.Base.FrontSprite;

    }

}