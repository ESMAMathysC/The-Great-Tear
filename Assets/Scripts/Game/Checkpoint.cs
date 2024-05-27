using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject respawnPoint;
    public GameObject player;
    public Vector2 respawnCoordinates;
    public bool isRaised = false;
    public Animator anim;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            respawnCoordinates = respawnPoint.transform.position;
            isRaised = true;
            anim.SetBool("isRaised", true);
        }
    }

    public void OnPlayerDeath()
    {
        player.transform.position = respawnCoordinates;
    }
}
