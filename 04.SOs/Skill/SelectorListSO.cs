using UnityEngine;

[CreateAssetMenu(fileName = "SelectorListSO", menuName = "SOs/Selector/List")]
public class SelectorListSO : ScriptableObject
{
    public SelectorSO[] selectors;
    public SelectorSO GetSelector(SkillSelectType type)
    {
        return selectors[(int)type];
    }
}
