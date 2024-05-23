using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~VARIABLES MOUVEMENT~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    public float horizontal;

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~VARIABLES SAUT~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    private float jumpCount;
    private bool canJump = true;
    private bool isGrounded;
    private bool isAirborne;

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~VARIABLES CROUCH~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    public bool isCrouching = false;
    private bool forceCrouch = false;

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~VARIABLES GLIDE~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    private float vertical;
    private bool isGliding = false;
    private int glideJumpCount = 0;


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~VARIABLES SPRITE~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    private bool isFacingRight = true;

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~VARIABLES HP~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    private bool isDead = false;

    [Header("Imports")]

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D col;
    [SerializeField] private CircleCollider2D circleCol;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject groundCheck1;
    [SerializeField] private GameObject groundCheck2;
    [SerializeField] private Transform groundCheck1Pos;
    [SerializeField] private Transform groundCheck2Pos;
    [SerializeField] private LayerMask groundLayer;

    [Header("Hitbox changes")]
    [SerializeField] private Vector2 standingSize;
    [SerializeField] private Vector2 standingOffset;
    [SerializeField] private Vector2 crouchingSize;
    [SerializeField] private Vector2 crouchingOffset;
    [SerializeField] private Vector2 planeSize;
    [SerializeField] private Vector2 planeOffset;

    [Header("Settings")]
    public float speed;
    [SerializeField] private float maxBallSpeed;
    [SerializeField] private float ballSpeed;
    [SerializeField] private float recoilForce;
    [SerializeField] private float jumpingPower;
    [SerializeField] private float jumpingPowerGlide;
    [SerializeField] private float baseGravityScale;

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
        if(Physics2D.OverlapArea(groundCheck1Pos.position, groundCheck2Pos.position) && rb.velocity.y == 0f)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

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
            if (Input.GetButtonDown("Jump") && canJump)
            {
                rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse);
                jumpCount += 1;
            }
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        vertical = rb.velocity.y;

        Flip();

        Crouch();

        Glide();

    }

    private void FixedUpdate()
    {
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~MOUVEMENT GAUCHE DROITE~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        if (!isDead && !isCrouching)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        if(!isDead && isGliding && isGrounded)
        {
            rb.velocity = new Vector2(horizontal *0, rb.velocity.y);
        }
        if (!isDead && isCrouching || forceCrouch)
        {
            var impulse = (-horizontal * ballSpeed * Mathf.Deg2Rad) * rb.inertia;
            rb.AddTorque(impulse, ForceMode2D.Impulse);
            if (rb.angularVelocity > maxBallSpeed)
            {
                rb.angularVelocity = maxBallSpeed;
            }
            if (rb.angularVelocity < -maxBallSpeed)
            {
                rb.angularVelocity = -maxBallSpeed;
            }
        }
    }
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~CROUCH~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

    private void Crouch()
    {
        if (Input.GetKey(KeyCode.S) && !isGliding || forceCrouch)
        {
            rb.freezeRotation = false;
            rb.GetComponent<Rigidbody2D>().sharedMaterial.friction = 1;
            isCrouching = true;
            canJump = false;
            col.enabled = false;
            circleCol.enabled = true;
            anim.SetBool("isCrouching", true);
        }
        else if (Input.GetKeyUp(KeyCode.S) && !forceCrouch || !forceCrouch)
        {
            rb.freezeRotation = true;
            rb.GetComponent<Rigidbody2D>().sharedMaterial.friction = 0;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            isCrouching = false;
            canJump = true;
            col.enabled = true;
            circleCol.enabled = false;
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
            rb.gravityScale = 1f;
            col.size = planeSize;
            col.offset = planeOffset;
            anim.SetBool("isGliding", true);
        }

        if (isGliding && Input.GetButtonDown("Jump") && glideJumpCount == 0)
        {
            glideJumpCount += 1;
            //Vector2 dash = new Vector2(rb.velocity.x * jumpingPowerGlide, 0f);
            //rb.velocity += dash;
            Debug.Log("HyperVitesse !");
            Debug.Log(new Vector2(Mathf.Sign(rb.velocity.x) * jumpingPowerGlide, 0f));
            rb.AddForce(new Vector2(Mathf.Sign(rb.velocity.x) * jumpingPowerGlide, 0f), ForceMode2D.Impulse);
            Debug.Log(rb.velocity.x);
        }

        else if (Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Mouse1) && !isAirborne && vertical == 0)
        {
            groundCheck1.SetActive(true);
            groundCheck2.SetActive(true);
            isGliding = false;
            canJump = true;
            glideJumpCount = 0;
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

    public void Die()
    {
        isDead = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("weakspot"))
        {
            rb.AddForce(Vector2.up * recoilForce, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("forceCrouch"))
        {
            forceCrouch = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("forceCrouch"))
        {
            forceCrouch = false;
            isCrouching = false;
        }
    }
}
