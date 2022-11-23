using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnSwitchManager : MonoBehaviour
{
    [SerializeField] string PrefsVariableName;
    [SerializeField] Sprite EnableSprite;
    [SerializeField] Sprite DisabledSprite;

    bool enabledSwitch;

    void Start()
    {
        int value = PlayerPrefs.GetInt(PrefsVariableName, 1);
        enabledSwitch = value == 1;
        SetImage();
    }

    void SetImage()
    {
        Image imageComponent = GetComponent<Image>();
        imageComponent.sprite = (enabledSwitch) ? EnableSprite : DisabledSprite;
    }

    public void Click()
    {
        if(enabledSwitch)
            PlayerPrefs.SetInt(PrefsVariableName, 0);
        else
            PlayerPrefs.SetInt(PrefsVariableName, 1);
        
        enabledSwitch = !enabledSwitch;
        SetImage();
        GlobalDJ.Instance.SwitchChannelVolume(PrefsVariableName, enabledSwitch);
    }
}
