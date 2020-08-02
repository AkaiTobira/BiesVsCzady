using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{

    [SerializeField] public Animator m_anim;

    [SerializeField] public float durability  = 0;

    [SerializeField] public bool canBeKnockBacked = false;

    [SerializeField] public CollisionDetector m_FloorDetector;

    [SerializeField] public float decreaseFactor;

    [SerializeField] public float massFactor;

    [SerializeField] public float timeToHitApex = 1.0f;

    private float gravityForce = 0.0f;

    private Vector2 moveValue;
    private Vector2 currentMoveValue;


    void Start() {
        m_anim     = GetComponent<Animator>();
        m_FloorDetector = GetComponent<CollisionDetector>(); 
    }

    private void HandleXMove(){
        if( Mathf.Abs( currentMoveValue.x ) > 10.0f){
            if( m_FloorDetector.isOnGround() ){
                float xFactor = Mathf.Abs( currentMoveValue.x );
                xFactor = Mathf.Max( xFactor - (decreaseFactor*massFactor), 0 );
                currentMoveValue.x = xFactor * Mathf.Sign(currentMoveValue.x);
            }
        }else{ 
            currentMoveValue.x = 0;
        };
    }

    private void HandleYMove(){
        if( m_FloorDetector.isOnCelling()){ 
            currentMoveValue.y = 0;
        }else if( !m_FloorDetector.isOnGround() ){
            currentMoveValue.y -= gravityForce * massFactor * Time.deltaTime;
        }else{
            currentMoveValue.y = 0;
            m_FloorDetector.autoGravityOn = true;
        };
    }

    void MoveObject(){
        HandleXMove();
        HandleYMove();
        if( moveValue != new Vector2(0,0)) m_FloorDetector.Move( currentMoveValue * Time.deltaTime);
    }

    void Update() {
        MoveObject();
    }

    void OnTriggerEnter2D(Collider2D other){
        if( durability < 0) return;
        GlobalUtils.AttackInfo infoPack = new GlobalUtils.AttackInfo();

        if( other.tag == "PlayerHitBox" ){
            infoPack = GlobalUtils.PlayerObject.GetComponent<IEntity>().GetAttackInfo();
        }else if( other.tag == "ChaseAttackBox" ){
            infoPack =   other.transform.parent.Find("Animator").
                                                Find("AttackBox").GetComponent<AttackBoxHandler>().
                        mainScript.GetComponent<IEntity>().GetAttackInfo();
        }

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
                }
            }else{
                if( durability < 0 ){
                    m_anim.SetBool("isDestroyed", true);
                    GetComponent<BoxCollider2D>().enabled = false;
                    GetComponent<CollisionDetector>().enabled = false;
                }
            }
        }
    }

}
