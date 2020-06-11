using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebugInfo : MonoBehaviour
{

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
