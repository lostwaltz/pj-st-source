using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;

namespace UGS
{
    public class Authentication : UGSRequester
    {
        // 3~20자 문자, 숫자, 특수기호: . - @ _
        public readonly Regex UsernameRegex = new Regex(@"^[a-zA-Z0-9.\\@_]{3,20}$");
        // 8~30, 대소문자 최소 1개 포함, 숫자, 특수문자 1개 포함
        public readonly Regex PasswordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?"":{}|<>])[A-Za-z\d!@#$%^&*(),.?"":{}|<>]{8,30}$");
        
        public bool IsOnline { get; private set; }
        public bool IsAnonymous { get; private set; }

        public event Action OnSignUp; // 첫 접속. 기본 재화 제공
        public event Action OnSignIn; // 로그인. 데이터 불러오기

        
        public void Initialize()
        {
            UIAuthentication ui = Core.UIManager.GetUI<UIAuthentication>();
            ui.Open();
            
            Connect(ui.Login.Open);

            IsOnline = false;
            IsAnonymous = false;
        }

        public async void Connect(Action onConnected = null)
        {
            await Request(UnityServices.InitializeAsync(), () =>
            {
                IsOnline = true;
                onConnected?.Invoke();
            });
        }
        
        
        // 회원가입
        public async void SignUp(string username, string password)
        {
            IsAnonymous = false;
            await Request(AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password), OnSignUpEvent);
        }
        
        
        // 익명 로그인
        public async void SignInAnonymously()
        {
            IsAnonymous = true;
            await Request(AuthenticationService.Instance.SignInAnonymouslyAsync(), OnSignUpEvent);
        }

        public void OnSignUpEvent()
        {
            OnSignUp?.Invoke(); // 기본 제화 제공
            
            manager.Data.CallSave();
            manager.StartGame(); // 게임 시작
        }
        
        // 로그인
        public async void SignInUsername(string username, string password)
        {
            IsAnonymous = false;
            await Request(AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password), 
                () =>
                {
                    // 로그인 완료
                    OnSignIn?.Invoke();
                });
        }
        
        // 로그아웃
        public void SignOut()
        {
            AuthenticationService.Instance.SignOut();
        }
    }  
}