using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskMaster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GlobalUtils.TaskMaster = transform;
    }


}
