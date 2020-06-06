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
    public class BiesValues{
    }

    [System.Serializable]
    public class CatValues{
        float minJumpHeight;
        float targetJumpHeight;
        float timeToJumpApex;
        float moveDistancef;
        float moveDistanceInAir;
        float maxMoveDistanceInAir;
        float wallFriction;
        float WallJumpFactorsX;
        float WallJumpFactorsY;
    }

}
