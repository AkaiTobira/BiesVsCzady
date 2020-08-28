using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarController : MonoBehaviour
{

    [SerializeField] Image hpFill;

    public void UpdateHp( float currentHP, float maxHP ){
        hpFill.fillAmount = currentHP/maxHP ;
    }

}
