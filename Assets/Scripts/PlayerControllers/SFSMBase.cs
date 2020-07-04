using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFSMBase
{
    public GameObject m_controllabledObject { get; private set; }
    protected Stack<IBaseState> m_states = new Stack<IBaseState>();

    public SFSMBase ( GameObject controlledObj, IBaseState IBaseState ){
        m_controllabledObject = controlledObj;
        m_states.Push(IBaseState);
    }

    private void cleanStack(){
        while( m_states.Peek().isOver() ) m_states.Pop().OnExit();
    }

    private void processStack(){
        IBaseState current_state = m_states.Peek();
        if( current_state.isOver() ) return;
        current_state.HandleInput();
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

    public virtual string GetCurrentForm(){
        return "";
    }

    public virtual void OverriteStates( string targetState, GlobalUtils.AttackInfo attackInfo = new GlobalUtils.AttackInfo() ){

    }

}
