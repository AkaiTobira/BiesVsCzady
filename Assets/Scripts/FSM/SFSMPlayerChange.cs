using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFSMPlayerChange : ISFSMBase
{
    public SFSMPlayerChange ( GameObject controlledObj, PlayerBaseState PlayerBaseState ) : 
        base(controlledObj, PlayerBaseState )
    {}

    public override string StackStatusPrint(){
        string stackInfo = "";
        foreach( PlayerBaseState b in m_states ){
            stackInfo += b.name + " : " + b.isOver() + " :  " + b.GetDirection().ToString() + "\n";
        }
        return stackInfo;
    }

    protected override void processStack(){
        base.processStack();
        PlayerBaseState current_state = (PlayerBaseState) m_states.Peek();
        if( current_state.isOver() ) return;
        current_state.HandleInput();
    }

    public override void Update(){
        base.Update();
        ProcessCharacterChange();
        UpdateTutorialInfo();
    }


    private void UpdateTutorialInfo(){
        GlobalUtils.TutorialConsole.text = m_states.Peek().GetTutorialAdvice();
    }


    public override void OverriteStates(string targetState, GlobalUtils.AttackInfo attackInfo){

        string currentStateName = RemoveDirectionInfo(GetStateName());
        string currentFormName  = GetCurrentFormName(currentStateName);
        
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

    public  string GetCurrentForm(){
        return GetCurrentFormName( GetStateName() );
    }


    public override GlobalUtils.Direction GetDirection(){
        return m_states.Peek().GetDirection();
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
        
        FMODUnity.RuntimeManager.PlayOneShot( "event:/SFX/Kot/kot-bies-przemiana", GlobalUtils.PlayerObject.position);


        GlobalUtils.Direction currentDirection = m_states.Peek().GetDirection();
        m_states.Clear();
        currentFormName = PlayerChangeRules.ChangeFormName( currentFormName );
        m_states.Push( PlayerChangeRules.GetIdleState(currentFormName) );
        
        PlayerBaseState newState = PlayerChangeRules.TranslateActiveState( currentFormName, currentStateName, currentDirection);
        
        if( newState != null ){
           currentStateName = RemoveFormName( RemoveDirectionInfo( newState.name ));
           if( !newState.name.Contains("Idle") ) m_states.Push( newState );
        }else if( newState.name.Contains("Idle") ){
            currentStateName = "Idle";    
        }
        PlayerChangeRules.ChangeAnimation( currentFormName, currentStateName, currentDirection);
        
    }
}
