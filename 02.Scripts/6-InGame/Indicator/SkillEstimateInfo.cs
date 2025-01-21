using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillEstimateInfo : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI text;
    
    public void Set(int percent)
    {
        text.text = $"{percent}%";
    }
    
    public void Set(int percent, Sprite sprite)
    {
        icon.sprite = sprite;
        Set(percent);
    }
}
