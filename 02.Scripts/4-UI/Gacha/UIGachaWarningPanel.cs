using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGachaWarningPanel : UIBase
{
    [SerializeField] private Button confirmBtn;

    public void Start()
    {
        confirmBtn.onClick.AddListener(CloseUI);
    }

    protected override void OpenProcedure()
    {
        UISound.PlayError();
    }

    public void CloseUI()
    {
        UISound.PlayBackButtonClick();
        gameObject.SetActive(false);
    }

}
