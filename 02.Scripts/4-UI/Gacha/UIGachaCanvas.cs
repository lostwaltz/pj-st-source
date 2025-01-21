using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIGachaCanvas : UIBase
{
    [SerializeField] private Image commonBGLineImg;
    [SerializeField] private Image legendaryBGImg;
    [SerializeField] private Image epicBGImg;
    [SerializeField] private Image rareBGImg;

    [SerializeField] private Image unitImage;

    [SerializeField] private Image characteristicIcon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text characteristicText;
   
    public void Init(UnitInstance unit)
    {
        //임의 테스트 코드

        if (unit.UnitBase.Grade == ExternalEnums.Grade.Legendary)
        {
            legendaryBGImg.gameObject.SetActive(true);
            epicBGImg.gameObject.SetActive(false);
            rareBGImg.gameObject.SetActive(false);
            commonBGLineImg.gameObject.SetActive(false);
        }
        else if (unit.UnitBase.Grade == ExternalEnums.Grade.Epic)
        {
            legendaryBGImg.gameObject.SetActive(false);
            epicBGImg.gameObject.SetActive(true);
            rareBGImg.gameObject.SetActive(false);
            commonBGLineImg.gameObject.SetActive(false);
        }
        else if (unit.UnitBase.Grade == ExternalEnums.Grade.Rare)
        {
            legendaryBGImg.gameObject.SetActive(false);
            epicBGImg.gameObject.SetActive(false);
            rareBGImg.gameObject.SetActive(true);
            commonBGLineImg.gameObject.SetActive(false);
        }
        else
        {
            legendaryBGImg.gameObject.SetActive(false);
            epicBGImg.gameObject.SetActive(false);
            rareBGImg.gameObject.SetActive(false);
            commonBGLineImg.gameObject.SetActive(true);
        }


        unitImage.sprite = Resources.Load<Sprite>(unit.UnitBase.WholePath);
        characteristicIcon.sprite = Resources.Load<Sprite>(unit.UnitBase.SpecialityPath);
        nameText.text = unit.UnitBase.Name;
        characteristicText.text = string.Join(",", unit.UnitBase.GachaSpecialityInfo);

    }
}
