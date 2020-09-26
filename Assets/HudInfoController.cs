using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudInfoController : MonoBehaviour
{

    [SerializeField] private Animator _formChangeAnimator;

    private bool _isBies = false;
    private Player _player;

    void Start() {
        _player = GlobalUtils.PlayerObject.GetComponent<Player>();
    }


    void Update() {
        UpdateFormIcon();
    }

    void UpdateFormIcon(){
        if( _isBies ){
            if(  _player.GetCurrentFormName().Contains("Cat")){
                _isBies = false;
                _formChangeAnimator.SetBool("IsCat", true);
            }
            _formChangeAnimator.SetBool("IsBies", false);
        }else{
            if(  _player.GetCurrentFormName().Contains("Bies")){
                _isBies = true;
                _formChangeAnimator.SetBool("IsBies", true);
            }
            _formChangeAnimator.SetBool("IsCat", false);
        }
    }



}
