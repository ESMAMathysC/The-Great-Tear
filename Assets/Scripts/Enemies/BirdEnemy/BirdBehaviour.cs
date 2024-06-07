using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehaviour : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public Rigidbody2D rb;
    public Animator anim;
    public Transform currentPoint;
    public float speed;

    public int baseGravityScale = 1;
    public bool isDiving = false;
    public bool mustStop = false;
    public float groundedTime;
    public float jumpForce;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform; //position initiale arbitraire
    }

    private void Update()
    {
        Vector2 point = currentPoint.position - transform.position; //direction dans laquelle l'ennemi veut aller
        if (!isDiving)
        {
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
        if (mustStop)
        {
            isDiving = false;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            mustStop = false;
        }


    }
    public void DiveAttack()
    {
        StartCoroutine(Dive());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("stop"))
        {
            mustStop = true;

        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    IEnumerator Dive()
    {
        isDiving = true;
        anim.SetBool("isDiving", true);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 3;
        yield return new WaitForSeconds(groundedTime);
        rb.gravityScale = 2;
        anim.SetBool("isDiving", false);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}