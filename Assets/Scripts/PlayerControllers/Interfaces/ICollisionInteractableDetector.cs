using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ICollisionInteractableDetector {
    
    bool canClimbLedge();
    void enableFallForOneWayFloor();
    bool canFallByFloor();
    Transform GetClimbableObject();
    Transform GetPullableObject();
    bool IsWallPullable();
}