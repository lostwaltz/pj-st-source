using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIUnitInfoCanvas : UIBase
{
    [SerializeField] private TextMeshProUGUI unitNameTxt;

    [SerializeField] private Image hpBar;

    [SerializeField] UIStability stability;
    [SerializeField] UIShield shield;
    [SerializeField] private Color[] hpColorsByUnit;
    //0 : 파란색 - PlayerUnit | 1 : 빨간색 - EnemyUnit

    [SerializeField] private CanvasGroup canvasGroup;

    private Unit unitSc;
    private Camera mainCam;

    [Header("행동 완료 여부")]
    [SerializeField] private GameObject actionDoneIcon;

    [Header("스킬 발동 정보")]
    [SerializeField] private CanvasGroup skillCanvasGroup;
    [SerializeField] private Image skillIcon;
    [SerializeField] private TextMeshProUGUI skillNameTxt;

    //체력바 애니메이션 변수들
    [Header("Health Bar Animation")]
    [SerializeField] private float damageAnimationDuration = 0.5f; // 데미지 애니메이션 지속 시간

    bool animationComplete = false;
    Sequence healthBarSequence;
    Queue<int> damageQueue = new Queue<int>();
    Coroutine damageAnimCoroutine;

    WaitUntil until;

    private void Start()
    {
        mainCam = Camera.main;

        canvasGroup = GetComponent<CanvasGroup>();
        unitSc = GetComponentInParent<Unit>();

        //TODO : Unit은 부모오브젝트가 될 것이니, Transform을 받아오는 방법으로 구현하기
        //unit = GameObject.GetComponentsInChildren<GameObject>();
        //transform.position = unit.transform.position + new Vector3(0, 2f, 0);

        transform.position = unitSc.Requirement.hpPoint.position;
        //캔버스 위치 강제 삽입

        SetInfo();

        canvasGroup.alpha = 0.5f;

        GameManager.Instance.Interaction.OnClicked -= SetAlphaHealthBar;
        GameManager.Instance.Interaction.OnClicked += SetAlphaHealthBar;

        GameUnitManager.Instance.OnGameExit += () => GameManager.Instance.Interaction.OnClicked -= SetAlphaHealthBar;
        unitSc.OnReleaseUnit += () => GameManager.Instance.Interaction.OnClicked -= SetAlphaHealthBar;

        SetHeathBarColor(unitSc);

        if (unitSc.type == GameUnitManager.Playable)
        {
            unitSc.CommandSystem.OnStandBy += () => { SetActionDoneIcon(false); };
            unitSc.CommandSystem.OnCommandEnded += () => { SetActionDoneIcon(true); };
        }

        unitSc.HealthSystem.OnDamageTaken += PlayHealthBarAnimation;
        unitSc.HealthSystem.OnDamageTaken += ShowDamageHUD;
        unitSc.HealthSystem.OnHealthRestored += (percent) => hpBar.fillAmount = percent;

        CameraSystem.EventHandler.Subscribe(CameraEventTrigger.OnUnitActivatePassive, SetSkillInfo);

    }

    private void LateUpdate()
    {
        SetHealthBarPosition();
    }

    private void SetInfo()
    {
        UnitInstance instance = unitSc.data;
        UnitInfo unitInfo = instance.UnitBase;

        unitNameTxt.text = unitInfo.Name;
        stability.SetInfo(unitSc, (value, max) => { return value.ToString(); });// stabilityTxt.text = $"{instance.Level}";
        shield.SetInfo(unitSc);
    }

    private void SetHealthBarPosition()
    {
        transform.forward = mainCam.transform.forward;
    }

    private void SetAlphaHealthBar(IClickable thing)
    {
        if (thing is not Unit)
            return;

        if (thing as Unit == unitSc)
        {
            canvasGroup.alpha = 1f;
        }
        else
        {
            canvasGroup.alpha = 0.5f;
        }
    }

    private void ChangeHealthBar(float setHealth)
    {
        hpBar.fillAmount = setHealth;
    }

    private void SetHeathBarColor(Unit unit)
    {
        if (unitSc == unit is PlayerUnit)
        {
            hpBar.color = hpColorsByUnit[0];
        }
        else
        {
            hpBar.color = hpColorsByUnit[1];
        }
    }

    public void SetSkillInfo(CameraEventContext e)
    {
        if (e.Command.GetUnit() != unitSc)
            return;

        var skill = e.Command.GetUnit().SkillSystem.GetSkill(4);
        var skillInfo = Core.DataManager.SkillTable.GetByKey(skill.Data.SkillBase.Key);

        skillIcon.sprite = Resources.Load<Sprite>(skillInfo.Path);
        skillNameTxt.text = skillInfo.Name;

        skillCanvasGroup.DOFade(1, 1f)
        .SetEase(Ease.OutQuad)
        .OnComplete(() =>
        {
            skillCanvasGroup.DOFade(0, 0.3f).SetDelay(2f);
        });
    }

    public void SetActionDoneIcon(bool isOn)
    {
        actionDoneIcon.SetActive(isOn);
    }

    public void ShowDamageHUD(int damage)
    {
        DamageHUD damageHUD = Core.ObjectPoolManager.GetPooledObject<DamageHUD>("DamageHUD");
        if (damageHUD != null)
        {
            damageHUD.Initialize(transform.position, damage);
        }
    }

    public void PlayHealthBarAnimation(int damage)
    {
        damageQueue.Enqueue(damage);

        if (damageAnimCoroutine == null)
            damageAnimCoroutine = StartCoroutine(ProcessDamageAnimation());
    }

    private IEnumerator ProcessDamageAnimation()
    {
        until = new WaitUntil(() => { return animationComplete; });

        while (damageQueue.Count > 0)
        {
            int currentDamage = damageQueue.Dequeue();
            float amount = unitSc.HealthSystem.CalculatePercentage(currentDamage);

            float currentPercent = hpBar.fillAmount;
            float endPercent = currentPercent - amount;
            animationComplete = false;

            healthBarSequence = DOTween.Sequence();
            healthBarSequence.Append(
                DOTween.To(
                        () => currentPercent,
                        x => { currentPercent = x; hpBar.fillAmount = currentPercent; },
                        endPercent,
                        damageAnimationDuration
                    )
                    .SetEase(Ease.OutCubic)
                    .OnComplete(() => animationComplete = true)
            );

            yield return until;
        }

        damageAnimCoroutine = null;
    }

    private void OnDisable()
    {
        healthBarSequence?.Kill();
    }
}
