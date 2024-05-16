using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public float playerSpeed;
    public float playerSpeedAtk;
    
    public GameObject atkHb;

    public GameObject lookingPoint;
    public Vector2 lookingPointPos;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerSpeed = playerMovement.speed;
    }

    private void Update()
    {
        lookingPointPos = lookingPoint.transform.position;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(PlayerFreeze());
            GameObject atk = Instantiate(atkHb, lookingPointPos, Quaternion.identity);
        }
    }

    IEnumerator PlayerFreeze()
    {
        playerMovement.speed = playerSpeedAtk;
        yield return new WaitForSeconds(0.5f);
        playerMovement.speed = playerSpeed;

    }
}
