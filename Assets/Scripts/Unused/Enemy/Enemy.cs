using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private IEnemyState currentState;
    public EnemyController enemyController;
    public GameObject Target { get; set; }
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
        ChangeState(new IdleState());
    }

    void Update()
    {
        currentState.Execute();

        LookAtTarget();
    }

    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= enemyController.meleeRange;
            }

            return false;
        }
    }

    public bool InShootRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= enemyController.shootRange;
            }

            return false;
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter(this);
    }

    public void Move()
    {
        transform.Translate(GetDirection() * enemyController.speed * Time.deltaTime);
    }

    public Vector2 GetDirection()
    {
        return enemyController.facingRight ? Vector2.right : Vector2.left;
    }

    public void ChangeDirection()
    {
        enemyController.facingRight = !enemyController.facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentState.OnTriggerEnter(collision);
    }

    private void LookAtTarget()
    {
        if (Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;

            if (xDir < 0 && enemyController.facingRight || xDir > 0 && !enemyController.facingRight)
            {
                ChangeDirection();
            }
        }
    }

    public void ShootToPlayer()
    {
        if (enemyController.facingRight)
        {
            GameObject temp = Instantiate(enemyController.projectile, enemyController.shootPosition.transform.position, Quaternion.identity);
            temp.GetComponent<Projectile>().dir = Vector2.right;
            temp.GetComponent<Projectile>().LookAt();
        }
        else
        {
            GameObject temp = Instantiate(enemyController.projectile, enemyController.shootPosition.transform.position, Quaternion.identity);
            temp.GetComponent<Projectile>().dir = Vector2.left;
        }
    }
}
