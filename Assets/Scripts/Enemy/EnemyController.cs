using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;

    public bool shootingType = false;
    public bool isTriggered = false;
    public bool facingRight = true;

    [Header("Enemy Properties")]
    [Range(15,30)] public int health = 15;
    public float speed;
    public float lookRadius = 7f;
    public float retreatDistance;
    public float meleeRange;
    public float shootRange;

    public Transform player;
    public Animator anim;

    [Header("Bullet")]
    public GameObject projectile;
    public GameObject shootPosition;
    public float shootTimer;
    public float shootCoolDown;
    public bool canShoot;

    void Awake()
    {
        instance = this;    
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        //player = GlobalUtils.PlayerObject;
    }

    void FlipEnemySprite()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingRight = !facingRight;
    }
}
