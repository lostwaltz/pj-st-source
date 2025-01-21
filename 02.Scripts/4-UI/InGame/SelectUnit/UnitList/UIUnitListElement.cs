using System;
using System.Collections;
using DG.Tweening;
using EnumTypes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIUnitListElement : UIAnimation
{
    [SerializeField] private Image imgAvatar;
    [SerializeField] private TMP_Text txtLevel;
    [SerializeField] private ScrollRect scrollRect;

    [SerializeField] private RectTransform uiElement;

    [SerializeField] private GameObject CubeContainer;
    [SerializeField] private GameObject Cube;
    public event Action<UnitInstance, Vector2> TryInstantiateUnit;

    private UnitInstance unitInstance;
    private bool isDragging = false;
    private UIDragHandler dragHandler;

    private bool isPointerDragOutLeft;
    private Vector2 curHitCellCoord;

    private LayerMask cellLayer;
    private Camera cam;

    private Tween tween;
    private float baseY;

    protected override void Start()
    {
        base.Start();

        cam = Camera.main;
        cellLayer = LayerMask.GetMask("Cell");
        dragHandler = new UIDragHandler(scrollRect.gameObject);

        BindEvent(gameObject, _ =>
        {
            Core.UIManager.GetUI<UIGachaInfoCanvas>().SetDataOpenUI(unitInstance);
        });
        
        BindEvent(gameObject, _ =>
        {
            Debug.Log($"{unitInstance.UnitBase.Name} OnActiveInfoDisplay");
            UIBattle.PlayUIBattleClickNormalSound();
        });
        BindEvent(gameObject, dragHandler.HandleDrag, UIEvent.Drag);
        BindEvent(gameObject, eventData =>
        {
            UIBattle.PlayUIBattleClickNormalSound();
            dragHandler.HandleDragBegin(eventData);
        }, UIEvent.BeginDrag);
        BindEvent(gameObject, dragHandler.HandleDragEnd, UIEvent.EndDrag);

        BindEvent(gameObject, _ => isDragging = true, UIEvent.BeginDrag);
        BindEvent(gameObject, _ => isDragging = false, UIEvent.EndDrag);

        BindEvent(gameObject, CheckPoint, UIEvent.Drag);

        BindEvent(gameObject, _ => OnUpPoint(), UIEvent.Up);

        BindEvent(gameObject, _ => StartCoroutine(OnExit()), UIEvent.Exit);
    }

    private IEnumerator OnExit()
    {
        yield return null;

        if (!dragHandler.StopHandleDrag)
            transform.DOScale(1f, 0.1f);
    }

    private void CheckPoint(PointerEventData eventData)
    {
        if (!isDragging) return;
        if (!CheckExitToLeft(eventData, uiElement)) return;

        dragHandler.StopHandleDrag = true;
        isPointerDragOutLeft = true;
    }

    private void Update()
    {
        if (!isPointerDragOutLeft) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, cellLayer)) return;

        Vector2 hitCoord = hit.transform.gameObject.gameObject.GetComponent<StageComponent>().placement.coord;

        StageSO currentStage = Core.DataManager.SelectedStage;
        var vaildPlacement = currentStage.playerPlacements.Find(p => p.coord == hitCoord && !p.isPlaced);

        if (vaildPlacement.Equals(default(PlayerPlacement))) return;

        curHitCellCoord = hitCoord;
        Vector3 hitPoint = new Vector3(hit.transform.position.x, Cube.transform.position.y, hit.transform.position.z);
        Cube.transform.position = hitPoint;

        if (Camera.main != null)
        {
            Vector3 direction = Camera.main.transform.forward;
            direction.y = 0;
            direction.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            Vector3 currentRotation = Cube.transform.rotation.eulerAngles;
            Vector3 newRotation = new Vector3(currentRotation.x, targetRotation.eulerAngles.y, currentRotation.z);

            Cube.transform.rotation = Quaternion.Euler(newRotation);
        }

        if (tween != null) return;
        
        Cube.SetActive(true);

        Cube.transform.position = hitPoint;
        Cube.transform.position = Vector3.up * 0.5f;

        tween = Cube.transform.DOMoveY(Cube.transform.position.y + 0.5f, 0.5f)
            .ChangeStartValue(new Vector3(hit.point.x, Cube.transform.position.y, hit.point.z))
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void OnUpPoint()
    {
        if (false == isPointerDragOutLeft) return;

        dragHandler.StopHandleDrag = false;
        isPointerDragOutLeft = false;

        Vector3 resultPosition = Cube.transform.position;
        resultPosition.x = (int)resultPosition.x - 1;
        resultPosition.y = (int)resultPosition.y - 1;
        resultPosition.z = (int)resultPosition.z;

        TryInstantiateUnit?.Invoke(unitInstance, curHitCellCoord);

        UIBattle.PlayUIBattleCharacterAssign();

        Cube.SetActive(false);
        tween.Kill();
        tween = null;
    }

    public void UpdateElement(UnitInstance unitInstance)
    {
        this.unitInstance = unitInstance;
        txtLevel.text = $"Lv. {unitInstance.Level}";
        imgAvatar.sprite = Resources.Load<Sprite>(unitInstance.UnitBase.BustPath);
    }

    public bool CheckExitToLeft(PointerEventData eventData, RectTransform uiElement)
    {
        var mousePosition = eventData.position;
        return mousePosition.x <= (uiElement.position.x - (uiElement.rect.width * 0.5f));
    }
}
