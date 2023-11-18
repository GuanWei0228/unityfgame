using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    Vector2 movement;
    public PokemonBase test;
    public string Monstertag;
    public event Action OnEncountered;
    public float speed;


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
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    void SwitchAnim()
    {
        anim.SetFloat("speed", movement.magnitude);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Monster")
        {
            Monstertag = other.gameObject.tag;
            HandleMonsterEncounter(other.gameObject);
        }

        if (other.gameObject.tag == "Boss")
        {
            Monstertag = other.gameObject.tag;
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
