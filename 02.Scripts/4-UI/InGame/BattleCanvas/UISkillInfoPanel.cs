using TMPro;
using UnityEngine;

public class UISkillInfoPanel : UIBase
{
    public TMP_Text skillNameTxt;
    public TMP_Text skillDescriptionTxt;
    
    [SerializeField] private UISkill[] uiSkill;

    public void SetSkillInfoTxt(int Index)
    {
        skillNameTxt.text = $"{uiSkill[Index].skillName} Lv.{uiSkill[Index].skillLevel}";
        skillDescriptionTxt.text = $"{uiSkill[Index].skillDescription}";
    }

}
