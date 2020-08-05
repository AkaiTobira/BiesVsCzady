using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DestroyableObjectSaveInfo : GeneralObjectInfoPack{

    public float pullFriction;
    public float pushFriction;
    public float gravity;
    public float maxGravity;

    public float   durability;
}


public class DestroyableObjectController : MonoBehaviour
{

    int baseChildNumber = 0;

    [SerializeField] GameObject childPrefab = null;

    Dictionary<int, DestroyableObjectSaveInfo> lastSavedObjects = new Dictionary<int, DestroyableObjectSaveInfo>();

    void Start()
    {
        baseChildNumber = transform.childCount;
        for( int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).GetComponent<ObjectIdHolder>().objectId = i;
        }
        SaveAllObjects();
    }

    DestroyableObjectSaveInfo fillObjectInfo( Transform go){

        DestroyableObjectSaveInfo infoPack = new DestroyableObjectSaveInfo();
        infoPack.saveBasics( go );

        infoPack.pullFriction = go.GetComponent<CollisionDetectorMovable>().PullFriction;
        infoPack.pushFriction = go.GetComponent<CollisionDetectorMovable>().PushFriction;
        infoPack.gravity      = go.GetComponent<CollisionDetectorMovable>().GravityForce;
        infoPack.maxGravity   = go.GetComponent<CollisionDetectorMovable>().MaxGravityForce;
        infoPack.durability   = go.GetComponent<DestroyableObject>().durability;

        return infoPack;
    }

    public void SaveAllObjects(){
        lastSavedObjects = new Dictionary<int, DestroyableObjectSaveInfo>();
        for( int i = 0; i < transform.childCount; i++){
            Transform child = transform.GetChild(i);
            if( child.GetComponent<DestroyableObject>().durability <= 0 ) continue;
            lastSavedObjects[child.GetComponent<ObjectIdHolder>().objectId] = fillObjectInfo( child );
        }
    }

    public void ResetChild( int i, int index)
    {
        if( !lastSavedObjects.ContainsKey(index) ) return;

        DestroyableObjectSaveInfo info = lastSavedObjects[index];

        transform.GetChild(i).position = info.position;
        transform.GetChild(i).rotation = info.rotation;
        transform.GetChild(i).localScale = info.scale;

        transform.GetChild(i).GetComponent<CollisionDetectorMovable>().PushFriction = info.pushFriction;
        transform.GetChild(i).GetComponent<CollisionDetectorMovable>().PullFriction = info.pullFriction;
        transform.GetChild(i).GetComponent<CollisionDetectorMovable>().GravityForce = info.gravity;
        transform.GetChild(i).GetComponent<CollisionDetectorMovable>().MaxGravityForce = info.maxGravity;
        transform.GetChild(i).GetComponent<DestroyableObject>().durability = info.durability;

        transform.GetChild(i).GetComponent<Animator>().Rebind();
    }


    public void LoadAllObjects(){
//        Debug.Log( "CHILDS EQUIE " + (transform.childCount == baseChildNumber).ToString());
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
                Debug.Log( "Don't find " + i.ToString());
                PushAgainOnScene( i );
            }
        }
    }

    void PushAgainOnScene( int index){
        DestroyableObjectSaveInfo info = lastSavedObjects[index];

        var newInstancion = Instantiate( childPrefab, info.position, info.rotation, transform);
        newInstancion.transform.localScale = info.scale;

        newInstancion.transform.GetComponent<CollisionDetectorMovable>().PushFriction = info.pushFriction;
        newInstancion.transform.GetComponent<CollisionDetectorMovable>().PullFriction = info.pullFriction;
        newInstancion.transform.GetComponent<CollisionDetectorMovable>().GravityForce = info.gravity;
        newInstancion.transform.GetComponent<CollisionDetectorMovable>().MaxGravityForce = info.maxGravity;
        newInstancion.transform.GetComponent<DestroyableObject>().durability = info.durability;
    }

}
