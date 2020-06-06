using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiesBalance : MonoBehaviour
{
 [Header("Bies")]
    [SerializeField] float minJumpHeight         = 5.0f;
    [SerializeField] float targetJumpHeight      = 0.0f;
    [SerializeField] public float timeToJumpApex = 0.0f;
    [SerializeField] float moveDistance          = 15.0f;
    [SerializeField] float moveDistanceInAir     = 5.0f;
    [SerializeField] float maxMoveDistanceInAir     = 10.0f;

    [SerializeField] float Attack1Damage = 2;
    [SerializeField] Vector2 KnockBackValueAttack1 =  new Vector2( 100, 1000);
    [SerializeField] float Attack2Damage = 0;
    [SerializeField] Vector2 KnockBackValueAttack2 =  new Vector2( 100, 400);
    [SerializeField] float Attack3Damage = 5;
    [SerializeField] Vector2 KnockBackValueAttack3 =  new Vector2( 100, 400);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BiesUtils.GravityForce          = (2 * targetJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
        BiesUtils.PlayerJumpForceMin    = Mathf.Sqrt (2 * Mathf.Abs (BiesUtils.GravityForce) * minJumpHeight);
        BiesUtils.PlayerSpeed           = moveDistance;
        BiesUtils.PlayerJumpForceMax    = Mathf.Abs(BiesUtils.GravityForce) * timeToJumpApex;
        BiesUtils.JumpMaxTime           = timeToJumpApex;
        BiesUtils.MoveSpeedInAir        = moveDistanceInAir;
        BiesUtils.maxMoveDistanceInAir     = maxMoveDistanceInAir;
        BiesUtils.Attack1Damage         = Attack1Damage;
        BiesUtils.Attack2Damage         = Attack2Damage;
        BiesUtils.Attack3Damage         = Attack3Damage;
        BiesUtils.KnockBackValueAttack1 = KnockBackValueAttack1;
        BiesUtils.KnockBackValueAttack2 = KnockBackValueAttack2;
        BiesUtils.KnockBackValueAttack3 = KnockBackValueAttack3;
    }
}
