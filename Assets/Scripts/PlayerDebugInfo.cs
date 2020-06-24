using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDebugInfo : MonoBehaviour
{

    public void Awake() {
        GlobalUtils.debugConsole = m_debugText;
    }

    [SerializeField] public Text m_debugText = null;

    public void LockBiesValue(){
        GetComponent<BiesBalance>().LockCurrentTemp();
    }

    public void LockCatValues(){
        GetComponent<CatBalance>().LockCurrentTemp();
    }

    public void SaveValues(){
        GetComponent<BiesBalance>().SaveBalance();
        GetComponent<CatBalance>().SaveBalance();
    }

    public void LoadValues(){
        GetComponent<BiesBalance>().LoadBalance("Temp/BiesValues");
        GetComponent<CatBalance>().LoadBalance("Temp/CatValues");
    }
}
