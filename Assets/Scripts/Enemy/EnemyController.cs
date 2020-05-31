using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;

    public bool shootingType = false;

    [Header("Movement")]
    public float speed;
    public float lookRadius = 7f;
    public float retreatDistance;
    public float attackDistance;
    public bool isTriggered = false;
    bool facingRight = true;

    Transform player;

    [Header("Bullet")]
    public GameObject projectile;
    private float timeBetweenShots;
    public float startTimeBetweenShots;

    void Awake()
    {
        instance = this;    
    }

    void Start()
    {
        player = GlobalUtils.PlayerObject;
        timeBetweenShots = startTimeBetweenShots;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) < lookRadius)
        {
            isTriggered = true;
        }

        FollowPlayer();
    }

    public void FollowPlayer()
    {
        if (isTriggered)
        {
            if (shootingType == true)
            {
                if (Vector2.Distance(transform.position, player.position) > attackDistance)
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

                    if (player.position.x > transform.position.x && !facingRight)
                        FlipEnemySprite();
                    if (player.position.x < transform.position.x && facingRight)
                        FlipEnemySprite();
                }
                //else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
                //{
                //    transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
                //}
                else
                {
                    transform.position = transform.position;
                }
            }
            else
            {
                //TODO: change attack distance later 
                if (Vector2.Distance(transform.position, player.position) > 2)
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

                    if (player.position.x > transform.position.x && !facingRight)
                        FlipEnemySprite();
                    if (player.position.x < transform.position.x && facingRight)
                        FlipEnemySprite();
                }
            }

            if (shootingType == true)
                Shoot();
        }
    }

    void FlipEnemySprite()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingRight = !facingRight;
    }

    void Shoot()
    {
        if (timeBetweenShots <= 0)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }
}
