using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~VARIABLES MOUVEMENT~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    private float horizontal;
    public float speed;

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~VARIABLES SAUT~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    public float jumpingPower;
    public float jumpCount;
    public bool canJump = true;
    public bool isGrounded;
    public bool isAirborne;

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~VARIABLES CROUCH~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    public Vector2 crouchingSize;
    public Vector2 crouchingOffset;
    public bool isCrouching = false;

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~VARIABLES GLIDE~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

    public float baseGravityScale;
    public float vertical;
    public bool isGliding = false;
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

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~SAUT~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        isGrounded = Physics2D.OverlapArea(groundCheck1Pos.position, groundCheck2Pos.position);

        if (!isGrounded)
        {
            isAirborne = true;
        }
        else
        {
            isAirborne = false;
        }

        if (isGrounded)
        {
            jumpCount = 0;
        }
            if (jumpCount < 1)
            {
                if (Input.GetButtonDown("Jump") && isGrounded && canJump) 
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

        Crouch();

        Glide();
    }

    private void FixedUpdate()
    {
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~MOUVEMENT GAUCHE DROITE~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        if (!isDead)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        if(!isDead && isGliding && isGrounded)
        {
            rb.velocity = new Vector2(horizontal *0, rb.velocity.y);

        }
    }
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~CROUCH~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

    private void Crouch()
    {
        if (Input.GetKey(KeyCode.S) && !isGliding)
        {
            isCrouching = true;
            canJump = false;
            col.size = crouchingSize;
            col.offset = crouchingOffset;
            anim.SetBool("isCrouching", true);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            isCrouching = false;
            canJump = true;
            col.size = standingSize;
            col.offset = standingOffset;
            anim.SetBool("isCrouching", false);
        }
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~GLIDE~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    private void Glide()
    {
        if (Input.GetKey(KeyCode.Mouse1) && isAirborne && vertical < 0 && !isCrouching)
        {
            groundCheck1.SetActive(false);
            groundCheck2.SetActive(false);
            isGliding = true;
            canJump = false;
            rb.gravityScale = 0.2f;
            col.size = planeSize;
            col.offset = planeOffset;
            anim.SetBool("isGliding", true);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Mouse1) && !isAirborne && vertical == 0)
        {
            groundCheck1.SetActive(true);
            groundCheck2.SetActive(true);
            isGliding = false;
            canJump = true;
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
