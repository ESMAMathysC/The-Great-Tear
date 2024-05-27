using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectionBird : MonoBehaviour
{
    public BirdBehaviour behaviour;

    private void Start()
    {
        behaviour = FindObjectOfType<BirdBehaviour>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Trigger entré");
            behaviour.DiveAttack();
        }
    }
}
