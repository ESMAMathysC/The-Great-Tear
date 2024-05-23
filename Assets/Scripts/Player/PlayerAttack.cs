using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public PlayerMovement movement;
    public bool isCrouching;
    public int cooldown;
    public bool cooldownReached = true;

    public GameObject atkHb;

    public GameObject lookingPoint;
    public Vector2 lookingPointPos;

    private void Start()
    {
        movement = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        isCrouching = movement.isCrouching;
        lookingPointPos = lookingPoint.transform.position;
        
        if (Input.GetKeyDown(KeyCode.Mouse0) && cooldownReached && !isCrouching)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        cooldownReached = false;
        GameObject atk = Instantiate(atkHb, lookingPointPos, Quaternion.identity);
        yield return new WaitForSeconds(cooldown);
        cooldownReached = true;
    }

}
