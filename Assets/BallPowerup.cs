using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPowerup : MonoBehaviour
{
    public PlayerMovement getCrouchPower;
    public void Start()
    {
        getCrouchPower = GameObject.FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().GetCrouch();
            Destroy(this.gameObject);
        }
    }
}
