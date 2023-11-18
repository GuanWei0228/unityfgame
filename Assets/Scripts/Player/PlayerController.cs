using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public PokemonBase test;
    Animator anim;
    public event Action OnEncountered;
    public float speed;
    Vector2 movement;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x != 0)
        {
            transform.localScale = new Vector3(movement.x, 1, 1);
        }
        SwitchAnim();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement *speed*Time.fixedDeltaTime);
    }

    void SwitchAnim() 
    {
        anim.SetFloat("speed", movement.magnitude);    
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Monster")
        {
            HandleMonsterEncounter(other.gameObject);
        }

        if (other.gameObject.tag == "Boss")
        {
            HandleBossEncounter(other.gameObject);
        }
    }

    void HandleMonsterEncounter(GameObject Monster)
    {
        EnemyMovement MonsterScript = Monster.GetComponent<EnemyMovement>();

        if (MonsterScript != null)
        {
            PokemonBase encounteredPokemonBase = MonsterScript.GetPokemonBase();

            test = encounteredPokemonBase;
            OnEncountered();
        }
    }
    void HandleBossEncounter(GameObject Boss)
    {
        BossMovement BossScript = Boss.GetComponent<BossMovement>();

        if (BossScript != null)
        {
            PokemonBase encounteredPokemonBase = BossScript.GetPokemonBase();

            test = encounteredPokemonBase;
            OnEncountered();
        }
    }

}
