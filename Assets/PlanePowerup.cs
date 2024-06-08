using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanePowerup : MonoBehaviour
{
    public PlayerMovement getPlanePower;
    public GameObject dialogue;

    public void Start()
    {
        getPlanePower = GameObject.FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().GetPlane();
            dialogue.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
