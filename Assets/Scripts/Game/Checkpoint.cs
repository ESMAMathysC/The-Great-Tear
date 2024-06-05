using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject respawnPoint;
    public GameObject player;
    public Vector2 respawnCoordinates;
    public PlayerMovement respawnCoord;
    public bool isRaised = false;
    public Animator anim;

    private void Start()
    {
        respawnCoord = FindObjectOfType<PlayerMovement>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            respawnCoordinates = respawnPoint.transform.position;
            respawnCoord.respawnCoordinates = respawnCoordinates;
            isRaised = true;
            anim.SetBool("isRaised", true);
        }
    }
}
