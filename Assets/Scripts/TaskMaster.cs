using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskMaster : MonoBehaviour
{

    public Vector2 lastCheckPoint = new Vector2();
    public Transform PlayerPrefab;

    void Start()
    {
        GlobalUtils.TaskMaster = GetComponent<TaskMaster>();
        lastCheckPoint = GlobalUtils.PlayerObject.transform.position;
    }


    public void SetPlayerAtLastCheckpoint(){
        GlobalUtils.PlayerObject.position = lastCheckPoint;
        GlobalUtils.PlayerObject.GetComponent<Player>().ResetPlayer();
        
        //var newPlayer = Instantiate( PlayerPrefab, (Vector3)lastCheckPoint, Quaternion.identity );
        //GlobalUtils.Camera.SetNewFollowable( newPlayer );
        //Destroy(GlobalUtils.PlayerObject.gameObject, 1.0f);
        //GlobalUtils.PlayerObject = newPlayer;
    }

    public void UpdateCheckpoint( Vector2 checkpointPosition){
        lastCheckPoint = checkpointPosition;
    }

}
