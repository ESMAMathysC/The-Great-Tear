using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballBooster : MonoBehaviour
{
    public PlayerMovement movement;

    private void Start()
    {
        movement = FindObjectOfType<PlayerMovement>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            movement.BallBoost();
        }
    }
}
