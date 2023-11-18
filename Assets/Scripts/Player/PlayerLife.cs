using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerLife : MonoBehaviour
{
    // Start is called before the first frame update

    public float playerHP;
    public Image hpBar;
    



    void Start()
    {
        playerHP = 1;
        
        Invoke("ClearMessage", 3);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHP <= 0)
            {
            Invoke("ClearMessage", 4);
        }
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Monster")
        {
            playerHP = 0.1f;
            hpBar.GetComponent<Image>().fillAmount -= 0.1f;
            print("¦©¦å");
        }
    }
    
}
