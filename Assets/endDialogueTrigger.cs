using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endDialogueTrigger : MonoBehaviour
{
    public GameObject dialogue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogue.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
