using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdHealth : MonoBehaviour
{
    public int hp;
    public GameObject objectToDestroy;
    public BoxCollider2D enemyHb;
    public Animator anim;
    public Rigidbody2D rb;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "atk")
        {
            TakeDamage();
        }
    }
    public void TakeDamage()
    {
        hp -= 1;

        if (hp <= 0)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            enemyHb.enabled = false;
            anim.SetBool("isDead", true);
            Destroy(objectToDestroy, 0.5f);
        }
    }
}
