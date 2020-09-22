using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySaveInfo : GeneralObjectInfoPack{

    public Animator animator;

}


public class KeysResetController : MonoBehaviour
{

    int baseChildNumber = 0;

    [SerializeField] GameObject childPrefab = null;

    Dictionary<int, KeySaveInfo> lastSavedObjects = new Dictionary<int, KeySaveInfo>();

    void Start()
    {
        baseChildNumber = transform.childCount;
        for( int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).GetComponent<ObjectIdHolder>().objectId = i;
        }
        SaveAllObjects();
    }

    KeySaveInfo fillObjectInfo( Transform go){

        KeySaveInfo infoPack = new KeySaveInfo();
        infoPack.saveBasics( go );

        infoPack.animator = go.GetComponent<KeyDetector>().TextNode;

        return infoPack;
    }

    public void SaveAllObjects(){
        lastSavedObjects = new Dictionary<int, KeySaveInfo>();
        for( int i = 0; i < transform.childCount; i++){
            Transform child = transform.GetChild(i);
            lastSavedObjects[child.GetComponent<ObjectIdHolder>().objectId] = fillObjectInfo( child );
        }
    }

    public void ResetChild( int i, int index)
    {
        KeySaveInfo info = lastSavedObjects[index];
        transform.GetChild(i).position = info.position;
        transform.GetChild(i).rotation = info.rotation;
        transform.GetChild(i).localScale = info.scale;

        if( info.animator ){
            transform.GetChild(i).GetComponent<KeyDetector>().TextNode = info.animator;
            transform.GetChild(i).GetComponent<KeyDetector>().TextNode?.Rebind();
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
        KeySaveInfo info = lastSavedObjects[index];

        var newInstancion = Instantiate( childPrefab, info.position, info.rotation, transform);
        newInstancion.transform.localScale = info.scale;

        if( info.animator ){
            newInstancion.GetComponent<KeyDetector>().TextNode = info.animator;
            newInstancion.GetComponent<KeyDetector>().TextNode.Rebind();
        }

        newInstancion.GetComponent<ObjectIdHolder>().objectId = index;
    }

}
