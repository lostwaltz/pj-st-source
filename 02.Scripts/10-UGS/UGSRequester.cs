using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;

namespace UGS
{
    public class UGSRequester
    {
        protected UGSManager manager => Core.UGSManager;
        
        protected async Task Request<T>(T request, Action onSuccess = null) where T : Task
        {
            var loading = Core.CommonUIManager.OpenUI<UIUGSLoading>(); 
            
            try
            {
                await request;
                onSuccess?.Invoke();
            }
            catch (AuthenticationException e)
            {
                //UI 알림
                Core.CommonUIManager.GetUI<UIInformPopup>().Initialize(e.Message);
            }
            catch (RequestFailedException e)
            {
                //UI 알림
                Core.CommonUIManager.GetUI<UIInformPopup>().Initialize(e.Message);
            }
            
            loading.Close();
        }
    
        protected async Task<T2> Request<T1, T2>(T1 request) where T1 : Task<T2> where T2 : class
        {
            var loading = Core.CommonUIManager.OpenUI<UIUGSLoading>(); 
            
            try
            {
                var result = await request;
                
                loading.Close();
                return result;
            }
            catch (AuthenticationException e)
            {
                //UI 알림
                Core.CommonUIManager.GetUI<UIInformPopup>().Initialize(e.Message);
            }
            catch (RequestFailedException e)
            {
                //UI 알림
                Core.CommonUIManager.GetUI<UIInformPopup>().Initialize(e.Message);
            }
            
            loading.Close();
            return null;
        }
    }
}