using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyBaseState : EnemyBaseState
{

    new protected FlyingAkaiController entityScript = null;

    public FlyingEnemyBaseState( GameObject controllable ) : base(controllable){
        m_controllabledObject = controllable;
        m_FloorDetector = controllable.transform.Find("Detector").
                            GetComponent<ICollisionFloorDetector>();
        m_animator      = controllable.transform.Find("Animator")
                            .GetComponent<Animator>();
        m_nextState = null;
        entityScript = controllable.GetComponent<FlyingAkaiController>();
    }

}
