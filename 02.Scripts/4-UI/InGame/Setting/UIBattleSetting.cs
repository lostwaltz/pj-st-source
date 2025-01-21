using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using DG.Tweening;
using EnumTypes;
using UGS;
public class UIBattleSetting : UIBase
{
    [Header("UI Component")]
    [SerializeField] private GameObject stageInfoWindow;
    [SerializeField] private CanvasGroup settingWindow;
    [SerializeField] private Button backButton;
    [SerializeField] private Button settingWindowOpenButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button continueButton;

    [Header("스테이지정보 창")]
    [SerializeField] private CanvasGroup stageInfoCanvasGroup;
    [SerializeField] private CanvasGroup stageInfoButton;
    [SerializeField] private TextMeshProUGUI txtStageTitle;
    [SerializeField] private TextMeshProUGUI txtStageDescription;
    [SerializeField] private Transform challengeGoalsGird;
    [SerializeField] private GameObject challengeGoalPrefab;
    [SerializeField] private Image stageInfoArrow;



    [Header("시스템설정 창")]
    [SerializeField] private CanvasGroup systemSettingCanvasGroup;
    [SerializeField] private CanvasGroup systemSettingButton;
    [SerializeField] private TextMeshProUGUI bgmVolumeText;
    [SerializeField] private TextMeshProUGUI effectVolumeText;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider effectVolumeSlider;
    [SerializeField] private Image systemSettingArrow;

    [Header("확인창")]
    [SerializeField] private CanvasGroup confirmWindow;
    [SerializeField] private Button confirmYesButton;
    [SerializeField] private Button confirmNoButton;

    [Header("Enemy Unit Thumbnail")]
    [SerializeField] private GameObject stageInfoPanel;
    [SerializeField] private Transform enemyUnitsGrid;
    [SerializeField] private GameObject enemyUnitPrefab;
    [SerializeField] private TextMeshProUGUI enemyUnitLevel;
    [SerializeField] private GameObject enemyUnitElite;
    [SerializeField] private GameObject enemyUnitBoss;

    [Header("Button Colors")]
    [SerializeField] private Color selectedColor = new Color(56f / 255f, 75f / 255f, 83f / 255f, 1f);
    [SerializeField] private Color normalColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private Color selectedTextColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private Color normalTextColor = new Color(56f / 255f, 75f / 255f, 83f / 255f, 1f);


    private StageSO currentStage;
    private float fadeInDuration = 0.5f;
    private float fadeOutDuration = 0.5f;
    private Image stageInfoButtonImage;
    private Image systemSettingButtonImage;
    private Button currentSelectedButton;


    private float previousTimeScale;



    void Start()
    {
        stageInfoButtonImage = stageInfoButton.GetComponent<Image>();
        systemSettingButtonImage = systemSettingButton.GetComponent<Image>();


        BindEvent(settingWindowOpenButton.gameObject, _ =>
        {
            Open();
        });
        BindEvent(backButton.gameObject, _ =>
        {
            ResumeGame();
            UISound.PlayBackButtonClick();
            Close();
        });


        BindEvent(stageInfoButton.gameObject, _ =>
        {
            SelectButton(stageInfoButton.GetComponent<Button>());
            OpenStageInfoTap();
        });
        BindEvent(systemSettingButton.gameObject, _ =>
        {
            SelectButton(systemSettingButton.GetComponent<Button>());
            OpenSystemSettingTap();
        });

        BindEvent(stageInfoButton.gameObject, (eventData) =>
        {
            if (currentSelectedButton != stageInfoButton.GetComponent<Button>())
            {
                stageInfoButton.transform.DOScale(1.05f, 0.2f);
                stageInfoButton.GetComponent<Image>()
                .DOColor(new Color(1f, 1f, 1f, 1f), 0f)
                .SetUpdate(true);
            }
        }, UIEvent.Enter);

        BindEvent(stageInfoButton.gameObject, (eventData) =>
        {
            if (currentSelectedButton != stageInfoButton.GetComponent<Button>())
            {
                stageInfoButton.transform.DOScale(1f, 0.2f);
                stageInfoButton.GetComponent<Image>()
                .DOColor(new Color(0.7f, 0.7f, 0.7f, 1f), 0f)
                .SetUpdate(true);
            }
        }, UIEvent.Exit);

        BindEvent(systemSettingButton.gameObject, (eventData) =>
        {
            if (currentSelectedButton != systemSettingButton.GetComponent<Button>())
            {
                systemSettingButton.transform
                .DOScale(1.05f, 0.2f)
                .SetUpdate(true);
                systemSettingButton.GetComponent<Image>()
                .DOColor(new Color(1f, 1f, 1f, 1f), 0f)
                .SetUpdate(true);
            }
        }, UIEvent.Enter);

        BindEvent(systemSettingButton.gameObject, (eventData) =>
        {
            if (currentSelectedButton != systemSettingButton.GetComponent<Button>())
            {
                systemSettingButton.transform
                .DOScale(1f, 0.2f)
                .SetUpdate(true);
                systemSettingButton.GetComponent<Image>()
                .DOColor(new Color(0.7f, 0.7f, 0.7f, 1f), 0f)
                .SetUpdate(true);
            }
        }, UIEvent.Exit);







        BindEvent(continueButton.gameObject, _ =>
        {
            UIBattle.PlayUIBattleClickNormalSound();
            ResumeGame();
            Close();
        });

        BindEvent(exitButton.gameObject, _ =>
        {
            //확인창띄우기
            UISound.PlayStageNodeClick();
            UIBattle.PlayUIBattleCharacterFilteringWindowOpen();
            confirmWindow.gameObject.SetActive(true);
            confirmWindow.alpha = 0f;
            DOTween.Sequence()
            .SetUpdate(true)
            .Join(confirmWindow.DOFade(1f, fadeInDuration));
        });
        BindEvent(confirmYesButton.gameObject, _ =>
        {
            ResumeGame();
            //메인메뉴로
            BGE.StopCurrentBGE();
            BGM.PlayBGMAfterFadeOut(() =>
            {
                BGM.PlayMainMenuBGM();
            });

            Core.UIManager.GetUI<UIScreenFade>().FadeTo(0f, 1f, 0.2f)
            .OnComplete(() =>
            {
                // Core.SceneLoadManager.OnSceneLoadComplete += () => Core.UIManager.GetUI<UIStageSelect>().Open();
                GameManager.Instance.BackToLobby();
            });
        });
        BindEvent(confirmNoButton.gameObject, _ =>
        {
            UISound.PlayStageNodeClick();
            UIBattle.PlayUIBattleCharacterFilteringWindowClose();
            DOTween.Sequence()
            .SetUpdate(true)
            .Join(confirmWindow.DOFade(0f, fadeOutDuration))
            .OnComplete(() =>
            {
                confirmWindow.gameObject.SetActive(false);
            });
        });
    }


    public void Init()
    {
        currentStage = StageManager.Instance.stageData;
        if (currentStage != null)
        {
            txtStageTitle.text = currentStage.stageName;
            txtStageDescription.text = currentStage.stageDescription;
        }

        // 튜토리얼 중일 때 exitButton 비활성화
        exitButton.gameObject.SetActive(TutorialManager.Instance.IsCompleteTutorial);

        UpdateUI();
        InitVolume();
    }

    private void PauseGame()
    {
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = previousTimeScale;
        }
    }

    public override void Open()
    {
        base.Open();
        UISound.PlayChangeScene();
        stageInfoWindow.SetActive(true);
        settingWindow.alpha = 0f;
        stageInfoButton.alpha = 0f;
        systemSettingButton.alpha = 0f;

        PauseGame();

        DOTween.Sequence()
            .SetUpdate(true)
            .Join(settingWindow.DOFade(1f, fadeInDuration))
            .Join(stageInfoButton.DOFade(1f, fadeInDuration + 0.5f))
            .Join(systemSettingButton.DOFade(1f, fadeInDuration + 0.5f));
    }

    public override void Close()
    {
        float fadeOutDuration = 0.2f;

        ResumeGame();

        DOTween.Sequence()
           .SetUpdate(true)
           .Join(stageInfoButton.DOFade(0f, fadeOutDuration + 0.3f))
           .Join(systemSettingButton.DOFade(0f, fadeOutDuration + 0.3f))
           .Join(settingWindow.DOFade(0f, fadeOutDuration))
           .OnComplete(() =>
           {
               stageInfoWindow.SetActive(false);
           });
    }

    private void OnDisable()
    {
        ResumeGame();
    }

    private void SelectButton(Button selectedButton)
    {
        if (currentSelectedButton != null)
        {
            Image previousArrow = currentSelectedButton == stageInfoButton.GetComponent<Button>()
            ? stageInfoArrow
            : systemSettingArrow;

            previousArrow.transform.DOComplete();
            previousArrow.GetComponent<Image>().DOComplete();

            currentSelectedButton.GetComponent<Image>().color = normalColor;
            currentSelectedButton.GetComponentInChildren<TextMeshProUGUI>().color = normalTextColor;

            previousArrow.color = new Color(240 / 255f, 175 / 255f, 22 / 255f, 0f);
        }

        selectedButton.transform
        .DOScale(1f, 0.2f)
        .SetUpdate(true);

        selectedButton.GetComponent<Image>().color = selectedColor;
        selectedButton.GetComponentInChildren<TextMeshProUGUI>().color = selectedTextColor;

        Image currentArrow = selectedButton == stageInfoButton.GetComponent<Button>()
        ? stageInfoArrow
        : systemSettingArrow;

        currentArrow.DOFade(1f, 1f)
        .SetUpdate(true)
        .SetEase(Ease.OutQuad);

        currentSelectedButton = selectedButton;
    }

    private void UpdateUI()
    {
        if (currentStage == null) return;

        //적 유닛 썸네일 
        UpdateEnemyUnitThumbnail();
        //도전목표
        UpdateChallengeGoals();

    }

    private void OpenSystemSettingTap()
    {
        UISound.PlayStageNodeClick();
        stageInfoCanvasGroup.gameObject.SetActive(false);
        systemSettingCanvasGroup.gameObject.SetActive(true);
        systemSettingCanvasGroup.alpha = 0f;
        DOTween.Sequence()
        .SetUpdate(true)
        .Join(systemSettingCanvasGroup.DOFade(1f, fadeInDuration));
    }
    private void OpenStageInfoTap()
    {
        UISound.PlayStageNodeClick();
        systemSettingCanvasGroup.gameObject.SetActive(false);
        stageInfoCanvasGroup.gameObject.SetActive(true);
        stageInfoCanvasGroup.alpha = 0f;
        DOTween.Sequence()
        .SetUpdate(true)
        .Join(stageInfoCanvasGroup.DOFade(1f, fadeInDuration));
    }

    private void UpdateEnemyUnitThumbnail()
    {
        ClearGrid(enemyUnitsGrid);
        if (currentStage.enemyPlacements == null || currentStage.enemyPlacements.Count == 0) return;
        var enemyGroups = currentStage.enemyPlacements
        .GroupBy(p => p.unitKey)
        .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var group in enemyGroups)
        {
            var unitInfo = Core.DataManager.UnitTable.GetByKey(group.Key);
            if (unitInfo == null) continue;

            CreateEnemyUnitUI(unitInfo, group.Value.Max(p => p.level));

        }

    }

    private void CreateEnemyUnitUI(UnitInfo unitInfo, int level)
    {
        GameObject enemyUnitObj = Instantiate(enemyUnitPrefab, enemyUnitsGrid);

        Image unitImage = enemyUnitObj.GetComponentInChildren<Image>();
        var enemyImage = enemyUnitObj.GetComponentInChildren<UIStageInfoEnemyImage>();
        if (unitImage != null)
        {
            var sprite = Resources.Load<Sprite>(unitInfo.Path);
            enemyImage.enemyHeadImage.sprite = sprite ?? throw new System.Exception($"경로에 스프라이트이미지없음 {unitInfo.Path}");
        }

        TextMeshProUGUI levelText = enemyUnitObj.GetComponentInChildren<TextMeshProUGUI>();
        if (levelText != null)
        {
            levelText.text = $"Lv. {level}";
        }
    }
    private void ClearGrid(Transform grid)
    {
        if (grid == null) return;

        foreach (Transform child in grid)
        {
            Destroy(child.gameObject);
        }
    }

    private void UpdateChallengeGoals()
    {
        ClearGrid(challengeGoalsGird);

        if (currentStage.challengeGoals == null || currentStage.challengeGoals.Count == 0)
        {
            GameObject goalObj = Instantiate(challengeGoalPrefab, challengeGoalsGird);
            goalObj.GetComponentInChildren<TextMeshProUGUI>().text = "도전 목표가 없습니다.";
            return;
        }

        var container = Core.UGSManager.Data.CloudDatas[typeof(ChallengeObjective)] as CloudDataContainer<ChallengeObjective>;
        if (container == null) return;

        var stageKey = currentStage.stageData.StageKey;
        var dataList = container.GetData() as List<ChallengeObjective>;

        foreach (var goal in currentStage.challengeGoals)
        {
            GameObject goalObj = Instantiate(challengeGoalPrefab, challengeGoalsGird);
            UICombatGoalSlotForSettingWindow goalSlot = goalObj.GetComponent<UICombatGoalSlotForSettingWindow>();

            if (goalSlot != null)
            {
                bool isCompleted = false;
                if (dataList != null)
                {
                    isCompleted = dataList.Any(data =>
                        data.StageKey == stageKey &&
                        data.goalKey == goal.goalKey &&
                        data.isCompleted);
                }

                if (isCompleted)
                {
                    goalSlot.SetCompletedGoal(goal);
                }
                else
                {
                    goalSlot.SetUncompletedGoal(goal);
                }
            }
        }
    }

    //TODO: 믹서 볼륨 조절로 변경해야함
    private void InitVolume()
    {
        bgmVolumeSlider.value = MusicManager.Instance.MaxVolume * 100;
        UpdateBGMVolumeText(bgmVolumeSlider.value);
        bgmVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChanged);

        effectVolumeSlider.value = Core.SoundManager.MasterVolume;
        UpdateEffectVolumeText(effectVolumeSlider.value);
        effectVolumeSlider.onValueChanged.AddListener(OnEffectVolumeChanged);
    }

    private void OnBGMVolumeChanged(float value)
    {
        MusicManager.Instance.MaxVolume = value * 0.1f;
        UpdateBGMVolumeText(value);
    }

    private void OnEffectVolumeChanged(float value)
    {
        Core.SoundManager.MasterVolume = value;
        UpdateEffectVolumeText(value);
    }


    private void UpdateBGMVolumeText(float value)
    {
        bgmVolumeText.text = $"{Mathf.RoundToInt(value * 100)}";
    }
    private void UpdateEffectVolumeText(float value)
    {
        effectVolumeText.text = $"{Mathf.RoundToInt(value * 100)}";
    }
}


