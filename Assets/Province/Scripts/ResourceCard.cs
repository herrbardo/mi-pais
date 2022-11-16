using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceCard : MonoBehaviour
{
    [Header("Resource Components")]
    [SerializeField] TMP_Text ResourceNameText;
    [SerializeField] TMP_Text DurationInTurnsText;
    [SerializeField] TMP_Text CooldownInTurnsText;

    private void Awake()
    {
        Hide();
        UIEvents.GetInstance().MouseResourceAction += MouseResourceAction;
    }

    private void OnDestroy()
    {
        UIEvents.GetInstance().MouseResourceAction -= MouseResourceAction;
    }

    void Hide()
    {
        this.gameObject.SetActive(false);
    }

    void Show()
    {
        this.gameObject.SetActive(true);
    }

    void MouseResourceAction(NaturalResource resource, bool enter)
    {
        if(enter)
        {
            Show();
            FillCardWithResourceData(resource);
        }
        else
            Hide();
    }

    void FillCardWithResourceData(NaturalResource resource)
    {
        ResourceNameText.text = resource.Name;
        DurationInTurnsText.text = resource.DurationInTurns + " turnos";
        CooldownInTurnsText.text = resource.CooldownInTurns + " turnos";
    }
}
