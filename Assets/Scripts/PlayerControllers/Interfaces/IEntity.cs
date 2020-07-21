using UnityEngine;

public abstract class IEntity : MonoBehaviour
{
    protected ISFSMBase m_controller;
    protected ICollisionFloorDetector m_FloorDetector;
    protected Animator m_animator;

    public abstract GlobalUtils.AttackInfo GetAttackInfo();
    public virtual string GetCurrentState(){
        return m_controller.GetStateName();
    }
    public abstract void OnHit( GlobalUtils.AttackInfo infoPack );

}
