using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProvinceLostContainer : MonoBehaviour
{
    [SerializeField] List<ProvinceLostInfo> ProvincesInfo;
    [SerializeField] Image Logo;
    [SerializeField] TMP_Text Description;

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void ShowState(ProvinceState state)
    {
        if(state == ProvinceState.Working)
            Hide();
        else
        {
            ProvinceLostInfo selectedInfo = ProvincesInfo.Where(p => p.State == state).FirstOrDefault();
            Logo.sprite = selectedInfo.Logo;
            Description.text = selectedInfo.Description;
            this.gameObject.SetActive(true);
        }
    }
}
