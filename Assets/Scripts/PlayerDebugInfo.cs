using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDebugInfo : MonoBehaviour
{

    public void Awake() {
        GlobalUtils.debugConsole  = m_debugText;
        GlobalUtils.debugConsole2 = m_debugText3;
    }

    [SerializeField] public Text m_debugText = null;
    [SerializeField] public Text m_debugText2 = null;
    [SerializeField] public Text m_debugText3 = null;

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

    void Update()
    {
        m_debugText2.text = CatUtils.PlayerJumpForceMax.ToString() + " " + CatUtils.PlayerJumpForceMin.ToString();
        
        m_debugText3.text = transform.position + "\n";
        m_debugText3.text += CommonValues.PlayerVelocity.ToString();
    }

}
