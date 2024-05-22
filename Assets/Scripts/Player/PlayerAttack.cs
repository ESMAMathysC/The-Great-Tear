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

    private void Update()
    {
        lookingPointPos = lookingPoint.transform.position;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject atk = Instantiate(atkHb, lookingPointPos, Quaternion.identity);
        }
    }

}
