using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballBooster : MonoBehaviour
{
    public PlayerMovement movement;
    public bool isCrouching;
    public Rigidbody2D rb;
    public int boostForce;
    public int boostForceUp;

    private void Start()
    {
        movement = FindObjectOfType<PlayerMovement>();
        rb = movement.rb;
    }

    private void Update()
    {
        isCrouching = movement.isCrouching;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCrouching)
        {
            rb.AddForce(Vector2.right * boostForce, ForceMode2D.Impulse);
            rb.AddForce(Vector2.up * boostForceUp, ForceMode2D.Impulse);

        }


    }
}
