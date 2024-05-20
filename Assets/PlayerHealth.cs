using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealh : MonoBehaviour
{
    public int hp;
    public PlayerMovement movement;
    public PlayerAttack attack;
    public Animator anim;
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "enemy")
        {
            hp -= 1;
        }
    }

    public void Update()
    {
        if (hp <= 0)
        {
            hp = 0;
            movement.enabled = false;
            attack.enabled = false;
            anim.SetBool("isDead", true);
        }
    }
    public void GainHealth()
    {
        hp += 1;
    }
}
