using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStartGachaPanel : UIBase
{
    [SerializeField] private TMP_Text tenAccessIconText;
    [SerializeField] private Button accessButton;

    private void Start()
    {
        tenAccessIconText.text = "X100";
        
        if (Core.CurrencyManager.GetAmount<Credits>() < 100)
        {
            tenAccessIconText.color = new Color(255f / 255f, 95f / 255f, 64f / 255f);
        }
        else
        {
            tenAccessIconText.color = new Color(255f / 255f, 255f / 255f, 0f / 255f);
        }
        accessButton.onClick.AddListener(() => OnClickAccessButton());
    }

    public void OnClickAccessButton()
    {
        if (Core.CurrencyManager.Spend<Credits>(100) == false)
        {
            Core.UIManager.OpenUI<UIGachaWarningPanel>();
            return;
        }
        GachaSound.PlayGachaStart();
        Core.DataManager.GachaCount = 10;
        Core.SceneLoadManager.LoadScene("GachaScene");
    }
}
