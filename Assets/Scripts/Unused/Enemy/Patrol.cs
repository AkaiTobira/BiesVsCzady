using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    float speed;
    public float distance;

    bool movingRight = true;
    public Transform patrolPath;

    private void Start()
    {
        speed = EnemyController.instance.speed;
    }

    void Update()
    {
        //EnemyPatrol();
    }

    void EnemyPatrol()
    {
        if (EnemyController.instance.isTriggered == false)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            RaycastHit2D pathInfo = Physics2D.Raycast(patrolPath.position, Vector2.down, distance);

            if (pathInfo.collider == false)
            {
                if (movingRight == true)
                {
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    movingRight = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    movingRight = true;
                }
            }

            EnemyController.instance.anim.SetBool("isRunning", true);
        }
    }
}
