using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudInfoController : MonoBehaviour
{

    [Header("CurrentForm")]
    [SerializeField] private Animator _formChangeAnimator;

    [Header("TransformIcon")]
    [SerializeField] private Image  _transformIcon;
    [SerializeField] private Color _baseColorTransform;
    [SerializeField] private Color _lockedColorTransform;
 
    [Header("RoarIcon")]
    [SerializeField] private Image  _roarIcon;

    [Header("KeyIcon")]

    [SerializeField] private Text _amountOfKeys;

    private bool _isBies = false;
    private Player _player;

    void Start() {
        GUIElements.GUIinfo = gameObject;
        _player = GlobalUtils.PlayerObject.GetComponent<Player>();
    }


    void Update() {
        UpdateFormIcon();
        UpdateTransformIcon();
        UpdateRoarIcon();
        UpdateNumberOfKeys();
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

    void UpdateTransformIcon(){
        if( LockAreaOverseer.isChangeLocked ){
            _transformIcon.color = _lockedColorTransform;
        }else{
            _transformIcon.color = _baseColorTransform;
        }
    }

    void UpdateRoarIcon(){
        _roarIcon.fillAmount = (PlayerRoarHelper.ROAR_COLDOWN - PlayerRoarHelper.ColdownTImer) / PlayerRoarHelper.ROAR_COLDOWN;
    }

    void UpdateNumberOfKeys(){
        _amountOfKeys.text = _player.keys.ToString();
    }

}
