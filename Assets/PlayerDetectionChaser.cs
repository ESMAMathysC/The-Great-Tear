using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectionChaser : MonoBehaviour
{
    public ChaserBehaviour movement;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            movement.VariableChange(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            movement.VariableChange(false);
        }
    }
}
