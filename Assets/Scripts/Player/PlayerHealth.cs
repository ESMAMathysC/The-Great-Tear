using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currentHp;
    public int maxHp = 3;
    public PlayerMovement movement;
    public PlayerAttack attack;
    public Animator anim;

    public Checkpoint checkpoint;

    private void Start()
    {
        currentHp = maxHp;
    }
    public void TakeDamage()
    {
        currentHp -= 1;

        if (currentHp <= 0)
        {
            StartCoroutine(RespawnTimer());
        }
    }

    public void OneShot()
    {
        currentHp = 0;
        movement.Die();
        movement.enabled = false;
        attack.enabled = false;
        anim.SetBool("isDead", true);
    }

    IEnumerator RespawnTimer()
    {
        currentHp = 0;
        movement.Die();
        movement.enabled = false;
        attack.enabled = false;
        anim.SetBool("isDead", true);
        yield return new WaitForSeconds(2);
        currentHp = maxHp;
        movement.Respawn();
        movement.enabled = true;
        attack.enabled = true;
        anim.SetBool("isDead", false);
        movement.OnPlayerDeath();
    }
}
