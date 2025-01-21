using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRareGachaPanel : UIBase
{
    [SerializeField] private TMP_Text unitCountText;
    [SerializeField] private TMP_Text oneAccessIconText;
    [SerializeField] private TMP_Text tenAccessIconText;
    [SerializeField] private Button oneAccessButton;
    [SerializeField] private Button tenAccessButton;

    void Start()
    {
        //TODO : 정보 세팅 되면 담아주기
        unitCountText.text = "1/260";
        oneAccessIconText.text = "X20";
        tenAccessIconText.text = "X200";
        if (Core.CurrencyManager.GetAmount<Credits>() < 20)
        {
            oneAccessIconText.color = new Color(255f / 255f, 95f / 255f, 64f / 255f);
        }
        else
        {
            oneAccessIconText.color = new Color(28f / 255f, 42f / 255f, 50f / 255f);
        }
        if (Core.CurrencyManager.GetAmount<Credits>() < 200)
        {
            tenAccessIconText.color = new Color(255f / 255f, 95f / 255f, 64f / 255f);
        }
        else
        {
            oneAccessIconText.color = new Color(28f / 255f, 42f / 255f, 50f / 255f);
        }
        oneAccessButton.onClick.AddListener(() => OnClickOneAccessBtn());
        tenAccessButton.onClick.AddListener(() => OnClickTenAccessBtn());
    }

    public void OnClickOneAccessBtn()
    {
        if (Core.CurrencyManager.Spend<Credits>(20) == false)
        {
            Core.UIManager.OpenUI<UIGachaWarningPanel>();
            return;
        }
        GachaSound.PlayGachaStart();
        Core.DataManager.GachaCount = 1;
        Core.SceneLoadManager.LoadScene("GachaScene");
    }
    public void OnClickTenAccessBtn()
    {
        if (Core.CurrencyManager.Spend<Credits>(200) == false)
        {
            Core.UIManager.OpenUI<UIGachaWarningPanel>();
            return;
        }
        GachaSound.PlayGachaStart();
        Core.DataManager.GachaCount = 10;
        Core.SceneLoadManager.LoadScene("GachaScene");
    }
}
