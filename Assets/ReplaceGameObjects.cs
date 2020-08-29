using UnityEngine;
using UnityEditor;
using System.Collections;
// CopyComponents - by Michael L. Croswell for Colorado Game Coders, LLC
// March 2010

public class ReplaceGameObjects : ScriptableWizard
{
public bool copyTransform = true;
public GameObject useGameObject;
public GameObject replaceChildOf;

[MenuItem ("Custom/Replace GameObjects")]


static void CreateWizard ()
{
ScriptableWizard.DisplayWizard("Replace GameObjects", typeof(ReplaceGameObjects), "Replace");
}

void OnWizardCreate ()
{

    int numberOfChild = replaceChildOf.transform.childCount;

    GameObject newChildHolder = new GameObject();
    newChildHolder.transform.parent = replaceChildOf.transform;
    newChildHolder.name = "Replaced";

    for( int i = 0; i < numberOfChild; i += 1){
        Transform t = replaceChildOf.transform.GetChild( i );
        GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab( useGameObject, newChildHolder.transform);
        newObject.name += " (" + i + ")"; 
        if( copyTransform ){
            newObject.transform.position   = t.position;
            newObject.transform.rotation   = t.rotation;
            newObject.transform.localScale = t.localScale;
        }
    }

    Debug.Log( "All children has been saved to 'Replaced' in " + replaceChildOf.name );
}

}