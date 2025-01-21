using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UILoadingScene : UIBase
{
    public Image ImgLoadingLine;
    public TMP_Text TextPer;
    public TMP_Text TextDesc;

    private readonly List<string> textList = new();
    
    private void Start()
    {
        Core.SceneLoadManager.OnLoadingProgressUpdated += CallbackProgressUpdated;
        textList.Add("WASD로 카메라를 움직이고 Q와 E로 회전할수 있습니다.");
        // textList.Add("전설은 말한다. 가장 완벽한 코드는 마지막 저장 직전에 날아간다고...");
        // textList.Add("연휴는 코드와 함께.");
        // textList.Add("모든 Stack Overflow 답변을 가진 유일한 스레드가 있다고 한다... 링크는 사라졌지만..");
        
        TextDesc.text = textList[Random.Range(0, textList.Count)];
    }

    private void CallbackProgressUpdated(float progress)
    {
        ImgLoadingLine.fillAmount = progress;
        TextPer.text = Utils.Str.Clear().Append(((int)(progress * 100)).ToString()).Append("%").ToString();
    }

    private void OnDisable()
    {
        Core.SceneLoadManager.OnLoadingProgressUpdated -= CallbackProgressUpdated;
    }
}
