using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPieceSlot : UIBase
{
    [SerializeField] private Image UnitImg;
    [SerializeField] private TMP_Text countTxt;
    public int unitCount;
    public int unitKey;
    
    public void Init(UnitInstance unit, int amount, int key)
    {
        unitCount = amount;
        unit.UnitBase.Key = key;
        unitKey = unit.UnitBase.Key;
        UnitImg.sprite = Resources.Load<Sprite>(unit.UnitBase.GachaUnitPiece);
        countTxt.text = $"X{(unitCount)*30}";
    }
}
