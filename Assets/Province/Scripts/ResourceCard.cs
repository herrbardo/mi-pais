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
    [SerializeField] FollowingCard FollowingCardController;

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

    void MouseResourceAction(NaturalResource resource, bool enter, Vector3 offset, int resourceUse, int cooldown)
    {
        if(enter)
        {
            FollowingCardController.OffsetPosition = offset;
            Show();
            FillCardWithResourceData(resource, resourceUse, cooldown);
        }
        else
            Hide();
    }

    void FillCardWithResourceData(NaturalResource resource, int resourceUse, int cooldown)
    {
        int turnsLeft = resource.DurationInTurns - resourceUse;
        ResourceNameText.text = resource.Name;
        DurationInTurnsText.text = string.Format("{0}/{1} turnos", turnsLeft, resource.DurationInTurns);

        if(cooldown == 0)
            CooldownInTurnsText.text = resource.CooldownInTurns + " turnos";
        else
            CooldownInTurnsText.text = string.Format("{0}/{1} turnos", cooldown, resource.CooldownInTurns);
    }
}
