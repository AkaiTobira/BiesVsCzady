using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{

    [SerializeField] public Animator m_anim;

    [SerializeField] public float durability  = 0;

    [SerializeField] public bool canBeKnockBacked = false;

    [SerializeField] public CollisionDetector m_detector;

    [SerializeField] public float decreaseFactor;

    private Vector2 moveValue;
    private Vector2 currentMoveValue;

    private float Gravity = 0.0f;

    void Start() {
        m_anim     = GetComponent<Animator>();
        m_detector = GetComponent<CollisionDetector>(); 
    }


    void MoveObject(){
        if( currentMoveValue.magnitude > 5.0f){
            m_detector.autoGravityOn = false;
            Gravity -= PlayerUtils.GravityForce * Time.deltaTime;
            currentMoveValue -= moveValue * decreaseFactor * Time.deltaTime;
            m_detector.Move((currentMoveValue + new Vector2( 0, Gravity)) * Time.deltaTime);
        }else{
            Gravity = 0.0f;
            m_detector.autoGravityOn = true;
        }
    }

    void Update() {
        MoveObject();
    }

    void OnTriggerEnter2D(Collider2D other){

        if( other.tag == "PlayerHitBox"){

            if( durability < 0) return;
            GlobalUtils.AttackStateInfo infoPack =  
                GlobalUtils.PlayerObject.GetComponent<Player>().GetPlayerAttackInfo();
            
            if( infoPack.isValid){
                durability -= infoPack.attackDamage;
                if( m_anim == null){
                    if( durability < 0 ){
                        Destroy(gameObject);
                    }else if( canBeKnockBacked ){
                        moveValue    = infoPack.knockBackValue;
                        moveValue.x *=  (int)infoPack.fromCameAttack;
                        currentMoveValue += moveValue;
                        Debug.Log( moveValue );
                    }
                }else{
                    if( durability < 0 ){
                        m_anim.SetBool("isDestroyed", true);
                        //ForExample
                    }
                }
            }
        }
    }

}
