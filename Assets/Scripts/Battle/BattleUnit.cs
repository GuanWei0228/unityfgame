using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
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