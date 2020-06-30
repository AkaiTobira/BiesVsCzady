using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFSMBase
{
    public GameObject m_controllabledObject { get; private set; }
    protected Stack<BaseState> m_states = new Stack<BaseState>();

    public SFSMBase ( GameObject controlledObj, BaseState baseState ){
        m_controllabledObject = controlledObj;
        m_states.Push(baseState);
    }

    private void cleanStack(){
        while( m_states.Peek().isOver() ) m_states.Pop().OnExit();
    }

    private void processStack(){
        BaseState current_state = m_states.Peek();
        if( current_state.isOver() ) return;
        current_state.HandleInput();
        current_state.Process();
        current_state.UpdateAnimator();
    }

    public string GetStateName(){
        return  m_states.Peek().name;
    }

    private void switchState(){
        BaseState nextState = m_states.Peek().NextState();
        if( nextState == null ) return;
        m_states.Push(nextState);
        m_states.Peek().OnEnter();
    }

    public GlobalUtils.Direction GetDirection(){
        return m_states.Peek().GetDirection();
    }

    public virtual void Update(){
        cleanStack();
        processStack();
        switchState();
    }

    public virtual string GetCurrentForm(){
        return "";
    }

    public virtual void OverriteStates( string targetState, GlobalUtils.AttackInfo attackInfo = new GlobalUtils.AttackInfo() ){

    }

}
