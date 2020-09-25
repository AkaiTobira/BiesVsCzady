using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarController : MonoBehaviour
{

    [SerializeField] Image hpFill;
    [SerializeField] private bool swichColorActive = false;
    [SerializeField] private Color[] colorChange;

    private Color acitveColor;

    void Start() {
        acitveColor = colorChange[colorChange.Length-1];
        hpFill.color = acitveColor;
    }

    public void UpdateHp( float currentHP, float maxHP ){
        hpFill.fillAmount = currentHP/maxHP ;
        UpdateColor();
    }

    private void UpdateColor(){
        if( !swichColorActive ) return;

        float stepSize    = ( colorChange.Length < 2) ? 1.0f :  1.0f/(colorChange.Length-1);
        float currentStep = hpFill.fillAmount;

        int index = 0;
        for( int i = 0; i < colorChange.Length; i++){
            if( currentStep > stepSize * (i+1)){

            }else{
                index= i;
                break;
            }
        }

        float curr = 0.0f;
        for( curr = currentStep; curr > stepSize; curr -= stepSize ){}
        float t = curr/stepSize;

     //   Debug.LogWarning( acitveColor +" Index : "+ /*colorChange[index] + " " + colorChange[index + 1] +" "+*/ index  + " \n::  " 
     //                      + " t :" + t + "  amount to max step :" + curr + " max Step " + stepSize + " \n:: whole Value " + currentStep );


        if( index == colorChange.Length-1){
            hpFill.color = colorChange[colorChange.Length-1];
            return;
        }

        acitveColor = (1.0f-t) * colorChange[index] + t * colorChange[index+1];
        hpFill.color = acitveColor;


    }

}
