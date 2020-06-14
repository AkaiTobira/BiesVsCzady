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
        current_state.UpdateDirection();
    }

    public string GetStateName(){
        return  m_states.Peek().name;
    }

    private void switchState(){
        BaseState nextState = m_states.Peek().NextState();
        if( nextState != null ) m_states.Push(nextState);
    }

    public GlobalUtils.Direction GetDirection(){
        return m_states.Peek().GetDirection();
    }

    public virtual void Update(){
        cleanStack();
        processStack();
        switchState();
    }
}
