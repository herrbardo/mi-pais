using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class GameFinishedManager : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] Sprite GameOverImage;

    [Header("Components")]
    [SerializeField] TMP_Text ResultLabel;
    [SerializeField] TMP_Text ProvincesValue;
    [SerializeField] TMP_Text MoneyValue;
    [SerializeField] TMP_Text UnemploymentValue;
    [SerializeField] Image BackgroundComponent;

    private void Start()
    {
        int lostProvinces = ManagementResult.GetInstance().Provinces.Where(p => p.State != ProvinceState.Working).Count();
        int totalProvinces = ManagementResult.GetInstance().Provinces.Count();
        double totalMoney = ManagementResult.GetInstance().Provinces.Select(p => p.Money).Sum();
        double totalPopulation = ManagementResult.GetInstance().Provinces.Select(p => p.Population).Sum();
        double totalEmployees = ManagementResult.GetInstance().Provinces.Select(p => p.PeopleEmployeed).Sum();
        double unemploymentPercentage = (totalEmployees == 0) ? 100 : totalEmployees / totalPopulation * 100;

        if(totalProvinces == lostProvinces)
        {
            ResultLabel.text = "PÉSIMA";
            ResultLabel.color = Color.red;
            GlobalDJ.Instance.PlaySong(1, true);
            BackgroundComponent.sprite = GameOverImage;
        }
        else if(lostProvinces > 0)
        {
            ResultLabel.text = "REGULAR";
            ResultLabel.color = Color.yellow;
        }
        else if(lostProvinces == 0)
        {
            ResultLabel.text = "BUENA";
            ResultLabel.color = Color.green;
        }

        ProvincesValue.text = string.Format("{0}/{1}", (totalProvinces - lostProvinces), totalProvinces);
        MoneyValue.text = string.Format("${0:N0}", totalMoney);
        UnemploymentValue.text = string.Format("{0}%", unemploymentPercentage);
    }
}
