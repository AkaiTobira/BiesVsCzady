using UnityEngine;

public abstract class IBaseState : IDirectionInfo
{
    protected GameObject m_controllabledObject;

    protected IBaseState  m_nextState;

    protected Animator m_animator;

    protected ICollisionFloorDetector m_FloorDetector;

    public string name = ""; 

    protected bool m_isOver = false;
    public bool isOver(){ return m_isOver; }

    public  virtual void Process(){}

    public IBaseState GetNextState(){
        IBaseState temp = m_nextState;
        m_nextState     = null;
        return temp;
    }

    public virtual void UpdateAnimator(){}

    public virtual void OnExit(){}
    public virtual void OnEnter(){}

    public virtual void HandleInput(){}
}
