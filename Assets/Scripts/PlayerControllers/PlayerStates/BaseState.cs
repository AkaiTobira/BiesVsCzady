using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    protected GameObject m_controllabledObject;
    protected CollisionDetectorPlayer m_detector;
    protected BaseState  m_nextState = null;

    public string name = ""; 
    protected Vector2    velocity = new Vector2(0,0); 

    public BaseState( GameObject controllableObject ){
        m_controllabledObject = controllableObject;
        m_detector            = controllableObject.GetComponent<CollisionDetectorPlayer>();
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

    public virtual void OnExit(){
        velocity = new Vector2();
    }

}
