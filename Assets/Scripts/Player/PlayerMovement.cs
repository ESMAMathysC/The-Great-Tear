using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~VARIABLES MOUVEMENT~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    private float horizontal;
    public float speed;
    public float speedDamp;

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~VARIABLES SAUT~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    public float jumpingPower;
    public float jumpCount;
    public bool isGrounded;

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~VARIABLES GLIDE~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

    public float baseGravityScale;
    public float vertical;
    public Vector2 planeSize;
    public Vector2 planeOffset;

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~VARIABLES SPRITE~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    public bool isFacingRight = true;

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~VARIABLES HP~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    public bool isDead = false;

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~REFERENCES GAMEOBJECTS ET AUTRE~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    public Rigidbody2D rb;
    public BoxCollider2D col;
    public Animator anim;
    public Vector2 standingSize;
    public Vector2 standingOffset;
    public GameObject groundCheck1;
    public GameObject groundCheck2;
    public Transform groundCheck1Pos;
    public Transform groundCheck2Pos;
    public LayerMask groundLayer;

    private void Start()
    {
        baseGravityScale = rb.gravityScale;
    }

    void Update()
    {
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~MOUVEMENT ANIMATION~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        if (!isDead)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            float characterVelocity = Mathf.Abs(rb.velocity.x); //convertit la valeur de la vitesse en chiffre positif pour l'animator
        }

        if (Input.GetButtonUp("Horizontal"))
        {
            while (rb.velocity.x != 0)
            {
                rb.velocity = new Vector2(rb.velocity.x * speedDamp, rb.velocity.y);

            }
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~SAUT~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

        isGrounded = Physics2D.OverlapArea(groundCheck1Pos.position, groundCheck2Pos.position);

        if (isGrounded)
        {
            jumpCount = 0;
        }
        if (jumpCount < 1)
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse);
                jumpCount += 1;
            }

            if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        }

        vertical = rb.velocity.y;

        Flip();
    }

    private void FixedUpdate()
    {
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~MOUVEMENT GAUCHE DROITE~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

        if (!isDead)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~GLIDE~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

        if (Input.GetKey(KeyCode.Mouse1) && !isGrounded && vertical < 0)
        {
            groundCheck1.SetActive(false);
            groundCheck2.SetActive(false);
            rb.gravityScale = 0.2f;
            col.size = planeSize;
            col.offset = planeOffset;
            anim.SetBool("isGliding", true);
        }
        else
        {
            groundCheck1.SetActive(true);
            groundCheck2.SetActive(true);
            rb.gravityScale = baseGravityScale;
            col.size = standingSize;
            col.offset = standingOffset;
            anim.SetBool("isGliding", false);
        }

    }



    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~FONCTION FLIP~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

    private void Flip()
    {
        //SI le joueur regarde vers la droite ET l'input horizontal est inférieur à 0
        //OU le joueur ne regarde pas vers la droite ET l'input horizontal est supérieur à 0
        //on FLIP le joueur
        if ((isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f))
        {
            //isFacingRight devient son opposé (TRUE ou FALSE)
            //on multiplie le scale X du joueur par -1
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
