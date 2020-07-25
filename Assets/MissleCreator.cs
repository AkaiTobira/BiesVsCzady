using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleCreator : MonoBehaviour
{

    [SerializeField] Transform misslePrefab = null;

    void OnEnable()
    {
        Vector3 direction = GlobalUtils.PlayerObject.position - transform.TransformPoint(Vector3.zero);
        direction.Normalize();

        var instance = Instantiate( misslePrefab, transform.TransformPoint(Vector3.zero), Quaternion.identity );
        instance.GetComponent<MissleController>().direction = direction;
        instance.GetComponent<MissleController>().SetRotationZ(Vector3.Angle(Vector3.left, direction));
    }

}
