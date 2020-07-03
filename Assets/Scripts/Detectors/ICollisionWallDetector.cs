﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ICollisionWallDetector {
    
    void DetectWall( );
    float GetDistanceToClosestWallFront();
    float GetDistanceToClosestWallBack();
    bool isWallClose();
    bool isCollideWithLeftWall();
    bool isCollideWithRightWall();
}