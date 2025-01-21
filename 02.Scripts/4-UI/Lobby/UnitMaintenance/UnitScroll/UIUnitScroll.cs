using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitScroll : UIBase
{
    [SerializeField] private Transform Content;
    [SerializeField] private ScrollRect ScrollRect;
    [SerializeField] private VerticalLayoutGroup VerticalLayoutGroup;

    private readonly List<UIUnitScrollSlot> uiUnitScrollSlots = new();
    [SerializeField] private UIUnitScrollSlot UnitSlotPrefab;

    private void Start()
    {
        Core.UnitManager.OnValueChanged += UpdateUI;

        VerticalLayoutGroup.CalculateLayoutInputVertical();
        VerticalLayoutGroup.SetLayoutVertical();

    }

    public void UpdateUI(List<UnitInstance> unitInfoList)
    {
        for (var i = 0; i < unitInfoList.Count; i++)
        {
            if (uiUnitScrollSlots.Count <= i)
            {
                var slot = Instantiate(UnitSlotPrefab, Content.transform);
                slot.gameObject.SetActive(true);
                uiUnitScrollSlots.Add(slot);
            }

            uiUnitScrollSlots[i].UpdateUI(unitInfoList[i]);
        }
    }
    public IEnumerable<UIUnitScrollSlot> GetUIUnitScrollSlots()
    {
        return uiUnitScrollSlots;
    }

    private void OnDestroy()
    {
        Core.UnitManager.OnValueChanged -= UpdateUI;
    }

    protected override void OpenProcedure()
    {
        base.OpenProcedure();
        
        UpdateUI(Core.UnitManager.Build());
        LayoutRebuilder.ForceRebuildLayoutImmediate(ScrollRect.transform as RectTransform);
        GetComponent<UISlotGroupSystem>().InitAllSlots();
        uiUnitScrollSlots.First().InitSlot();
        uiUnitScrollSlots.First().InstantiateFirstUnit();
    }
}
