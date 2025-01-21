using TMPro;
using UnityEngine;

public class UILeftSkillInfoPanel : UIBase
{
    [SerializeField] private TMP_Text leftAdditionalSkillTxt;
    [SerializeField] private TMP_Text leftSkillNameTxt;
    [SerializeField] private TMP_Text leftSkillDescriptionTxt;
    
    [SerializeField] private UISkill[] uiSkill;
    
    public void SetSkillInfoTxt(int Index)
    {
        Open();
        
        //TODO : 데이터 테이블 세팅 추가 되면 맞는 정보 Text 담아주기
        leftAdditionalSkillTxt.text = $"{uiSkill[Index].skillTargetDescription}";
        
        leftSkillNameTxt.text = $"{uiSkill[Index].skillName} Lv.{uiSkill[Index].skillLevel}";
        leftSkillDescriptionTxt.text = $"{uiSkill[Index].skillDescription}";

    }
}
