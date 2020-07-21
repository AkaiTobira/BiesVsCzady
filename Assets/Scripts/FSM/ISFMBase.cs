using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ISFSMBase
{
    public GameObject m_controllabledObject { get; private set; }
    protected Stack<IBaseState> m_states = new Stack<IBaseState>();

    public ISFSMBase(GameObject controlledObj, IBaseState baseState ){
        m_controllabledObject = controlledObj;
        m_states.Push(baseState);
    }

    public string GetStackStatus( ){
        string stackInfo = "";
        foreach( IBaseState b in m_states ){
            stackInfo += b.name + " : " + b.isOver() + " :  " + b.GetDirection().ToString() + "\n";
        }
        return stackInfo;
    }

    private void cleanStack(){
        while( m_states.Peek().isOver() ) m_states.Pop().OnExit();
    }

    protected virtual void processStack(){
        IBaseState current_state = m_states.Peek();
        if( current_state.isOver() ) return;
        current_state.Process();
        current_state.UpdateAnimator();
    }

    public string GetStateName(){
        return  m_states.Peek().name;
    }

    private void switchState(){
        IBaseState nextState = m_states.Peek().GetNextState();
        if( nextState == null ) return;
        m_states.Push(nextState);
    }

    public virtual GlobalUtils.Direction GetDirection(){
        return m_states.Peek().GetDirection();
    }

    public virtual void Update(){
        cleanStack();
        processStack();
        switchState();
    }

    public virtual string StackStatusPrint(){
        return "";
    }

    public virtual void OverriteStates( string targetState, GlobalUtils.AttackInfo attackInfo = new GlobalUtils.AttackInfo() ){}

}
