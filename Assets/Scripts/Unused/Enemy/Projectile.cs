using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;

    public EnemyController enemyController;
    Rigidbody2D rb;
    public Vector2 dir;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyController = FindObjectOfType<EnemyController>();
    }

    private void Update()
    {
        rb.velocity = dir * 850f;
    }

    public void LookAt()
    {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

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
