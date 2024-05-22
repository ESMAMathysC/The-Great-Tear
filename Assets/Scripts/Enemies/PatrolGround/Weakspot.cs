using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakspot : MonoBehaviour
{
    public EnemyHealth oneShot;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            oneShot.Stomped();
        }
    }
}

