using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;

    private Transform player;
    private Vector2 moveDir;
    Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        moveDir = (player.position - transform.position).normalized * speed;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(moveDir.x, moveDir.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("gracz");
            Destroy(gameObject);
        }
        if (collision.CompareTag("Enviro"))
        {
            Debug.Log("świat");
            Destroy(gameObject);
        }
    }
}
