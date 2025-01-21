using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGuideMessage : UIBase
{
    private UIGuide guide;
    
    [SerializeField] TextMeshProUGUI txtTitle;
    [SerializeField] TextMeshProUGUI txtMessage;

    private int page;
    [SerializeField] TextMeshProUGUI txtPage;
    [SerializeField] private Button btnNextContent;

    private GuideContent curContent;

    public void Initialize(UIGuide uiGuide)
    {
        guide = uiGuide;
    }
    
    public void SetContent(GuideContent content)
    {
        curContent = content;
        page = 0;
        CheckNextButton();
        UpdateContent();
    }

    public void OnClickPage(int direction)
    {
        page += direction;
        page = Mathf.Clamp(page, 0, curContent.desc.Length - 1);
        CheckNextButton();
        UpdateContent();
    }

    void UpdateContent()
    {
        txtTitle.text = curContent.title;
        txtMessage.text = curContent.desc[page];
        txtPage.text = $"{page + 1}/{curContent.desc.Length}";
    }

    public void OnClickNext()
    {
        SetContent(guide.GetContent(curContent.nextContent));
    }

    void CheckNextButton()
    {
        if(curContent.nextContent >= 0)
            btnNextContent.gameObject.SetActive(page == curContent.desc.Length - 1);
        else
            btnNextContent.gameObject.SetActive(false);
    }
}

