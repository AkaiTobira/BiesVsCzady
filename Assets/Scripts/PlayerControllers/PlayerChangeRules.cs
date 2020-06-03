using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class  PlayerChangeRules
{

    public static string ChangeFormName( string formName){
        switch(formName){
            case "Cat" : return "Bies";
            case "Bies": return "Cat";
            default :    return "InvalidName";
        }
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
            case "Jump": return new BiesJump( GlobalUtils.PlayerObject.gameObject, dir);
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
            case "Jump": return new CatJump( GlobalUtils.PlayerObject.gameObject, dir);
            case "WallHold" : return new CatWallHold( GlobalUtils.PlayerObject.gameObject, dir);
            case "PullObj"  : return new CatWallHold( GlobalUtils.PlayerObject.gameObject, dir);
            case "PushObj"  : return new CatWallHold( GlobalUtils.PlayerObject.gameObject, dir);
            case "Attack1"  : return new CatIdle( GlobalUtils.PlayerObject.gameObject);
            case "Attack2"  : return new CatIdle( GlobalUtils.PlayerObject.gameObject);
            case "Attack3"  : return new CatFall( GlobalUtils.PlayerObject.gameObject, dir);
            default : return null;
        }
    }

    public static BaseState TranslateActiveState( string formName, string stateName, GlobalUtils.Direction dir ){
        switch( formName ){
            case "Cat" : return CatToBiesTranslation(ref stateName, ref dir);
            case "Bies": return BiesToCatTranslation(ref stateName, ref dir);
            default :    return null;
        }
    }


}
