using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedState : IEnemyState
{
    private Enemy enemy;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        if (enemy.enemyController.shootingType == true)
        {
            Shoot();
        }

        if (!enemy.InShootRange)
        {
            enemy.ChangeState(new IdleState());
        }
        //if (enemy.InMeleeRange)
        //{
        //    enemy.ChangeState(new AttackState());
        //}
        //else if (enemy.Target != null)
        //{
        //    enemy.Move();
        //}
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter(Collider2D collider2D)
    {
        
    }

    private void Shoot()
    {
        //enemy.anim.speed = 0;
        enemy.enemyController.shootTimer += Time.deltaTime;

        if (enemy.enemyController.shootTimer >= enemy.enemyController.shootCoolDown)
        {
            enemy.enemyController.canShoot = true;
            enemy.enemyController.shootTimer = 0;
        }

        if (enemy.enemyController.canShoot)
        {
            enemy.enemyController.canShoot = false;
            enemy.anim.SetTrigger("DistanceAttack");
            Debug.Log("EnemyShooting");
            enemy.ShootToPlayer();
        }
        //enemy.anim.speed = 1;
    }
}
