using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonTransition : MonoBehaviour
{
    [SerializeField] TMP_Text ButtonText;
    [SerializeField] string DisplayText;
    [SerializeField] string SceneToGo;

    private void Start()
    {
        ButtonText.text = DisplayText;
    }

    public void ButtonClick()
    {
        TransitionEvents.GetInstance().OnTransitionToScene(SceneToGo);
    }
    private void OnDrawGizmos()
    {
        if(ButtonText != null && DisplayText != null)
            ButtonText.text = DisplayText;
    }
}
