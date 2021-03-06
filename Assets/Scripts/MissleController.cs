﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleController : IEntity
{
    public Vector3 direction = new Vector3();
    public float speed = 0;

    public int AttackDamage = 3;

    private ICollisionWallDetector m_wallDetector;

    [SerializeField] private Vector2 knockbackInfo = new Vector2(); 

    void Start()
    {
        m_FloorDetector = GetComponent<CollisionDetectorMissle>();
        m_wallDetector  = GetComponent<CollisionDetectorMissle>();
        m_animator      = transform.GetChild(0).GetComponent<Animator>();
    }

    private bool canDeadDamage = true;

    public override GlobalUtils.AttackInfo GetAttackInfo(){

        GlobalUtils.AttackInfo infoPack = new GlobalUtils.AttackInfo();
        infoPack.isValid        = canDeadDamage;
        infoPack.attackDamage   = AttackDamage;
        infoPack.knockBackValue = knockbackInfo;
        infoPack.lockFaceDirectionDuringKnockback = true;
        infoPack.fromCameAttack = (GlobalUtils.Direction)Mathf.Sign( direction.x );
        return infoPack;
    }

    public void SetRotationZ( float angle ){
        transform.GetChild(0).transform.eulerAngles = new Vector3( 0, 0, angle);
    }

    private void OnHittt(){
        canDeadDamage = false;
        direction = new Vector3();
        m_animator.SetTrigger("isHit");
        Destroy(gameObject, 1.0f);
    }


    public override void OnHit( GlobalUtils.AttackInfo infoPack){
        OnHittt();
    }

    void Update()
    {
        m_FloorDetector.Move( direction * speed * Time.deltaTime );
        if( m_FloorDetector.isOnGround() || m_wallDetector.isCollideWithLeftWall() ) {
            
            OnHittt();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if( other.tag == "PlayerHurtBox"){
            var infoPack = GetAttackInfo( ); 
            GlobalUtils.PlayerObject.GetComponent<Player>().OnHit( infoPack );
            OnHittt();
        }
    }


}
