public class UISignUp : UIUserSigning
{
    public void OnClickSignUp()
    {
        UIBattle.PlayUIBattleClickNormalSound();
        if (usernameMatched && passwordMatched)
            Core.UGSManager.Auth.SignUp(usernameInput.text, passwordInput.text);
    }

    public void OnClickGoBack()
    {
        UISound.PlayBackButtonClick();
        Close();
        Core.UIManager.GetUI<UIAuthentication>().Login.Open();
    }

}
