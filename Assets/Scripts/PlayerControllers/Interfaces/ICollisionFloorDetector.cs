using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ICollisionFloorDetector {
    
    Vector2 GetSlopeAngle();
    Vector2 GetTransition();
    void Move( Vector2 velocity);

    void Move( Vector2 velocity, bool updateFaceDir);
    void Move( float x, float y);
    GlobalUtils.Direction GetCurrentDirection();
    void CheatMove( Vector2 velocity);
    bool isOnCelling();
    bool isOnGround();
    void setLock( bool foceStatus_yeyIamJedi);
    BoxCollider2D GetComponent<BoxCollider2D>();

}