using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalactitesObjectSaveInfo : GeneralObjectInfoPack{

    public float damage;

    public float enemyDamage;
    public float gravity;


}


public class StalactitesObjectResetController : MonoBehaviour
{

    int baseChildNumber = 0;

    [SerializeField] GameObject childPrefab = null;

    Dictionary<int, StalactitesObjectSaveInfo> lastSavedObjects = new Dictionary<int, StalactitesObjectSaveInfo>();

    void Start()
    {
        baseChildNumber = transform.childCount;
        for( int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).GetComponent<ObjectIdHolder>().objectId = i;
        }
        SaveAllObjects();
    }

    StalactitesObjectSaveInfo fillObjectInfo( Transform go){

        StalactitesObjectSaveInfo infoPack = new StalactitesObjectSaveInfo();
        infoPack.saveBasics( go );

        infoPack.damage      = go.GetComponent<StalactitController>().damage;
        infoPack.gravity     = go.GetComponent<StalactitController>().GravityForce;
        infoPack.enemyDamage = go.GetComponent<StalactitController>().enemyDamage;

        return infoPack;
    }

    public void SaveAllObjects(){
        lastSavedObjects = new Dictionary<int, StalactitesObjectSaveInfo>();
        for( int i = 0; i < transform.childCount; i++){
            Transform child = transform.GetChild(i);
            lastSavedObjects[child.GetComponent<ObjectIdHolder>().objectId] = fillObjectInfo( child );
        }
    }

    public void ResetChild( int i, int index)
    {

        if( transform.GetChild(i).GetComponent<StalactitController>().HasBeenTouched() ){
            transform.GetChild(i).GetComponent<ObjectIdHolder>().objectId = -1;
            Destroy( transform.GetChild(i).gameObject);
            PushAgainOnScene( index );

        }else{
            StalactitesObjectSaveInfo info = lastSavedObjects[index];
            transform.GetChild(i).position = info.position;
            transform.GetChild(i).rotation = info.rotation;
            transform.GetChild(i).localScale = info.scale;

            transform.GetChild(i).GetComponent<StalactitController>().damage       = info.damage;
            transform.GetChild(i).GetComponent<StalactitController>().GravityForce = info.gravity;

        }
    }


    public void LoadAllObjects(){
        if( transform.childCount == baseChildNumber){
            for( int i = 0; i < transform.childCount; i++){
                int childIndex  = transform.GetChild(i).GetComponent<ObjectIdHolder>().objectId;
                ResetChild( i, childIndex);
            }
        }else{
            List<int> reseted = new List<int>();
            for( int i = 0; i < transform.childCount; i++){
                int childIndex  = transform.GetChild(i).GetComponent<ObjectIdHolder>().objectId;
                ResetChild( i, childIndex);
                reseted.Add( childIndex );
            }

            var savedIds = lastSavedObjects.Keys;

            foreach( int i in savedIds ){
                if( reseted.Contains( i )) continue;
                PushAgainOnScene( i );
            }
        }
    }
    void PushAgainOnScene( int index){
        StalactitesObjectSaveInfo info = lastSavedObjects[index];

        var newInstancion = Instantiate( childPrefab, info.position, info.rotation, transform);
        newInstancion.transform.localScale = info.scale;

        newInstancion.GetComponent<ObjectIdHolder>().objectId = index;
    
        newInstancion.GetComponent<StalactitController>().damage       = info.damage;
        newInstancion.GetComponent<StalactitController>().GravityForce = info.gravity;
        newInstancion.GetComponent<StalactitController>().enemyDamage  = info.enemyDamage;
    }
}
