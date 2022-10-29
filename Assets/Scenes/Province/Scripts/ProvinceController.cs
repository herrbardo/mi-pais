using UnityEngine;

public class ProvinceController : MonoBehaviour
{
    [SerializeField] SpriteRenderer SelectedTickSprite;
    [SerializeField] ProvinceInfo Info;

    bool isSelected;

    private void Awake()
    {
        ProvinceEvents.GetInstance().ProvinceSelected += ProvinceSelected;
    }

    private void OnDestroy()
    {
        ProvinceEvents.GetInstance().ProvinceSelected -= ProvinceSelected;
    }

    void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
            if(!isSelected)
                SwitchSelection(true);
    }

    void SwitchSelection(bool select)
    {
        isSelected = select;
        SelectedTickSprite.gameObject.SetActive(select);

        if(select)
            ProvinceEvents.GetInstance().OnProvinceSelected(Info);
    }

    void ProvinceSelected(ProvinceInfo info)
    {
        if(!info.DisplayName.Equals(Info.DisplayName) && isSelected)
            SwitchSelection(false);
    }
}
