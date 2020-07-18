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
    [SerializeField] public Vector2 CatKnockbackValue;
    [SerializeField] public Vector2 BiesKnockbackValue;

    private Vector2 moveValue;
    private Vector2 currentMoveValue;



    [SerializeField] DirectionCorrectionY onHitMoveInDirectionVertical = 0;
    [SerializeField] DirectionCorrectionX onHitMoveInDirectionHorizontal = 0;

    [Range( 0.0001f, 10.0f)] public float onHitMoveUpAdditionalFactor = 1;


    enum DirectionCorrectionX{
        Left,
        Right,
        Opposite,
        Same
    }

    enum DirectionCorrectionY{
        Up,
        Down,
        Opposite,
        Same,
    }


    void Start() {
        m_anim     = GetComponent<Animator>();
        m_FloorDetector = GetComponent<CollisionDetector>(); 
    }

    private GlobalUtils.AttackInfo GetAttackInfo( Transform HittedObj ){
        GlobalUtils.AttackInfo infoPack = new GlobalUtils.AttackInfo();

        infoPack.isValid = true;
        infoPack.attackDamage   = damage;

        Vector2 directions = new Vector2( (int)m_FloorDetector.GetCurrentDirection() , Mathf.Sign( CommonValues.PlayerVelocity.y ) );

        if( GlobalUtils.PlayerObject.GetComponent<Player>().GetCurrentFormName() == "Cat" ){
            infoPack.knockBackValue = CatKnockbackValue;
        }else{
            infoPack.knockBackValue = BiesKnockbackValue;
        }

        fillHorizontalInfoPack( ref infoPack, directions.x);
        fillVerticalInfoPack(   ref infoPack, directions.y);

        infoPack.knockBackFrictionX = 10;

        return infoPack; 
    }


    void fillHorizontalInfoPack( ref GlobalUtils.AttackInfo infoPack, float direction ){

        if( onHitMoveInDirectionHorizontal == DirectionCorrectionX.Left ){
            infoPack.knockBackValue.x *= -1;
        }
        if( onHitMoveInDirectionHorizontal == DirectionCorrectionX.Same ){
            infoPack.knockBackValue.x *= direction;
        }
        if( onHitMoveInDirectionHorizontal == DirectionCorrectionX.Opposite ){
            infoPack.knockBackValue.x *= direction * -1;
        }
    }


    void fillVerticalInfoPack( ref GlobalUtils.AttackInfo infoPack, float direction ){

        if( onHitMoveInDirectionVertical == DirectionCorrectionY.Down ){
            infoPack.knockBackValue.y *= -1;
        }
        else if( onHitMoveInDirectionVertical == DirectionCorrectionY.Same ){
            infoPack.knockBackValue.y *= direction;
        }
        else if( onHitMoveInDirectionVertical == DirectionCorrectionY.Opposite ){
            infoPack.knockBackValue.y *= direction * -1;
        }
    }


    void OnTriggerStay2D(Collider2D other){

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
