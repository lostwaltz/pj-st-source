using System.Collections;
using TMPro;
using UnityEngine;

public class UIReward : UIAnimation
{
    [SerializeField] private GameObject Interaction;
    [SerializeField] private TMP_Text RewardText;
    
    protected override void Start()
    {
        base.Start();
        
        Interaction.SetActive(false);
        
        BindEvent(Interaction, _ => Close());

        StartCoroutine(DelayInteract());
    }

    public void TakeReward(int amount)
    {
        RewardText.text = amount.ToString();
        Core.UGSManager.Data.CallSave();
        Open();
    }

    public override void Close()
    {
        base.Close();
        
        Core.UIManager.ReleaseUI<UIReward>();
    }
    protected override void CloseProcedure()
    {
        Destroy(gameObject);
    }

    private IEnumerator DelayInteract()
    {
        yield return new WaitForSeconds(0.5f);
        Interaction.SetActive(true);
    }
}
