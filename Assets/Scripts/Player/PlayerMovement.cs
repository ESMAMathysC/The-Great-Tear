using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~VARIABLES MOUVEMENT~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    public float horizontal;
    public Vector2 respawnCoordinates;

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~VARIABLES SAUT~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    private float jumpCount;
    private bool canJump = true;
    private bool isGrounded;
    private bool isAirborne;

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~VARIABLES CROUCH~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    public bool hasCrouchPower = false;
    public bool isCrouching = false;
    private bool forceCrouch = false;

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~VARIABLES GLIDE~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    public bool hasPlanePower = false;
    private float vertical;
    private bool isGliding = false;
    private int glideDashCount = 0;
    private bool wantsToDash = false;


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~VARIABLES SPRITE~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    public bool isFacingRight = true;

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~VARIABLES HP~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    private bool isDead = false;

    [Header("Imports")]

    [SerializeField] public Rigidbody2D rb;
    public PlayerHealth hp;
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
    [SerializeField] public float coinCount;
    [SerializeField] private float maxBallSpeed;
    [SerializeField] private float ballSpeed;
    [SerializeField] private float boostForce;
    [SerializeField] private float recoilForce;
    [SerializeField] private float jumpingPower;
    [SerializeField] private float glideSpeed;
    [SerializeField] private float dashGlidePower;
    [SerializeField] private float dashGlideUpPower;
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
            anim.SetFloat("speed", characterVelocity);


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
        if (jumpCount < 1 && !isGliding && isGrounded)
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
        float characterVerticalVelocity = Mathf.Abs(rb.velocity.y);
        anim.SetFloat("verticalSpeed", characterVerticalVelocity);

        Flip();

        Crouch();

        Glide();

        if (Input.GetKey(KeyCode.Alpha1))
        {
            transform.position = new Vector2(-253, -128);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            transform.position = new Vector2(-147, -142);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            transform.position = new Vector2(-165, -122);
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            transform.position = new Vector2(-148, -54);
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            transform.position = new Vector2(-336, -97);
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            transform.position = new Vector2(-373, -13);
        }
        if (Input.GetKey(KeyCode.Alpha7))
        {
            transform.position = new Vector2(-292, 77);
        }
        if (Input.GetKey(KeyCode.Alpha0))
        {
            hasCrouchPower = true;
            hasPlanePower = true;
        }
        if (Input.GetKey(KeyCode.Backspace))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void FixedUpdate()
    {
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~MOUVEMENT GAUCHE DROITE~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        if (!isDead && !isCrouching &&!isGliding && !wantsToDash)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }



        if (!isDead && isGliding && isFacingRight)
        {
            rb.AddForce(Vector2.right * glideSpeed);
        }
        else if (!isDead && isGliding && !isFacingRight)
        {
            rb.AddForce(Vector2.left * glideSpeed);
        }
        if (wantsToDash && isFacingRight)
        {
            rb.AddForce(Vector2.right * dashGlidePower);
            rb.AddForce(Vector2.up * dashGlideUpPower);

            wantsToDash = false;
        }
        else if(wantsToDash && !isFacingRight)
        {
            rb.AddForce(Vector2.left * dashGlidePower);
            rb.AddForce(Vector2.up * dashGlideUpPower);
            wantsToDash = false;
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
        if (hasCrouchPower)
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

    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~GLIDE~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
    private void Glide()
    {
        if (hasPlanePower)
        {
            if (Input.GetKey(KeyCode.Mouse1) && isAirborne && vertical < 0 && !isCrouching)
            {
                groundCheck1.SetActive(false);
                groundCheck2.SetActive(false);
                rb.drag = 2;
                isGliding = true;
                canJump = false;
                rb.gravityScale = 1f;
                col.size = planeSize;
                col.offset = planeOffset;
                anim.SetBool("isGliding", true);
            }

            if (isGliding && Input.GetButtonDown("Jump") && glideDashCount == 0 && isAirborne)
            {
                glideDashCount += 1;
                wantsToDash = true;
            }

            else if (Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Mouse1) && !isAirborne && vertical == 0)
            {
                groundCheck1.SetActive(true);
                groundCheck2.SetActive(true);
                rb.drag = 1;
                isGliding = false;
                canJump = true;
                rb.gravityScale = baseGravityScale;
                col.size = standingSize;
                col.offset = standingOffset;
                anim.SetBool("isGliding", false);
            }
            if (isGrounded)
            {
                glideDashCount = 0;
            }
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

    public void OnPlayerDeath()
    {
        transform.position = respawnCoordinates;
    }
    public void Respawn()
    {
        isDead = false;
    }
    public void GetCrouch()
    {
        hasCrouchPower = true;
    }
    public void GetPlane()
    {
        hasPlanePower = true;
    }

    public void GetCoin()
    {
        coinCount += 1;
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
