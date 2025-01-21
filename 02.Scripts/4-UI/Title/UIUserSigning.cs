using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public abstract class UIUserSigning : UIBase
{
    protected Regex UsernameRegex => Core.UGSManager.Auth.UsernameRegex;
    protected Regex PasswordRegex => Core.UGSManager.Auth.PasswordRegex;
    
    [SerializeField] protected bool usernameMatched;
    [SerializeField] protected bool passwordMatched;
    
    [SerializeField] protected TMP_InputField usernameInput;
    [SerializeField] protected TMP_InputField passwordInput;

    [SerializeField] private GameObject usernameVaildMark;
    [SerializeField] private GameObject usernameInvaildMark;
    [SerializeField] private GameObject passwordVaildMark;
    [SerializeField] private GameObject passwordInvaildMark;
        
    public void OnEndEditUsername(string value)
    {
        usernameMatched = UsernameRegex.IsMatch(value);
        
        usernameVaildMark.SetActive(usernameMatched);
        usernameInvaildMark.SetActive(!usernameMatched);
    }

    public void OnEndEditPassword(string value)
    {
        passwordMatched = PasswordRegex.IsMatch(value);
        
        passwordVaildMark.SetActive(passwordMatched);
        passwordInvaildMark.SetActive(!passwordMatched);
    }

    public void TogglePasswordDisplay(bool isOn)
    {
        passwordInput.contentType = isOn ? TMP_InputField.ContentType.Password : TMP_InputField.ContentType.Standard;
        passwordInput.Select();
    }
}