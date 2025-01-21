using UnityEngine;

public class UIAuthentication : UIBase
{
    public UILogin Login;
    public UISignUp SignUp;

    private void Awake()
    {
        if (Login == null)
        {
            Login = Core.UIManager.GetUI<UILogin>();
            InitTransform(Login.transform as RectTransform);
        }

        if (SignUp == null)
        {
            SignUp = Core.UIManager.GetUI<UISignUp>();
            InitTransform(SignUp.transform as RectTransform);
        }
        
        Login.gameObject.SetActive(false);
        SignUp.gameObject.SetActive(false);
    }

    public void InitTransform(RectTransform rect)
    {
        rect.SetParent(transform);
        rect.anchoredPosition = Vector2.zero;
    }
}
