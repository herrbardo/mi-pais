using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProvinceCard : MonoBehaviour
{
    [SerializeField] TMP_Text ProvinceNameText;
    [SerializeField] TMP_Text NaturalResourcesText;
    [SerializeField] TMP_Text ArrastrarAquiText;

    private void Awake()
    {
        ProvinceEvents.GetInstance().ProvinceSelected += ProvinceSelected;
    }
    
    private void Start()
    {
        ProvinceNameText.text = NaturalResourcesText.text = string.Empty;
        ArrastrarAquiText.enabled = false;
    }

    private void OnDestroy()
    {
        ProvinceEvents.GetInstance().ProvinceSelected -= ProvinceSelected;
    }

    void ProvinceSelected(ProvinceInfo info)
    {
        ProvinceNameText.text = info.DisplayName;
        ArrastrarAquiText.enabled = true;
        NaturalResourcesText.text = "Recursos: ";
        bool oneResourceAlreadyAdded = false;

        foreach (NaturalResources item in info.NaturalResources)
        {
            if(oneResourceAlreadyAdded)
                NaturalResourcesText.text += ", ";

            NaturalResourcesText.text += item.ToString();
            oneResourceAlreadyAdded = true;
        }
    }
}
