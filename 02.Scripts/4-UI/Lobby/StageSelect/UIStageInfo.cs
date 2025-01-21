using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using EnumTypes;
using System;
using UGS;
/// <summary>
/// 스테이지 정보를 표시하는 UI 컴포넌트
/// </summary>
public class UIStageInfo : UIBase
{
    [Header("스테이지 정보")]
    [SerializeField] private TextMeshProUGUI txtStageTitle;
    [SerializeField] private TextMeshProUGUI txtRecommendedLevel;
    [SerializeField] private TextMeshProUGUI txtStageDescription;
    [SerializeField] private Image stageChapterImage;
    [SerializeField] private Transform challengeGoalLayout;
    [SerializeField] private GameObject challengeGoalPrefab;

    [Header("적 유닛 썸네일")]
    [SerializeField] private Transform enemyUnitsGrid;
    [SerializeField] private GameObject enemyUnitPrefab;


    [Header("특수보상")]


    [Header("UI Components")]
    [SerializeField] private Button btnStart;
    [SerializeField] private RectTransform backgroundclose;
    [SerializeField] private RectTransform stageInfoRect;
    [SerializeField] private GameObject contentPanel;
    [SerializeField] private CanvasGroup contentCanvasGroup;
    [SerializeField] private Image OutLineOnEnter;


    [Header("애니메이션 설정")]
    [SerializeField] private float slideOffsetRatio = 1.2f; // 화면 너비의 120%
    [SerializeField] private float minTargetPosition = -320f;  // 16:9에서의 최소 위치
    [SerializeField] private float maxTargetPosition = -220f; // 21:9에서의 최대 위치
    private StageSO currentStage;
    private float slideDuration = 0.1f;
    private float fadeInDuration = 0.2f;
    private float fadeOutDuration = 0.2f;

    private Canvas cachedCanvas;
    private RectTransform cachedCanvasRect;
    private void Awake()
    {
        cachedCanvas = GetComponentInParent<Canvas>();
        if (cachedCanvas != null)
        {
            cachedCanvasRect = cachedCanvas.GetComponent<RectTransform>();
        }
    }


    private void Start()
    {
        backgroundclose = Core.UIManager.GetUI<UIStageSelect>()?.backgroundclose;
        backgroundclose?.GetComponent<Button>()?.onClick.AddListener(Close);
        btnStart.onClick.AddListener(OnStartButtonClicked);
        InitializePosition();

        BindEvent(btnStart.gameObject, _ =>
        {
            if (OutLineOnEnter != null)
            {
                OutLineOnEnter.enabled = true;
            }
        }, UIEvent.Enter);

        BindEvent(btnStart.gameObject, _ =>
        {
            if (OutLineOnEnter != null)
            {
                OutLineOnEnter.enabled = false;
            }
        }, UIEvent.Exit);
    }

    private void OnEnable()
    {
        Core.UGSManager.Data.OnAllDataLoaded += OnCloudDataReceived;
    }
    private void OnDisable()
    {
        Core.UGSManager.Data.OnAllDataLoaded -= OnCloudDataReceived;
    }





    private float GetSlideDistance()
    {
        if (cachedCanvasRect == null)
        {
            return 1920f * slideOffsetRatio;
        }
        return cachedCanvasRect.rect.width * slideOffsetRatio;
    }
    private float GetTargetPosition()
    {
        if (cachedCanvasRect == null)
        {
            return minTargetPosition;
        }

        // 현재 화면의 종횡비 계산
        float currentAspectRatio = cachedCanvasRect.rect.width / cachedCanvasRect.rect.height;

        // 16:9와 21:9 비율
        float ratio16_9 = 16f / 9f;  // 약 1.778
        float ratio21_9 = 21f / 9f;  // 약 2.333

        // 현재 비율이 16:9와 21:9 사이에서 어느 정도인지 계산 (0~1 사이 값)
        float t = Mathf.InverseLerp(ratio16_9, ratio21_9, currentAspectRatio);

        float targetPosition = Mathf.Lerp(minTargetPosition, maxTargetPosition, t);

        return targetPosition;
    }

    private void InitializePosition()
    {
        if (contentPanel != null)
        {
            contentPanel.transform.localPosition = new Vector3(GetSlideDistance(), 0, 0);
        }
    }

    public override void Open()
    {
        base.Open();
        UISound.PlayStageInfoUIStart();
        if (contentPanel != null)
        {
            contentCanvasGroup.alpha = 0f;
            contentPanel.transform.localPosition = new Vector3(GetSlideDistance(), 0, 0);

            DOTween.Sequence()
            .Join(contentPanel.transform.DOLocalMoveX(GetTargetPosition(), slideDuration).SetEase(Ease.OutQuint))
            .Join(contentCanvasGroup.DOFade(1f, fadeInDuration));
        }
    }

    public override void Close()
    {
        if (contentPanel != null)
        {
            DOTween.Sequence()
            .Join(contentPanel.transform.DOLocalMoveX(GetSlideDistance(), slideDuration).SetEase(Ease.InQuint))
            .Join(contentCanvasGroup.DOFade(0f, fadeOutDuration))
            .OnComplete(() => base.Close());
        }
    }


    private void OnCloudDataReceived(Dictionary<Type, ICloudDataContainer> cloudDatas)
    {
        // 현재 스테이지 정보가 있을 때만 업데이트
        if (currentStage != null)
        {
            UpdateChallengeGoal();
        }
    }
    public void ShowStageInfo(StageSO stage)
    {
        if (stage == null) return;

        currentStage = stage;

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        // 현재 UI가 활성화되어 있는 경우
        if (contentPanel != null)
        {
            DOTween.Sequence()
                .Join(contentPanel.transform.DOLocalMoveX(GetSlideDistance(), slideDuration).SetEase(Ease.InQuint))
                .Join(contentCanvasGroup.DOFade(0f, fadeOutDuration))
                .OnComplete(() =>
                {
                    UpdateBasicInfo();
                    UpdateEnemyUnit();
                    UpdateChallengeGoal();
                    Open();
                });
        }
        else
        {
            UpdateBasicInfo();
            UpdateEnemyUnit();
            UpdateChallengeGoal();
            Open();
        }
    }

    private void UpdateBasicInfo()
    {
        txtStageTitle.text = currentStage.stageName;
        txtStageDescription.text = currentStage.stageDescription;
        txtRecommendedLevel.text = $"추천 레벨: {currentStage.recommendedLevel}";
    }


    /// <summary>
    /// 도전목표
    /// </summary>
    private void UpdateChallengeGoal()
    {
        ClearGrid(challengeGoalLayout);

        if (currentStage.challengeGoals == null || currentStage.challengeGoals.Count == 0)
        {
            GameObject defaultGoal = Instantiate(challengeGoalPrefab, challengeGoalLayout);
            defaultGoal.GetComponentInChildren<TextMeshProUGUI>().text = "도전 목표가 없습니다.";
            return;
        }


        var container = Core.UGSManager.Data.CloudDatas[typeof(ChallengeObjective)] as CloudDataContainer<ChallengeObjective>;

        if (container == null || container.GetData() == null)
        {
            return;
        }

        var dataList = container.GetData() as List<ChallengeObjective>;

        foreach (var goal in currentStage.challengeGoals)
        {
            GameObject goalObj = Instantiate(challengeGoalPrefab, challengeGoalLayout);
            var goalSlot = goalObj.GetComponent<UICombatGoalSlotForStageInfoWindow>();
            if (goalSlot != null)
            {
                goalSlot.SetChallengeSlotInfo(goal);
            }
        }
    }


    /// <summary>
    /// 적유닛이미지
    /// </summary>
    private void UpdateEnemyUnit()
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

            CreateEnemyUnitUI(unitInfo, group.Value);
        }
    }

    private void CreateEnemyUnitUI(UnitInfo unitInfo, List<EnemyPlacement> enemyPlacements)
    {
        var enemyUnitObj = Instantiate(enemyUnitPrefab, enemyUnitsGrid);
        var unitImage = enemyUnitObj.GetComponentInChildren<Image>();
        var enemyImage = enemyUnitObj.GetComponentInChildren<UIStageInfoEnemyImage>();
        var maxLevelPlacement = enemyPlacements.OrderByDescending(p => p.level).First();

        if (unitImage != null)
        {
            var sprite = Resources.Load<Sprite>(unitInfo.Path);
            if (sprite == null)
            {
                Debug.LogError($"경로에 스프라이트이미지없음 {unitInfo.Path}");
                return;
            }
            enemyImage.enemyHeadImage.sprite = sprite;
        }

        var levelText = enemyUnitObj.GetComponentInChildren<TextMeshProUGUI>();
        if (levelText != null)
        {
            levelText.text = $"Lv. {maxLevelPlacement.level}";
        }

        enemyImage.eliteIcon.SetActive(maxLevelPlacement.enemyType == EnemyType.Elite);
        enemyImage.bossIcon.SetActive(maxLevelPlacement.enemyType == EnemyType.Boss);


    }

    public void OnStartButtonClicked()
    {
        BGE.StopCurrentBGE();
        BGM.StopMainMenuBGM();
        if (currentStage == null) return;
        UISound.PlayStageBattleStart();


        Core.UIManager.GetUI<UIScreenFade>().FadeTo(0f, 1f, 0.2f)
            .OnComplete(() =>
            {
                Core.DataManager.SelectedStage = currentStage;
                Core.SceneLoadManager.LoadScene("GameScene", currentStage.sceneName);
            });
    }

    private void ClearGrid(Transform grid)
    {
        if (grid == null) return;

        foreach (Transform child in grid)
        {
            Destroy(child.gameObject);
        }
    }
}
