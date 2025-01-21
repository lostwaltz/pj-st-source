using EnumTypes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIStageNode : UIBase
{
    [SerializeField] private GameObject ImgOutLine;
    [SerializeField] private GameObject ImgClearIcon;

    [SerializeField] private TMP_Text TextNodeName;
    [SerializeField] private TMP_Text TextStageNumber;

    private StageSO stageSo;

    public void Init(StageSO stageArgs)
    {
        stageSo = stageArgs;
        TextNodeName.text = stageSo.name.ToString();
        TextStageNumber.text = $"{stageSo.stageData.DependencyKey} - {stageSo.stageData.StageKey}";
        
        ImgClearIcon.SetActive(Core.DataManager.StageClearData[stageArgs.stageData.StageKey]);
    }
    private void Awake()
    {
        BindEvent(gameObject, OnClicked);
        BindEvent(gameObject, (eventData) => ImgOutLine.gameObject.SetActive(true), UIEvent.Enter);
        BindEvent(gameObject, (eventData) => ImgOutLine.gameObject.SetActive(false), UIEvent.Exit);
    }

    private void OnClicked(PointerEventData eventData)
    {
        UISound.PlayStageNodeClick();
        UIStageInfo stageInfo = Core.UIManager.GetUI<UIStageInfo>();
        if (stageInfo != null)
        {

            stageInfo.ShowStageInfo(stageSo);
        }
        
        Core.DataManager.SelectedStage = stageSo;
    }
}