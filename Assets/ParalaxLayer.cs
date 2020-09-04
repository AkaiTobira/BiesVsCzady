using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxLayer : MonoBehaviour
{

    private Transform mainCamera;

    public float verticalSpeed = 0;

    public float horizontalSpeed = 0;

    private Vector2 transition = new Vector2();

    private Vector2 basePosition = new Vector2();

    private Vector2 playerPostiionPreviousFrame = new Vector2();

    void Start()
    {
        mainCamera = Camera.main.transform;
        basePosition = transform.position;
    }

    bool waitForPlayer = true;

    void Update()
    {
        if( waitForPlayer ){
            if( GlobalUtils.PlayerObject == null ){
            
            }else{
                waitForPlayer = false;
                playerPostiionPreviousFrame = GlobalUtils.PlayerObject.transform.position;
            }
            return;
        }

        var change =  playerPostiionPreviousFrame - (Vector2)GlobalUtils.PlayerObject.transform.position;
        playerPostiionPreviousFrame = GlobalUtils.PlayerObject.transform.position;

        transition.x += verticalSpeed   * change.x * Time.deltaTime;
        transition.y += horizontalSpeed * change.y * Time.deltaTime;
        transform.position = transition + basePosition;
    }
}
