using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EnumTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIUnitScrollSlot : UIAnimation, ISlottable
{
    private UnitInstance unitInstance;
    [SerializeField] private GameObject Icon;

    [SerializeField] private Image outLineDefault;
    [SerializeField] private Image outLineOnEnter;
    [SerializeField] private Image outLineOnSelect;

    [SerializeField] private UISlotGroupSystem groupSystem;
    [SerializeField] private GameObject scrollRect;
    private UIDragHandler dragHandler;

    // 보유유닛 모델 프리팹
    private static readonly Dictionary<int, GameObject> unitPrefabs = new();
    private static readonly Dictionary<int, GameObject> instantiatedUnits = new();
    private static GameObject currentActiveUnit;


    protected override void Awake()
    {
        base.Awake();
        BindEvent(gameObject, OnClick);

        LoadUnitPrefabs();

        groupSystem.RegisterSlot(this);

        dragHandler = new UIDragHandler(scrollRect);

        BindEvent(gameObject, dragHandler.HandleDragBegin, UIEvent.BeginDrag);
        BindEvent(gameObject, dragHandler.HandleDrag, UIEvent.Drag);
        BindEvent(gameObject, dragHandler.HandleDragEnd, UIEvent.EndDrag);
    }

    private void LoadUnitPrefabs()
    {
        if (unitPrefabs.Count > 0) return;

        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs/Units/TrainingRoom");
        foreach (var prefab in prefabs)
        {
            string[] splitName = prefab.name.Split('_');
            if (splitName.Length == 2 && int.TryParse(splitName[1], out int key))
            {
                unitPrefabs[key] = prefab;
            }
        }
    }

    public void InitSlot()
    {
        Core.EventManager.Publish(new InteractionUIUnitInstance(unitInstance));

        FadeTo fadeTo = new FadeTo(0f, 1f, Ease.Linear, true, false, false);
        fadeTo.BehaviorExecute(outLineOnSelect.gameObject.transform, 0f);

        ScaleTo scaleTo = new ScaleTo(transform.localScale, Vector3.one * 1.25f, Ease.Linear, false, false, true, false);
        scaleTo.BehaviorExecute(gameObject.transform, 0f);
        IgnoreExitEvent = true;
        IgnoreEnterEvent = true;
    }
    public void Initialize()
    {
        transform.localScale = Vector3.one;
        Color color = outLineOnSelect.gameObject.GetComponent<Image>().color;
        color.a = 0f;
        outLineOnSelect.gameObject.GetComponent<Image>().color = color;
    }

    public void OnClick(PointerEventData eventData)
    {
        if (IgnoreExitEvent || IgnoreEnterEvent) return;

        UIBattle.PlayUIBattleClickNormalSound();

        int unitKey = unitInstance.UnitBase.Key;
        if (currentActiveUnit != null)
        {
            currentActiveUnit.SetActive(false);
        }

        GameObject unitObj;

        if (instantiatedUnits.TryGetValue(unitKey, out GameObject cachedUnit))
        {
            unitObj = cachedUnit;
            unitObj.SetActive(true);
            currentActiveUnit = unitObj;
        }
        else
        {
            if (unitPrefabs.TryGetValue(unitKey, out GameObject prefab))
            {
                unitObj = Instantiate(prefab, new Vector3(-0.5f, 0, 0), Quaternion.Euler(0, 180, 0));
                Unit unit = unitObj.GetComponent<Unit>();
                if (unit != null)
                {
                    unit.data = unitInstance;
                }
                instantiatedUnits[unitKey] = unitObj;

                currentActiveUnit = unitObj;
            }
        }

        Core.EventManager.Publish(new InteractionUIUnitInstance(unitInstance));

        groupSystem.DeselectAll();

        FadeTo fadeTo = new FadeTo(0f, 1f, Ease.Linear, true, false, false);
        fadeTo.BehaviorExecute(outLineOnSelect.gameObject.transform, 0.1f);

        ScaleTo scaleTo = new ScaleTo(transform.localScale, Vector3.one * 1.25f, Ease.Linear, false, false, true, false);
        scaleTo.BehaviorExecute(gameObject.transform, 0.1f);

        IgnoreExitEvent = true;
        IgnoreEnterEvent = true;
    }

    public void InstantiateFirstUnit()
    {
        int unitKey = unitInstance.UnitBase.Key;

        if (unitPrefabs.TryGetValue(unitKey, out GameObject prefab))
        {
            GameObject unitObj = Instantiate(prefab, new Vector3(-0.5f, 0, 0), Quaternion.Euler(0, 180, 0));
            Unit unit = unitObj.GetComponent<Unit>();
            if (unit != null)
            {
                unit.data = unitInstance;
            }
            instantiatedUnits[unitKey] = unitObj;
            currentActiveUnit = unitObj;
        }
    }
    public void OnDestroyTrainingRoomUnit()
    {
        foreach (var Unit in instantiatedUnits.Values)
        {
            if (Unit != null)
            {
                Destroy(Unit);
            }
        }
        instantiatedUnits.Clear();
    }


    public void UpdateUI(UnitInstance unitInstance)
    {
        this.unitInstance = unitInstance;
        Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>(unitInstance.UnitBase.Path);
    }

    public void SetHighlight(bool highlight)
    {
        if (false != highlight) return;

        IgnoreExitEvent = false;
        IgnoreEnterEvent = false;

        ScaleTo scaleTo = new ScaleTo(transform.localScale, Vector3.one, Ease.Linear, false, false, true, false);
        scaleTo.BehaviorExecute(gameObject.transform, 0.1f);

        FadeTo fadeTo = new FadeTo(1f, 0f, Ease.Linear, true, false, false);
        fadeTo.BehaviorExecute(outLineOnSelect.gameObject.transform, 0.1f);
    }

    private void OnDisable()
    {
        IgnoreExitEvent = false;
        IgnoreEnterEvent = false;
    }
}
