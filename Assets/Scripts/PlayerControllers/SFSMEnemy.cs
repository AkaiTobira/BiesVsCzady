using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFSMEnemy : ISFSMBase
{
    public SFSMEnemy ( GameObject controlledObj, IBaseState PlayerBaseState ) : 
        base(controlledObj, PlayerBaseState )
    {}

    public void StackStatusPrint(){
        string stackInfo = "";
        foreach( PlayerBaseState b in m_states ){
            stackInfo += b.name + " : " + b.isOver() + " :  " + b.GetDirection().ToString() + "\n";
        }
        GlobalUtils.debugConsole.text = stackInfo;
    }



    public override void OverriteStates(string targetState, GlobalUtils.AttackInfo attackInfo){
/*
        string currentStateName = RemoveDirectionInfo(GetStateName());
        string currentFormName  = GetCurrentFormName(currentStateName);
        if( GetStateName().Contains("Dead")) return;
        if( GetStateName().Contains("Hurt")) return;
        
        currentStateName = RemoveFormName( currentStateName);
        m_states.Clear();
        m_states.Push( PlayerChangeRules.GetIdleState(currentFormName) );
        
        switch( targetState){
            case "Hurt" : 
                switch( currentFormName ){
                    case "Bies" : 
                        m_states.Push( new BiesHurt(m_controllabledObject, attackInfo));
                    break;
                    case "Cat" :
                        m_states.Push( new CatHurt(m_controllabledObject, attackInfo));
                    break;
                }
                break;
            case "Stun" : 
                switch( currentFormName ){
                    case "Bies" : 
                        m_states.Push( new BiesStun(m_controllabledObject, attackInfo));
                    break;
                    case "Cat" :
                        m_states.Push( new CatStun(m_controllabledObject, attackInfo));
                    break;
                }
                break;
            case "Dead" : 
                switch( currentFormName ){
                    case "Bies" : 
                        m_states.Push( new BiesDead(m_controllabledObject, attackInfo));
                    break;
                    case "Cat" :
                        m_states.Push( new CatDead(m_controllabledObject, attackInfo));
                    break;
                }
                break;
        }
  */
    }
    
    private string RemoveDirectionInfo( string stateName ){
        if( stateName.EndsWith("L") || stateName.EndsWith("R")){
            return stateName.Substring( 0, stateName.Length-1);
        }
        return stateName;
    }
/*
    public  string GetCurrentForm(){
        return GetCurrentFormName( GetStateName() );
    }
*/

    public override GlobalUtils.Direction GetDirection(){
        return m_states.Peek().GetDirection();
    }
}
