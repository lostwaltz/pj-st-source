using TMPro;
using UnityEngine;


public class UICombatRewardSlot : UIBase
{
    [SerializeField] private TMP_Text rewardCountText;

    public void SetRewardSlot(RewardableFactory reward)
    {
        rewardCountText.text = reward.Amount.ToString();
    }
}
