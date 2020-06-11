using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JsonLoader : MonoBehaviour
{
    private static JsonLoader _instance;

    public TextAsset GameTips;

    public Tips allGameTips;

    public static JsonLoader Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<JsonLoader>();

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        LoadJsonFile();
    }

    public void LoadJsonFile()
    {
        allGameTips = JsonUtility.FromJson<Tips>(GameTips.ToString());
    }

    [System.Serializable]
    public class Tips
    {
        public string name;
        public string name1;
        public string name2;
    }

    [System.Serializable]
    public class CatValues{
        public float minJumpHeight;
        public float targetJumpHeight;
        public float timeToJumpApex;
        public float moveDistance;
        public float maxWallClimbSpeed;
        public float wallSlideFriction;
        public float wallClimbSpeed;
        public float moveDistanceInAir;
        public float maxMoveDistanceInAir;
        public float wallFriction;
        public float WallJumpFactorsX;
        public float WallJumpFactorsY;
    }

    [System.Serializable]
    public class BiesValues{
        public float minJumpHeight;
        public float targetJumpHeight;
        public float timeToJumpApex;
        public float moveDistance;
        public float moveDistanceInAir;
        public float maxMoveDistanceInAir;
        public float wallFriction;
        public float WallJumpFactorsX;
        public float WallJumpFactorsY;
        public float Attack1Damage;
        public float Attack2Damage;
        public float Attack3Damage;
        public float KnockBackValueAttack1X;
        public float KnockBackValueAttack1Y;
        public float KnockBackValueAttack2X;
        public float KnockBackValueAttack2Y;
        public float KnockBackValueAttack3X; 
        public float KnockBackValueAttack3Y;
    }
}
