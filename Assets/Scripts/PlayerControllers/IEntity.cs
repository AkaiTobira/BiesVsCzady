using UnityEngine;

public abstract class IEntity : MonoBehaviour
{
    protected ISFSMBase m_controller;
    protected ICollisionFloorDetector m_FloorDetector;
    protected Animator m_animator;

    public virtual GlobalUtils.AttackInfo GetAttackInfo(){
        GlobalUtils.AttackInfo infoPack = new GlobalUtils.AttackInfo();
        infoPack.isValid = false;
        return infoPack;
    }

    public virtual void OnHit( GlobalUtils.AttackInfo infoPack ){}

}
