using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskMaster : MonoBehaviour
{

    

    public Vector2 lastCheckPoint = new Vector2();

    private int savedNumberOfKeys = 0;

    public LevelController levelControl;

    public int triggeredEnemies = 0;

    [SerializeField] private CheckpointController _activeCheckpoint;

    public bool IsPlayerInCombat() { return triggeredEnemies != 0 ; }

    public void EnemyTriggered(){
    //    triggeredEnemies += 1;
    }

    public void EnemyIsOutOfCombat(){
        triggeredEnemies = Mathf.Max(0, triggeredEnemies - 1);
    }

    void Start()
    {
        GlobalUtils.TaskMaster = GetComponent<TaskMaster>();
        if( GlobalUtils.PlayerObject ) lastCheckPoint    = GlobalUtils.PlayerObject.transform.position;
        if( GlobalUtils.PlayerObject ) savedNumberOfKeys = GlobalUtils.PlayerObject.GetComponent<Player>().keys;
    }

    public void SetPlayerAtLastCheckpoint(){
        GlobalUtils.PlayerObject.position = lastCheckPoint;
        GlobalUtils.PlayerObject.GetComponent<Player>().ResetPlayer();
        GlobalUtils.PlayerObject.GetComponent<Player>().keys = savedNumberOfKeys;

        levelControl.LoadLevelStatus();
    }


    private void UpdateInstance( CheckpointController activeChecpoint ){
        if( _activeCheckpoint == activeChecpoint) return;
        lastCheckPoint = activeChecpoint.transform.position;
        _activeCheckpoint.ResetCheckpoint();
        _activeCheckpoint = activeChecpoint;
        _activeCheckpoint.SetAcitve();
    }

    public void UpdateCheckpoint( CheckpointController activeChecpoint){
        UpdateInstance( activeChecpoint );
        savedNumberOfKeys = GlobalUtils.PlayerObject.GetComponent<Player>().keys;
        levelControl.SaveLevelStatus();
    }

}
