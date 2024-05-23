using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float life;
    public float speed;
    public Rigidbody2D rb;

    private void Update()
    {
        rb.velocity = new Vector2(-speed, rb.velocity.y);
    }

    private void Awake()
    {
        Destroy(gameObject, life);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
