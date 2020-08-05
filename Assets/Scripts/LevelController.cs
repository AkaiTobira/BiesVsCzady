using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    [SerializeField] MovableObjectController m_movable = null;
    [SerializeField] DestroyableObjectController m_destroyable = null;

    [SerializeField] StalactitesObjectResetController m_stalactits = null;

    [SerializeField] StalactitesAreaObjectResetController m_areaStalactits = null;

    [SerializeField] DoorsResetController m_doors = null;

    [SerializeField] KeysResetController m_keys = null;

    public void SaveLevelStatus(){
        m_movable.SaveAllObjects();
        m_destroyable.SaveAllObjects();
        m_stalactits.SaveAllObjects();
        m_areaStalactits.SaveAllObjects();
        m_keys.SaveAllObjects();
        m_doors.SaveAllObjects();
    }

    public void LoadLevelStatus(){
        m_movable.LoadAllObjects();
        m_destroyable.LoadAllObjects();
        m_stalactits.LoadAllObjects();
        m_areaStalactits.LoadAllObjects();
        m_doors.LoadAllObjects();
        m_keys.LoadAllObjects();
    }


}
