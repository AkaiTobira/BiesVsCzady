using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private Enemy enemy;
    private float patrolTimer;
    private float patrolDuration = 10;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Patrol();

        if ((enemy.enemyController.shootingType && !enemy.InShootRange) || (!enemy.enemyController.shootingType && !enemy.InMeleeRange))
            enemy.Move();

        if (enemy.Target != null && enemy.InShootRange && enemy.enemyController.shootingType)
        {
            enemy.ChangeState(new RangedState());
        }
        else if (enemy.Target != null && enemy.InMeleeRange && !enemy.enemyController.shootingType)
        {
            enemy.ChangeState(new AttackState());
        }
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter(Collider2D collider2D)
    {
        if (collider2D.tag == "Enviro")
        {
            enemy.ChangeDirection();
        }
    }

    private void Patrol()
    {
        enemy.anim.SetBool("isRunning", true);
        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolDuration)
        {
            enemy.ChangeState(new IdleState());
        }
    }
}
