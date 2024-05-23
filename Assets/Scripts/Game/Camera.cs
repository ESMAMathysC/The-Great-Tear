using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject player;
    public float timeOffset;
    public Vector3 posOffset;
    public PlayerMovement movement;
    public int newOffset;
    public float horizontal;

    private Vector3 velocity;

    private void Start()
    {
        movement = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        horizontal = movement.horizontal;
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + posOffset, ref velocity, timeOffset);

        if(horizontal > 0)
        {
            posOffset.x = newOffset;
        }

        if (horizontal < 0)
        {
            posOffset.x = -newOffset;
        }

        if (horizontal == 0)
        {
            posOffset.x = 0;
        }
    }
}