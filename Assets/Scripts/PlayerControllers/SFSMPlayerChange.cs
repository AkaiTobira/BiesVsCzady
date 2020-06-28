using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFSMPlayerChange : SFSMBase
{
    public SFSMPlayerChange ( GameObject controlledObj, BaseState baseState ) : base(controlledObj, baseState )
    {}

    public void StackStatusPrint(){
        string stackInfo = "";
        foreach( BaseState b in m_states ){
            stackInfo += b.name + " : " + b.isOver() + " :  " + b.GetDirection().ToString() + "\n";
        }
        GlobalUtils.debugConsole.text = stackInfo;
    }

    public override void Update(){
        base.Update();
        ProcessCharacterChange();
        StackStatusPrint();
    }

    public override void OverriteStates(string targetState, GlobalUtils.AttackInfo attackInfo){

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
    }
    
    private string RemoveDirectionInfo( string stateName ){
        if( stateName.EndsWith("L") || stateName.EndsWith("R")){
            return stateName.Substring( 0, stateName.Length-1);
        }
        return stateName;
    }

    private string GetCurrentFormName( string stateName){
        if( stateName.StartsWith("Cat"))  return "Cat";
        if( stateName.StartsWith("Bies")) return "Bies";
        return "InvalidStateName";
    }

    private string RemoveFormName( string stateName ){
        if( stateName.StartsWith("Cat"))  return stateName.Substring(3, stateName.Length-3);
        if( stateName.StartsWith("Bies")) return stateName.Substring(4, stateName.Length-4);
        return "InvalidStateName";
    }

    private void ProcessCharacterChange(){
        if( ! PlayerInput.isChangeFormKeyJustPressed() ) return;
        string currentStateName = RemoveDirectionInfo(GetStateName());
        string currentFormName  = GetCurrentFormName(currentStateName);
        currentStateName = RemoveFormName( currentStateName);
        if( !PlayerChangeRules.CanTransformInCurrentState( currentStateName )) return;
        GlobalUtils.Direction currentDirection = m_states.Peek().GetDirection();
        m_states.Clear();
        currentFormName = PlayerChangeRules.ChangeFormName( currentFormName );
        m_states.Push( PlayerChangeRules.GetIdleState(currentFormName) );
        BaseState newState = PlayerChangeRules.TranslateActiveState( currentFormName, currentStateName, currentDirection);
        PlayerChangeRules.ChangeAnimation( currentFormName, currentStateName, currentDirection);
        if( newState == null || newState.name.Contains("Idle") ) return;
        m_states.Push( newState );
    }


}
