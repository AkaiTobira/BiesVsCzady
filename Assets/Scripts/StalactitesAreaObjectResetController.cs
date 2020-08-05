using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalactitesAreaObjectSaveInfo : GeneralObjectInfoPack{
    public List<int> indexes = new List<int>();

    public bool resetArea;
}


public class StalactitesAreaObjectResetController : MonoBehaviour
{

    int baseChildNumber = 0;

    [SerializeField] Transform statlactites = null;

    Dictionary<int, StalactitesAreaObjectSaveInfo> lastSavedObjects = new Dictionary<int, StalactitesAreaObjectSaveInfo>();

    void Start()
    {
        baseChildNumber = transform.childCount;
        for( int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).GetComponent<ObjectIdHolder>().objectId = i;
        }
        SaveAllObjects();
    }

    StalactitesAreaObjectSaveInfo fillObjectInfo( Transform go){
        StalactitesAreaObjectSaveInfo infoPack = new StalactitesAreaObjectSaveInfo();

        foreach( StalactitController sc in  go.GetComponent<RoarExtensionAreaController>().objectsToTakeDown){
            if( sc != null ) infoPack.indexes.Add( sc.GetComponent<ObjectIdHolder>().objectId );
        }

        infoPack.resetArea = go.GetComponent<RoarExtensionAreaController>().hasBeenActivated;

        return infoPack;
    }

    public void SaveAllObjects(){
        lastSavedObjects = new Dictionary<int, StalactitesAreaObjectSaveInfo>();
        for( int i = 0; i < transform.childCount; i++){
            Transform child = transform.GetChild(i);
            lastSavedObjects[child.GetComponent<ObjectIdHolder>().objectId] = fillObjectInfo( child );
        }
    }

    public void ResetChild( int i, int index)
    {
        StalactitesAreaObjectSaveInfo info = lastSavedObjects[index];

        StalactitController[] objToTakeDown = new StalactitController[info.indexes.Count ]; 
        int h = 0;
        for( int j = 0; j < statlactites.childCount; j++){
            var t = statlactites.GetChild(j).GetComponent<ObjectIdHolder>();
            if( info.indexes.Contains( t.objectId )){
//                Debug.Log( h.ToString() + "  " + j.ToString() );
                if( statlactites.GetChild(j) == null ) continue;
                objToTakeDown[h] = statlactites.GetChild(j).GetComponent<StalactitController>();
                h++;
            }
        }

        transform.GetChild( index ).GetComponent<RoarExtensionAreaController>().objectsToTakeDown = objToTakeDown;
        transform.GetChild( index ).GetComponent<RoarExtensionAreaController>().hasBeenActivated = info.resetArea;
        
        
    }


    public void LoadAllObjects(){
//        Debug.Log( "CHILDS EQUIE " + (transform.childCount == baseChildNumber).ToString());
        if( transform.childCount == baseChildNumber){
            for( int i = 0; i < transform.childCount; i++){
                int childIndex  = transform.GetChild(i).GetComponent<ObjectIdHolder>().objectId;
                ResetChild( i, childIndex);
            }
        }
    }

}
