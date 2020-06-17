using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class  PlayerChangeRules
{

    const float SHIFT_BASED_ON_SCALE = 138.0f;

    private static List<string> triggersList = new List<string>{
        "SwitchToCatIdle",
        "SwitchToBiesIdle"
    };

    public static void ChangeAnimation( string formName, string stateName, 
                                        GlobalUtils.Direction dir // This Argument will be removed
    ){
        
        Animator playerAnimator = GlobalUtils.PlayerObject
                                .GetComponent<Player>().animationNode
                                .GetComponent<Animator>();
        foreach( string trigger in triggersList){
            playerAnimator.ResetTrigger( trigger );
        }

        ScalePlayer( formName );
        PositionCorrenciton( formName, stateName);

        playerAnimator.SetTrigger( "SwitchTo" + formName + "Idle");
        // Target when animations will be done :
        //playerAnimator.SetTrigger( "SwitchTo" + formName+ stateName );
    }

    private static void ScalePlayer( string formName){
        if( formName.Contains("Cat") ){
            GlobalUtils.PlayerObject.localScale = new Vector3(120, 120, 75);
        }else{
            GlobalUtils.PlayerObject.localScale = new Vector3(180, 180, 100);
        }
    }

    private static void PositionCorrenciton( string formName, string stateName){
        
        if( formName.Contains("Bies")){
            var playerDetector = GlobalUtils.PlayerObject.GetComponent<CollisionDetectorPlayer>();
            Vector2 Translation = new Vector2();

            float distanceToWall = playerDetector.GetDistanceToClosestWall();

            if( stateName.Contains("Hold") || stateName.Contains("Slide") ){
                Translation = new Vector2(-SHIFT_BASED_ON_SCALE, 0 ) * (int)playerDetector.GetCurrentDirection();
            }
            
            if( distanceToWall < SHIFT_BASED_ON_SCALE ){
                Translation = new Vector2(-SHIFT_BASED_ON_SCALE, 0 ) * (int)playerDetector.GetCurrentDirection();
            }

            Debug.Log(Translation);
            playerDetector.CheatMove(Translation);
        }
    }

    public static string ChangeFormName( string formName){
        switch(formName){
            case "Cat" : return "Bies";
            case "Bies": return "Cat";
            default :    return "InvalidName";
        }
    }

    public static bool CanTransformInCurrentState( string currentStateName ){
//        Debug.Log( currentStateName.Contains("LedgeClimb") );
        if( currentStateName.Contains("LedgeClimb"))  return false;
        return true;
    }

    public static BaseState GetIdleState( string formName ){
        switch(formName){
            case "Cat" : return new CatIdle ( GlobalUtils.PlayerObject.gameObject );
            case "Bies": return new BiesIdle( GlobalUtils.PlayerObject.gameObject );
            default :    return null;
        }
    }

    private static BaseState CatToBiesTranslation(ref string stateName, ref GlobalUtils.Direction dir){
        switch( stateName ){
            case "Idle": return new BiesIdle( GlobalUtils.PlayerObject.gameObject );
            case "Move": return new BiesMove( GlobalUtils.PlayerObject.gameObject, dir);
            case "Fall": return new BiesFall( GlobalUtils.PlayerObject.gameObject, dir);
            case "Jump": return new BiesFall( GlobalUtils.PlayerObject.gameObject, dir);
            case "WallHold" : return new BiesWallHold( GlobalUtils.PlayerObject.gameObject, dir);
            case "JumpWall" : return new BiesFall( GlobalUtils.PlayerObject.gameObject, dir);
            case "JumpSlide": return new BiesFall( GlobalUtils.PlayerObject.gameObject, dir);
            case "WallClimb": return new BiesFall( GlobalUtils.PlayerObject.gameObject, dir);
        //    case "Attack2"  : return new BiesAttack2( GlobalUtils.PlayerObject.gameObject);
            default : return null;
        }
    }

    private static BaseState BiesToCatTranslation(ref string stateName, ref GlobalUtils.Direction dir){
        switch( stateName ){
            case "Idle": return new CatIdle( GlobalUtils.PlayerObject.gameObject );
            case "Move": return new CatMove( GlobalUtils.PlayerObject.gameObject, dir);
            case "Fall": return new CatFall( GlobalUtils.PlayerObject.gameObject, dir);
            case "Jump": return new CatFall( GlobalUtils.PlayerObject.gameObject, dir);
            case "WallHold" : return new CatFall( GlobalUtils.PlayerObject.gameObject, dir);
            case "PullObj"  : return new CatFall( GlobalUtils.PlayerObject.gameObject, dir);
            case "PushObj"  : return new CatFall( GlobalUtils.PlayerObject.gameObject, dir);
            case "Attack1"  : return new CatIdle( GlobalUtils.PlayerObject.gameObject);
            case "Attack2"  : return new CatIdle( GlobalUtils.PlayerObject.gameObject);
            case "Attack3"  : return new CatFall( GlobalUtils.PlayerObject.gameObject, dir);
            default : return null;
        }
    }

    public static BaseState TranslateActiveState( string formName, string stateName, GlobalUtils.Direction dir ){
        Debug.Log( "TRanslation info :" + formName + " " + stateName);
        
        switch( formName ){
            case "Bies" : return CatToBiesTranslation(ref stateName, ref dir);
            case "Cat"  : return BiesToCatTranslation(ref stateName, ref dir);
            default :    return null;
        }
    }

}
