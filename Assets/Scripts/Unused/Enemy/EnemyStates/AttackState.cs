using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    private Enemy enemy;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        if (enemy.enemyController.shootingType == false)
        {
            Attack();
        }

        if (!enemy.InMeleeRange)
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter(Collider2D collider2D)
    {
        
    }

    private void Attack()
    {
        
        enemy.enemyController.shootTimer += Time.deltaTime;

        if (enemy.enemyController.shootTimer >= enemy.enemyController.shootCoolDown)
        {
            enemy.enemyController.canShoot = true;
            enemy.enemyController.shootTimer = 0;
        }

        if (enemy.enemyController.canShoot)
        {
            enemy.enemyController.canShoot = false;
            enemy.anim.SetTrigger("MeleeAttack");
            Debug.Log("EnemyMeleeAttack");
            //enemy.ShootToPlayer();
        }
    }
}
