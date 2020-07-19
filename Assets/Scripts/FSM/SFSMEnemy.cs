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

        string currentStateName = GetStateName();

        if( currentStateName.Contains("Dead")) return;

        while( m_states.Count != 1 ) m_states.Pop();

        switch( targetState ) {
            case "FlyingCombatEngage" : 
                m_states.Push( new FlyingCzadPlayerDetected( m_controllabledObject ));
            break;
            case "FlyingHurt" : 
                m_states.Push( new FlyingCzadPlayerDetected( m_controllabledObject ));
                m_states.Push( new CzadHurt(m_controllabledObject, attackInfo));
            break;
            case "FlyingStun" : 
                m_states.Push( new FlyingCzadPlayerDetected( m_controllabledObject ));
                m_states.Push( new CzadStun(m_controllabledObject, attackInfo));
            break;
            case "CombatEngage" : 
                m_states.Push( new CzadPlayerDetected( m_controllabledObject ));
            break;
            case "Hurt" :
                m_states.Push( new CzadPlayerDetected( m_controllabledObject ));
                m_states.Push( new CzadHurt(m_controllabledObject, attackInfo));
            break;
            case "Stun" :
                m_states.Push( new CzadPlayerDetected( m_controllabledObject ));
                m_states.Push( new CzadStun(m_controllabledObject, attackInfo));
            break;
            case "Dead":
                m_states.Push(new CzadDead(m_controllabledObject, attackInfo));
            break;
            default :
                Debug.Log( targetState + " :: Not found");
            break;
        }
    }
    
    private string RemoveDirectionInfo( string stateName ){
        if( stateName.EndsWith("L") || stateName.EndsWith("R")){
            return stateName.Substring( 0, stateName.Length-1);
        }
        return stateName;
    }

    public override GlobalUtils.Direction GetDirection(){
        return m_states.Peek().GetDirection();
    }
}
