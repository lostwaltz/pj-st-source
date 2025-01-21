using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using TMPro;
using UIUnitStatusInfo;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace UIUnitStatusInfo
{
    [System.Serializable]
    public struct StatusInfoUI
    {
        public TMP_Text Damage;
        public TMP_Text Health;
        public TMP_Text Defence;

        public TMP_Text TotalPower;

        public HorizontalLayoutGroup HorizontalLayoutGroup;
    }

    [System.Serializable]
    public struct WeaponInfoUI
    {
        public Image WeaponImage;
        public TMP_Text WeaponName;
    }

    [System.Serializable]
    public struct LevelInfoUI
    {
        public TMP_Text LevelText;
    }

    [System.Serializable]
    public struct ETCInfoUI
    {
        public TMP_Text UnitNameText;
        public TMP_Text SpecialityNameText;
        public TMP_Text RoleText;
    }
}

public class UIUnitStatusDisplay : UIBase
{
    [SerializeField] private CanvasGroup CanvasGroup;

    [SerializeField] private ETCInfoUI etcInfo;
    [SerializeField] private LevelInfoUI levelInfo;
    [SerializeField] private StatusInfoUI statusInfo;
    [SerializeField] private WeaponInfoUI weaponInfo;

    [SerializeField] private Image unitAvatar;
    [SerializeField] private GameObject spineElement;
    [SerializeField] private GameObject spineContainer;
    [SerializeField] private TMP_Text pieceCount;
    [SerializeField] private GameObject btnSpineUp;
    [SerializeField] private GameObject btnLevelUp;

    [SerializeField] private TMP_Text NeedValue;
    [SerializeField] private TMP_Text CurValue;

    [SerializeField] private Image expBar;

    [SerializeField] private List<UISkillSlot> SkillSlotList = new();
    [SerializeField] private float FadeDuration;
    [SerializeField] private UIUnitManageSkillInfo UIUnitManageSkillInfo;

    private Tween tween;
    private readonly List<GameObject> spineAnimations = new();
    private UnitInstance unitInstance;

    private void Start()
    {
        BindEvent(btnSpineUp, _ => unitInstance.SpineLevelUp(OnSpineLevelUp));
        BindEvent(btnLevelUp, _ =>
        {
            UISound.PlayMainClickNormal();
            LevelUpUnit();
        });
    }

    private void LevelUpUnit()
    {
        if (unitInstance.Level >= 10) return;

        var needValue = unitInstance.NeedExpToLevelUp();

        if (!Core.CurrencyManager.GetCurrency<Shard>().Spend(needValue)) return;

        unitInstance.AddMaxExp();

        UISound.PlayLevelUp();

        OnInteractionUnitInfo(new InteractionUIUnitInstance(unitInstance));
    }

    public void OnSpineLevelUp(int level)
    {
        Transform target = spineAnimations[level].transform;

        var toA = target.transform.DOShakePosition(1f, 2f);
        var toB = target.GetComponentInChildren<Image>().DOColor(new Color(223f / 255f, 158f / 255f, 0f / 255f), 1f);

        target.GetComponentInChildren<Image>().color = new Color(223f / 255f, 158f / 255f, 0f / 255f);

        UISound.PlaySpineLevelUp(level + 1);

        pieceCount.text = Utils.Str.Clear().Append(unitInstance.Pieces).Append(" / 30").ToString();

        OnInteractionUnitInfo(new InteractionUIUnitInstance(unitInstance));
    }

    private IEnumerator PlaySpineAnimationWithDelay(float delay)
    {
        foreach (var spineAnimation in spineAnimations)
        {
            UIAnimation ani = spineAnimation.GetComponentInChildren<UIAnimation>();
            spineAnimation.SetActive(true);
            ani.OnTrigger();

            yield return new WaitForSeconds(delay);
        }
    }

    private void PlaySpineAnimation(int spineLevel)
    {
        int index = 0;
        foreach (var spineAnimation in spineAnimations)
        {
            spineAnimation.gameObject.SetActive(false);

            var image = spineAnimation.GetComponentInChildren<Image>();

            image.color = index++ >= spineLevel ? new Color(255f, 255f, 255f, 255f) : new Color(223f / 255f, 158f / 255f, 0f / 255f);
        }

        StartCoroutine(PlaySpineAnimationWithDelay(0.05f));
    }

    protected override void OpenProcedure()
    {
        base.OpenProcedure();

        Core.EventManager?.Subscribe<InteractionUIUnitInstance>(OnInteractionUnitInfo);

        if (spineAnimations.Count >= UnitInstance.MaxSpineLevel) return;

        for (var i = 0; i < UnitInstance.MaxSpineLevel; i++)
            spineAnimations.Add(Instantiate(spineElement, spineContainer.transform));

        LayoutRebuilder.ForceRebuildLayoutImmediate(spineContainer.transform as RectTransform);
    }

    private void OnDisable()
    {
        Core.EventManager?.Unsubscribe<InteractionUIUnitInstance>(OnInteractionUnitInfo);
    }

    private void OnInteractionUnitInfo(InteractionUIUnitInstance instance)
    {
        tween?.Kill();
        CanvasGroup.alpha = 0f;
        tween = CanvasGroup.DOFade(1f, FadeDuration);

        UnitInfo unitBase = instance.UnitInstance.UnitBase;
        DataManager dataManager = Core.DataManager;
        unitInstance = instance.UnitInstance;

        // For Skill
        var index = 0;
        foreach (var skillKey in unitBase.Skill)
        {
            SkillInfo skillInfo = dataManager.SkillTable.GetByKey(skillKey);
            var skillSlot = SkillSlotList[index++];
            skillSlot.SkillIcon.sprite = Resources.Load<Sprite>(skillInfo.Path);

            var button = skillSlot.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => UIUnitManageSkillInfo.ShowSkillInfo(skillInfo));
            }
        }

        etcInfo.UnitNameText.text = instance.UnitInstance.UnitBase.Name;
        etcInfo.SpecialityNameText.text = instance.UnitInstance.UnitBase.SpecialityInfo;

        var specialtyInfoList = new List<string>(instance.UnitInstance.UnitBase.GachaSpecialityInfo);

        Utils.Str.Clear();
        foreach (var info in specialtyInfoList)
        {
            Utils.Str.Append(" / ");
            Utils.Str.Append(info);
        }
        Utils.Str.Remove((int)uint.MinValue, " / ".Length);

        etcInfo.RoleText.text = Utils.Str.ToString();

        // For LevelInfo
        levelInfo.LevelText.text = $"Lv. {instance.UnitInstance.Level} / <color=#747474>{10}</color>";

        int cur = Core.CurrencyManager.GetCurrency<Shard>().Amount;
        int value = unitInstance.NeedExpToLevelUp();

        bool isLack = cur < value;

        CurValue.text = isLack
            ? $"<color=red>{cur}</color>"
            : $"<color=white>{cur}</color>";

        NeedValue.text = value.ToString();


        expBar.fillAmount = unitInstance.GetExpPercentage();

        // For Stat with Animation
        UpdateStatWithAnimation(statusInfo.Damage, int.Parse(statusInfo.Damage.text), instance.UnitInstance.MaxAttackPower, 0.5f);
        UpdateStatWithAnimation(statusInfo.Health, int.Parse(statusInfo.Health.text), instance.UnitInstance.MaxHealth, 0.5f);
        UpdateStatWithAnimation(statusInfo.Defence, int.Parse(statusInfo.Defence.text), instance.UnitInstance.MaxDefensive, 0.5f);
        statusInfo.TotalPower.text = instance.UnitInstance.TotalPower.ToString();

        LayoutRebuilder.ForceRebuildLayoutImmediate(statusInfo.HorizontalLayoutGroup.transform as RectTransform);

        unitAvatar.sprite = Resources.Load<Sprite>(unitBase.GachaUnitPiece);

        WeaponInfo data = dataManager.WeaponTable.GetByKey(unitBase.WeaponKey);

        weaponInfo.WeaponImage.sprite = Resources.Load<Sprite>(data.SpritePath);
        weaponInfo.WeaponImage.transform.localScale = data.WeaponType switch
        {
            ExternalEnums.WeaponType.AssaultRifle or ExternalEnums.WeaponType.SniperRifle or ExternalEnums.WeaponType.Pistol or ExternalEnums.WeaponType.SubMachineGun
                or ExternalEnums.WeaponType.Blade or ExternalEnums.WeaponType.ShotGun or ExternalEnums.WeaponType.Bow => new Vector3(0.35f, 0.35f, 1f),
            ExternalEnums.WeaponType.MachineGun => new Vector3(0.5f, 0.5f, 1f),
            _ => throw new ArgumentOutOfRangeException()
        };

        pieceCount.text = Utils.Str.Clear().Append(instance.UnitInstance.Pieces).Append(" / 30").ToString();

        PlaySpineAnimation(instance.UnitInstance.SpineLevel);

        weaponInfo.WeaponName.text = data.Name;
    }

    private void UpdateStatWithAnimation(TMP_Text textComponent, int oldValue, int newValue, float duration)
    {
        DOVirtual.Float(oldValue, newValue, duration, value =>
        {
            textComponent.text = Mathf.RoundToInt(value).ToString();
        });
    }
}
