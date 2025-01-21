using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISkill : UIBase, IPointerEnterHandler, IPointerExitHandler
{
    public Button SkillButton { get; private set; }

    [SerializeField] private UISkillInfoPanel uiSkillInfoPanel;

    [SerializeField] private UISelectionPanel uiSelectionPanel;
    [SerializeField] private GameObject uiSelectionPanelObj;

    [SerializeField] private UIActionCancelPanel uiActionCancelPanel;
    
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject remoldElement;
    
    public Image skillSlotImgOutLine;
    public Image skillSelectedImg;
    
    //스킬슬롯패널에서 쓰기위함
    public int skillIndex;
    public Image skillSlot;
    public string skillName;
    public string skillLevel;
    public string skillDescription;
    public string skillTargetDescription;

    private List<GameObject> elements = new List<GameObject>();
    
   public void Awake()
   {
       skillSlot = transform.GetChild(0).GetComponent<Image>();
       
       skillSlotImgOutLine.enabled = false;
       skillSelectedImg.enabled = false;
       
       SkillButton = GetComponent<Button>();
   }

   public void SetRemolding(int count)
   {
       if (null == content) return;

       while (elements.Count < count)
           elements.Add(Instantiate(remoldElement, content.transform));

       foreach (var go in elements)
           go.SetActive(false);

       for (int i = 0; i < count; i++)
           elements[i].SetActive(true);
   }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (uiSelectionPanelObj.activeSelf)
        {
            return;
        }
        
        uiActionCancelPanel.Close();
        uiSkillInfoPanel.Open();
        skillSlotImgOutLine.enabled = true;
        skillSelectedImg.enabled = true;

        uiSkillInfoPanel.SetSkillInfoTxt(skillIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (uiSelectionPanelObj.activeSelf)
        {
            return;
        }
        
        uiSkillInfoPanel.Close();
        
        skillSlotImgOutLine.enabled = false;
        skillSelectedImg.enabled = false;
        uiActionCancelPanel.Open();
    }
}
