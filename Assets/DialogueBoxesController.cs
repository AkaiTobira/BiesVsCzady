using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxesController : MonoBehaviour
{

    public Text catText;
    public Text biesText;

    public Animator catAnimator;
    public Animator biesAnimator;

    private Queue<GlobalUtils.DialogueInfo> sequence = new Queue<GlobalUtils.DialogueInfo>();

    public void AddToSequence( GlobalUtils.DialogueInfo dialog ){
        sequence.Enqueue(dialog);
    }

    [SerializeField] private float timeToNextBox = 3;
    private float timerToNextBox = 0;

    void Start()
    {
        GUIElements.DialogueSystem = GetComponent<DialogueBoxesController>();
    }

    void Update()
    {
        timerToNextBox = Mathf.Max( timerToNextBox - Time.deltaTime, 0);
        if(sequence.Count == 0)  return;
        if(timerToNextBox != 0 ) return;
        ShowNextDialogueBox();
    }

    void ShowNextDialogueBox(){

        GlobalUtils.DialogueInfo nextBox = sequence.Dequeue();
        switch( nextBox.type ){
            case GlobalUtils.Types.Both :
                ActivateBiesBox(nextBox.text);
                ActivateCatBox(nextBox.text);
                break;
            case GlobalUtils.Types.Bies :
                ActivateBiesBox(nextBox.text);
                break;
            case GlobalUtils.Types.Cat  :
                ActivateCatBox(nextBox.text);
                break;
            default: break;
        }
        timerToNextBox = timeToNextBox;
    }

    private void ActivateBiesBox( string text){
        biesAnimator.SetTrigger("Activate");
        biesText.text = text;
    }

    private void ActivateCatBox(string text){
        catText.text  = text;
        catAnimator.SetTrigger("Activate");
    }
}
