using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDebugInfo : MonoBehaviour
{

    public void Awake() {
        GlobalUtils.debugConsole  = m_debugText[0];
        GlobalUtils.debugConsole2 = m_debugText[2];
    }

    [SerializeField] public Text[] m_debugText;

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
        m_debugText[0].text = GetComponent<Player>().GetStackInfo();
        m_debugText[1].text = CatUtils.PlayerJumpForceMax.ToString() + " " + CatUtils.PlayerJumpForceMin.ToString();     
        m_debugText[2].text = transform.position + "\n";
        m_debugText[2].text += CommonValues.PlayerVelocity.ToString();
        m_debugText[3].text = "HP : " +  GetComponent<Player>().healthPoints.ToString();
        m_debugText[4].text = "Invincible : " + GetComponent<Player>().isImmortal() + "\n";
        m_debugText[4].text += "Keys : " + GetComponent<Player>().keys;
        
    }

}
