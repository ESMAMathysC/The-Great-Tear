using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserBehaviour : MonoBehaviour
{
    public PlayerMovement target;
    public GameObject refPoint;
    public float speed;
    public bool canChase;

    void Start()
    {
        target = FindObjectOfType<PlayerMovement>();
    }
    void Update()
    {
        if (canChase)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, refPoint.transform.position, speed * Time.deltaTime);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage();
        }
    }

    public void VariableChange(bool _canChase)
    {
        canChase = _canChase;
    }
}
