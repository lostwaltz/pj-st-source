using UGS;

public class UGSManager
{
    public Authentication Auth { get; private set; }
    public CloudData Data { get; private set; }
    
    
        
    public void Init()
    {
        Auth = new Authentication();
        Data = new CloudData();
            
        Auth.Initialize();
    }

    public void StartGame()
    {
        Core.SceneLoadManager.LoadScene("LobbyScene");
    }
}   

