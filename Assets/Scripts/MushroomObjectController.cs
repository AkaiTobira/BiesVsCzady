using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MushroomSaveInfo : GeneralObjectInfoPack{

}


public class MushroomObjectController : MonoBehaviour
{

    int baseChildNumber = 0;

    [SerializeField] GameObject childPrefab;

    Dictionary<int, MushroomSaveInfo> lastSavedObjects = new Dictionary<int, MushroomSaveInfo>();

    void Start()
    {
        baseChildNumber = transform.childCount;
        for( int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).GetComponent<ObjectIdHolder>().objectId = i;
        }
        SaveAllObjects();
    }

    MushroomSaveInfo fillObjectInfo( Transform go){

        MushroomSaveInfo infoPack = new MushroomSaveInfo();
        infoPack.saveBasics( go );

    /*    infoPack.pullFriction = go.GetComponent<CollisionDetectorMovable>().PullFriction;
        infoPack.pushFriction = go.GetComponent<CollisionDetectorMovable>().PushFriction;
        infoPack.gravity      = go.GetComponent<CollisionDetectorMovable>().GravityForce;
        infoPack.maxGravity   = go.GetComponent<CollisionDetectorMovable>().MaxGravityForce;
*/
        return infoPack;
    }

    public void SaveAllObjects(){
        lastSavedObjects = new Dictionary<int, MushroomSaveInfo>();
        for( int i = 0; i < transform.childCount; i++){
            Transform child = transform.GetChild(i);
            if( !child.GetComponent<AutoDestroyablePlatform>().isActivated() )
            lastSavedObjects[child.GetComponent<ObjectIdHolder>().objectId] = fillObjectInfo( child );
        }
    }

    public void ResetChild( int i, int index)
    {
        MushroomSaveInfo info = lastSavedObjects[index];

        transform.GetChild(i).position = info.position;
        transform.GetChild(i).rotation = info.rotation;
        transform.GetChild(i).localScale = info.scale;

        transform.GetChild(i).GetComponent<AutoDestroyablePlatform>().ResetMushroom();

/*
        transform.GetChild(i).GetComponent<CollisionDetectorMovable>().PushFriction = info.pushFriction;
        transform.GetChild(i).GetComponent<CollisionDetectorMovable>().PullFriction = info.pullFriction;
        transform.GetChild(i).GetComponent<CollisionDetectorMovable>().GravityForce = info.gravity;
        transform.GetChild(i).GetComponent<CollisionDetectorMovable>().MaxGravityForce = info.maxGravity;
*/
    }


    public void LoadAllObjects(){
        if( transform.childCount == baseChildNumber){
            for( int i = 0; i < transform.childCount; i++){
                int childIndex  = transform.GetChild(i).GetComponent<ObjectIdHolder>().objectId;
                ResetChild( i, childIndex);
            }
        }
    }
}
