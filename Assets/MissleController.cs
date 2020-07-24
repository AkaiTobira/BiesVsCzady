using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleController : IEntity
{
    public Vector3 direction = new Vector3();
    public float speed = 0;

    public int AttackDamage = 3;

    void Start()
    {
        m_FloorDetector = GetComponent<CollisionDetector>();
    }

    public override GlobalUtils.AttackInfo GetAttackInfo(){
        GlobalUtils.AttackInfo infoPack = new GlobalUtils.AttackInfo();
        infoPack.isValid      = true;
        infoPack.attackDamage = AttackDamage;
        return infoPack;
    }

    public void SetRotationZ( float angle ){
        transform.GetChild(0).transform.eulerAngles = new Vector3( 0, 0, angle);
    }

    public override void OnHit( GlobalUtils.AttackInfo infoPack){
        Destroy(gameObject);
    }

    void Update()
    {
        m_FloorDetector.Move( direction * speed * Time.deltaTime );
        if( m_FloorDetector.isOnGround() ) Destroy(gameObject);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if( other.tag == "PlayerHurtBox"){
            var infoPack = GetAttackInfo( ); 
            GlobalUtils.PlayerObject.GetComponent<Player>().OnHit( infoPack );
        }
    }


}
