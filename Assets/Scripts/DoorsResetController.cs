using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSaveInfo : GeneralObjectInfoPack{

    public GeneralObjectInfoPack playerDetectorValues = new GeneralObjectInfoPack();

    public int requiredNumberOfKeys;

}


public class DoorsResetController : MonoBehaviour
{

    int baseChildNumber = 0;

    [SerializeField] GameObject childPrefab = null;

    Dictionary<int, DoorSaveInfo> lastSavedObjects = new Dictionary<int, DoorSaveInfo>();

    void Start()
    {
        baseChildNumber = transform.childCount;
        for( int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).GetComponent<ObjectIdHolder>().objectId = i;
        }
        SaveAllObjects();
    }

    DoorSaveInfo fillObjectInfo( Transform go){

        DoorSaveInfo infoPack = new DoorSaveInfo();
        infoPack.saveBasics( go );
        infoPack.playerDetectorValues.saveBasics( go.GetChild(0) );
        infoPack.requiredNumberOfKeys = go.GetChild(0).GetComponent<DoorController>().numberOfRequiredKeys;
        return infoPack;
    }

    public void SaveAllObjects(){
        lastSavedObjects = new Dictionary<int, DoorSaveInfo>();
        for( int i = 0; i < transform.childCount; i++){
            Transform child = transform.GetChild(i);
            if( !child.GetChild(0).GetComponent<DoorController>() ) continue;
            lastSavedObjects[child.GetComponent<ObjectIdHolder>().objectId] = fillObjectInfo( child );
        }
    }

    public void ResetChild( int i, int index)
    {
        DoorSaveInfo info = lastSavedObjects[index];
        transform.GetChild(i).position = info.position;
        transform.GetChild(i).rotation = info.rotation;
        transform.GetChild(i).localScale = info.scale;

        transform.GetChild(i).GetChild(0).position   = info.playerDetectorValues.position;
        transform.GetChild(i).GetChild(0).rotation   = info.playerDetectorValues.rotation;
        transform.GetChild(i).GetChild(0).localScale = info.playerDetectorValues.scale;

    }

    public void LoadAllObjects(){

            for( int i = 0; i < transform.childCount; i++){
                var node = transform.GetChild(i);
                int childIndex  = node.GetComponent<ObjectIdHolder>().objectId;

                if( !node.GetChild(0).GetComponent<DoorController>() ){
                    Destroy( node.gameObject );
                    PushAgainOnScene( i );
                }else{
                    ResetChild( i, childIndex);
                }
            }

    }
    void PushAgainOnScene( int index){
        if( !lastSavedObjects.ContainsKey(index) ) return;
        DoorSaveInfo info = lastSavedObjects[index];

        var newInstancion = Instantiate( childPrefab, info.position, info.rotation, transform);
        newInstancion.transform.localScale = info.scale;

        newInstancion.transform.GetChild(0).position   = info.playerDetectorValues.position;
        newInstancion.transform.GetChild(0).rotation   = info.playerDetectorValues.rotation;
        newInstancion.transform.GetChild(0).localScale = info.playerDetectorValues.scale;

        newInstancion.transform.GetChild(0).GetComponent<DoorController>().numberOfRequiredKeys = info.requiredNumberOfKeys;
        newInstancion.GetComponent<ObjectIdHolder>().objectId = index;
    }

}
