using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICombatResultSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text instanceName;
    [SerializeField] private TMP_Text instanceLevel;
    [SerializeField] private Image expBar;
    [SerializeField] private Image imgAvatar;


    public void UpdateSlot(UnitInstance unitInstance)
    {
        instanceName.text = unitInstance.UnitBase.Name;
        instanceLevel.text = $"Lv. {unitInstance.Level}";
        expBar.fillAmount = unitInstance.GetExpPercentage();

        imgAvatar.sprite = Resources.Load<Sprite>(unitInstance.UnitBase.Path);
    }
}
