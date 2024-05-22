using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public Rigidbody2D rb;
    public Transform currentPoint;
    public float speed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform; //position initiale arbitraire
    }

    private void Update()
    {
        Vector2 point = currentPoint.position - transform.position; //direction dans laquelle l'nenemi veut aller
        if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform) //si l'ennemi a atteint le point actuel et que le point est pointB
        {
            Flip();
            currentPoint = pointA.transform;
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform) //si l'ennemi a atteint le point actuel et que le point est pointA
        {
            Flip();
            currentPoint = pointB.transform;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage();
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}
