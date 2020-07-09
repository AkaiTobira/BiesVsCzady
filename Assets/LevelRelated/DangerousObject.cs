using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerousObject : MonoBehaviour
{

    [SerializeField] public Animator m_anim;

    [SerializeField] public float durability  = 0;

    [SerializeField] public bool canBeKnockBacked = false;

    [SerializeField] public CollisionDetector m_FloorDetector;

    [SerializeField] public float decreaseFactor;

    [SerializeField] public float massFactor;

    [SerializeField] public float timeToHitApex = 1.0f;

    [SerializeField] public float damage = 3;
    [SerializeField] public Vector2 CatKnockback = new Vector2( 0, 0);
    [SerializeField] public Vector2 BiesKnockback = new Vector2( 0, 0);

    private Vector2 moveValue;
    private Vector2 currentMoveValue;


    void Start() {
        m_anim     = GetComponent<Animator>();
        m_FloorDetector = GetComponent<CollisionDetector>(); 
    }

    private GlobalUtils.AttackInfo GetAttackInfo( Transform HittedObj ){
        GlobalUtils.AttackInfo infoPack = new GlobalUtils.AttackInfo();

        infoPack.isValid = true;
        infoPack.fromCameAttack     = ( HittedObj.position.x < transform.position.x ) ? GlobalUtils.Direction.Left : GlobalUtils.Direction.Right;
        infoPack.attackDamage       = damage;

        if( GlobalUtils.PlayerObject.GetComponent<Player>().GetCurrentFormName() == "Cat" ){
            infoPack.knockBackValue     = CatKnockback;
        }else{
            infoPack.knockBackValue     = BiesKnockback;
        }


        infoPack.knockBackFrictionX = 10;

        return infoPack; 
    }


    void OnTriggerEnter2D(Collider2D other){

        if( other.tag == "PlayerHurtBox"){

            var infoPack = GetAttackInfo( other.transform ); 
            GlobalUtils.PlayerObject.GetComponent<Player>().OnHit( infoPack );
//            other.gameObject.GetComponent<Player>().OnHit( infoPack );

    /*        if( durability < 0) return;
            GlobalUtils.AttackInfo infoPack =  
                GlobalUtils.PlayerObject.GetComponent<Player>().GetAttackInfo();
            
            if( infoPack.isValid){
                durability -= infoPack.attackDamage;
                if( m_anim == null){
                    if( durability < 0 ){
                        Destroy(gameObject);
                    }else if( canBeKnockBacked ){
                        moveValue    = infoPack.knockBackValue;
                        moveValue.x *=  (int)infoPack.fromCameAttack;
                        currentMoveValue += moveValue;
                        m_FloorDetector.autoGravityOn = false;
                        m_FloorDetector.CheatMove( new Vector2(0,  moveValue.y * Time.deltaTime) );
                        m_FloorDetector.Move(  new Vector2(0,  moveValue.y * Time.deltaTime) );
                        gravityForce = (2 * moveValue.y) / Mathf.Pow (timeToHitApex, 2);
                        Debug.Log( moveValue );
                    }
                }else{
                    if( durability < 0 ){
                        m_anim.SetBool("isDestroyed", true);
                        //ForExample
                    }
                }
            }

            */
        }
    }

}
