using UnityEngine;

public abstract class IBaseState
{
    protected GameObject m_controllabledObject;

    protected IBaseState  m_nextState;

    protected Animator m_animator;

    public string name = ""; 

    protected bool m_isOver = false;
    public bool isOver(){ return m_isOver; }
    protected GlobalUtils.Direction m_dir;

    protected bool isRightOriented(){
        return m_dir == GlobalUtils.Direction.Right;
    }

    protected bool isLeftOriented(){
        return m_dir == GlobalUtils.Direction.Left;
    }

    public virtual GlobalUtils.Direction GetDirection(){
        return m_dir;
    }

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
