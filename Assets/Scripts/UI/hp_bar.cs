using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hp_bar : MonoBehaviour
{
    public Animator anim;
    public PlayerHealth hp;
    public int currentHp;

    private void Start()
    {
        hp = FindObjectOfType<PlayerHealth>();
    }

    private void Update()
    {
        currentHp = hp.currentHp;

        if(currentHp == 3)
        {
            anim.Play("hpbar_wip_3");
        }

        if (currentHp == 2)
        {
            anim.Play("hpbar_wip_2");
        }

        if (currentHp == 1)
        {
            anim.Play("hpbar_wip_1");
        }

        if (currentHp <= 0)
        {
            anim.Play("hpbar_wip_0");
        }
    }
}
