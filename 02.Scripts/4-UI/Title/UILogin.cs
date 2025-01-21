public class UILogin : UIUserSigning
{
    public void OnClickLogin()
    {
        UIBattle.PlayUIBattleClickNormalSound();
        if (usernameMatched && passwordMatched)
            Core.UGSManager.Auth.SignInUsername(usernameInput.text, passwordInput.text);
    }

    public void OnClickLoginAnonymously()
    {
        UISound.PlayLobbyStartButtonClick();
        Core.UGSManager.Auth.SignInAnonymously();
    }

    public void OnClickSignUp()
    {
        UIBattle.PlayUIBattleClickNormalSound();
        Close();
        Core.UIManager.GetUI<UIAuthentication>().SignUp.Open();
    }

}