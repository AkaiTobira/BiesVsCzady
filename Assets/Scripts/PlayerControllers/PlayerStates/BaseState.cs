using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{

    protected GameObject m_controllabledObject;
    protected BaseState  m_nextState = null;

    public BaseState( GameObject controllableObject ){
        m_controllabledObject = controllableObject;
    }

    protected bool m_isOver = false;

    public bool isOver(){
        return m_isOver;
    }

    public virtual void HandleInput(){
    }

    public BaseState NextState(){
        BaseState temp = m_nextState;
        m_nextState = null;
        return temp;
    }

    public  virtual void Process()
    {
    }
}
