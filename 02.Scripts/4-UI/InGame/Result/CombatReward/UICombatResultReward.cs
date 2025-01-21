using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UICombatResultReward : UIBase
{
    private StageSO currentStage;
    [SerializeField] private List<UICombatGoalSlot> goalSlots = new();
    [SerializeField] private List<ChallengeGoalSO> goalList = new();
    [SerializeField] private GameObject goalSlotPrefab;// Goal 슬롯 프리팹
    [SerializeField] private Transform goalSlotParent;

    [SerializeField] private List<UICombatRewardSlot> rewardSlots = new();
    [SerializeField] private List<RewardableFactory> rewardList = new();
    [SerializeField] private GameObject[] rewardSlotPrefab;// Goal 슬롯 프리팹
    [SerializeField] private Transform rewardSlotParent;

    [SerializeField] private TextMeshProUGUI stageNumberText;

    [SerializeField] private Button nextStageButton;
    [SerializeField] private Button nextStageQuitButton;
    [SerializeField] private Button confirmButton;

    void Start()
    {
        Init();


        goalList = StageManager.Instance.stageData.challengeGoals;

        for (int i = 0; i < goalList.Count; i++)
        {
            //TODO : SO갯수만큼, 추가해주고 동적 할당 하기
            GameObject goalSlot = Instantiate(goalSlotPrefab, goalSlotParent);
            UICombatGoalSlot slotSc = goalSlot.GetComponent<UICombatGoalSlot>();

            if (slotSc != null)
            {
                slotSc.SetChallengeSlot(goalList[i]);
                goalSlots.Add(slotSc);
            }
        }

        rewardList = StageManager.Instance.stageData.rewardableFactories;

        for (int i = 0; i < rewardList.Count; i++)
        {
            //TODO : SO갯수만큼, 추가해주고 동적 할당 하기

            if (rewardList[i].RewardType == RewardType.Credits)
            {
                GameObject rewardSlot = Instantiate(rewardSlotPrefab[0], rewardSlotParent);
                UICombatRewardSlot rewardSlotSc = rewardSlot.GetComponent<UICombatRewardSlot>();
                if (rewardSlotSc != null)
                {
                    rewardSlotSc.SetRewardSlot(rewardList[i]);
                    rewardSlots.Add(rewardSlotSc);
                }
            }
            else
            {
                GameObject rewardSlot = Instantiate(rewardSlotPrefab[1], rewardSlotParent);
                UICombatRewardSlot rewardSlotSc = rewardSlot.GetComponent<UICombatRewardSlot>();
                if (rewardSlotSc != null)
                {
                    rewardSlotSc.SetRewardSlot(rewardList[i]);
                    rewardSlots.Add(rewardSlotSc);
                }
            }
            StageManager.Instance.stageData.ApplyReward();
        }
    }

    public void Init()
    {
        currentStage = StageManager.Instance.stageData;
        if (currentStage != null)
        {
            stageNumberText.text = currentStage.stageName;
        }

        ExistNextStage();

        nextStageButton.onClick.AddListener(OnClickNextStageBtn);
        nextStageQuitButton.onClick.AddListener(OnClickConfirmBtn);
    }

    public void OnClickConfirmBtn()
    {
        if (0 != StageManager.Instance.stageData.endDialogueKey)
        {
            DialogManager.Instance.ShowDialog<UINovelDialog>(StageManager.Instance.stageData.endDialogueKey);
            UINovelDialog ui = DialogManager.Instance.GetCurDialog<UINovelDialog>();

            GameManager.Instance.ReleaseGameScene();

            ui.OnClose += () => Core.SceneLoadManager.LoadScene(SceneLoadManager.PrevScene);
        }

        else
            GameManager.Instance.BackToLobby();
    }

    public void OnClickNextStageBtn()
    {
        if (0 != StageManager.Instance.stageData.endDialogueKey)
        {
            DialogManager.Instance.ShowDialog<UINovelDialog>(StageManager.Instance.stageData.endDialogueKey);
            UINovelDialog ui = DialogManager.Instance.GetCurDialog<UINovelDialog>();

            GameManager.Instance.ReleaseGameScene();

            ui.OnClose += NextStage;
        }
        else
        {
            GameManager.Instance.ReleaseGameScene();

            NextStage();
        }
    }

    private void NextStage()
    {
        BGE.StopCurrentBGE();
        BGM.StopMainMenuBGM();

        if (currentStage == null) return;
        var nextStage = Core.DataManager.StageDict.TryGetValue(currentStage.stageData.StageKey + 1, out var nextStageSO) ? nextStageSO : null;
        if (nextStage == null) return;

        UISound.PlayStageBattleStart();

        Core.UIManager.GetUI<UIScreenFade>().FadeTo(0f, 1f, 0.2f)
            .OnComplete(() =>
            {
                Core.DataManager.SelectedStage = nextStage;
                Debug.Log(nextStage.sceneName);
                Core.SceneLoadManager.LoadScene("GameScene", nextStage.sceneName);
            });
    }


    private void ExistNextStage()
    {
        var nextStage = Core.DataManager.StageDict.TryGetValue(currentStage.stageData.StageKey + 1, out var nextStageSO) ? nextStageSO : null;

        // 다음 스테이지가 있는 경우
        if (nextStage != null)
        {
            nextStageButton.gameObject.SetActive(true);
            nextStageQuitButton.gameObject.SetActive(true);
            confirmButton.gameObject.SetActive(false);
        }
        // 다음 스테이지가 없는 경우
        else
        {
            nextStageButton.gameObject.SetActive(false);
            nextStageQuitButton.gameObject.SetActive(false);
            confirmButton.gameObject.SetActive(true);
        }
    }
}
