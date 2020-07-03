using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AkaiController : IEntity
{
    [HideInInspector] public Vector2 velocity;

    public float moveBrakingTime = 0;
    public float moveAccelerationTime = 0;

    public float maxMoveSpeed    = 0;

    public float gravityForce    = 0;

    [Header("PatrolBehaviour")]
    public bool canPatrol       = false;

    public float patrolRangeLeft    = 0;
    public float patrolRangeRight    = 0;

    [Header("RandomMoveBehaviour")]
    public bool canRandomMove   = false;

    public float maxMoveDistance = 0;

    [Header("Debug")]

    [SerializeField] public Text DebugConsoleInfo1;
    [SerializeField] public Text DebugConsoleInfo2;

    void Start()
    {
        m_FloorDetector = transform.Find("Detector").GetComponent<ICollisionFloorDetector>();
        m_animator      = transform.Find("Animator").GetComponent<Animator>();
        m_controller    = new SFSMEnemy( gameObject, new CzadIdle( gameObject ) );
        m_FloorDetector.Move( new Vector2(0.1f, 0) );
    }


    void Update() {
        m_controller.Update();
        UpdateDebugConsole();
    }

    void UpdateDebugConsole(){
        DebugConsoleInfo2.text = m_controller.GetStackStatus();
        DebugConsoleInfo1.text = "";
        DebugConsoleInfo1.text += velocity.ToString() + "\n";
    }

    void OnDrawGizmos(){
        Debug.DrawLine( transform.position, transform.position + new Vector3( patrolRangeRight, 0,0), new Color(0,1,1));
        Debug.DrawLine( transform.position, transform.position + new Vector3( patrolRangeLeft , 0,0), new Color(0,1,1));
    }

}
