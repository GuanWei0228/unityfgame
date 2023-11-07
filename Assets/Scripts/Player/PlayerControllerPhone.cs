using UnityEngine;
using System;

public class PlayerControllerPhone : MonoBehaviour
{
    Rigidbody2D rb;
    VirtualJoystick joystick;
    Animator anim;
    //public event Action OnEncountered;
    public float speed;
    Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        joystick = FindObjectOfType<VirtualJoystick>();
    }

    void Update()
    {
        movement.x = MathF.Sign(joystick.Horizontal());
        movement.y = joystick.Vertical();
        //movement = getAxis
        print(movement.x);
        print(movement.y);


        if (movement.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(movement.x), 1, 1); // 調整角色的朝向
        }

        //print(movement);

        //Vector2 newPosition = rb.position + movement * speed * Time.fixedDeltaTime;
        //rb.MovePosition(newPosition);
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

    /*void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Monster")
        {
            OnEncountered();
        }
    }*/
}