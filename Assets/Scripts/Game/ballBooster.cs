using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballBooster : MonoBehaviour
{
    public PlayerMovement movement;
    public bool isFacingRight;
    public Rigidbody2D rb;
    public int boostForce;

    private void Start()
    {
        movement = FindObjectOfType<PlayerMovement>();
        isFacingRight = movement.isFacingRight;
        rb = movement.rb;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isFacingRight)
        {
            rb.AddForce(Vector2.right * boostForce, ForceMode2D.Impulse);

        }
        if (!isFacingRight)
        {
            rb.AddForce(Vector2.left * boostForce, ForceMode2D.Impulse);
        }
    }
}
