using System;
using System.Diagnostics;
using EnumTypes;
using UnityEngine;

[RequireComponent(typeof(UnitAnimation))]
[RequireComponent(typeof(UnitStatsSystem))]
[RequireComponent(typeof(UnitHealthSystem))]
[RequireComponent(typeof(UnitSkillSystem))]
[RequireComponent(typeof(UnitMovement))]
[RequireComponent(typeof(UnitCoverSystem))]
[RequireComponent(typeof(UnitStabilitySystem))]
[RequireComponent(typeof(UnitAnimationEventHandler))]
[RequireComponent(typeof(UnitCommandSystem))]
public abstract class Unit : MonoBehaviour, IClickable
{
    // 유닛 데이터
    public UnitInstance data;

    public int index;
    public Vector2 curCoord;
    public UnitType type;

    private const string MESH_PATH = Constants.Path.Units + "Mesh/";
    private const string ANIMATOR_PATH = "Animations/Controllers/";
    private const string AVATAR_PATH = "Animations/Avatars/";
    private GameObject uiObject;

    public event Action OnReleaseUnit;

    [field: SerializeField] public UnitStatsSystem StatsSystem { get; protected set; }
    [field: SerializeField] public UnitHealthSystem HealthSystem { get; protected set; }
    [field: SerializeField] public UnitGaugeSystem GaugeSystem { get; protected set; }
    [field: SerializeField] public UnitStabilitySystem StabilitySystem { get; protected set; }
    [field: SerializeField] public UnitShieldSystem ShieldSystem { get; protected set; }

    [field: SerializeField] public UnitCommandSystem CommandSystem { get; protected set; }
    [field: SerializeField] public UnitSkillSystem SkillSystem { get; protected set; }

    [field: SerializeField] public UnitMovement Movement { get; protected set; }
    [field: SerializeField] public UnitCoverSystem CoverSystem { get; protected set; }
    [field: SerializeField] public UnitAnimation Animation { get; protected set; }
    [field: SerializeField] public UnitAnimationEventHandler AnimationEventHandler { get; protected set; }

    [field: SerializeField] public UnitRequirement Requirement { get; protected set; }

#if UNITY_EDITOR
    private void OnValidate()
    {
        Animation = GetComponent<UnitAnimation>();
        StatsSystem = GetComponent<UnitStatsSystem>();
        HealthSystem = GetComponent<UnitHealthSystem>();
        SkillSystem = GetComponent<UnitSkillSystem>();
        Movement = GetComponent<UnitMovement>();
        GaugeSystem = GetComponent<UnitGaugeSystem>();
        StabilitySystem = GetComponent<UnitStabilitySystem>();
        ShieldSystem = GetComponent<UnitShieldSystem>();
        CoverSystem = GetComponent<UnitCoverSystem>();
        AnimationEventHandler = GetComponent<UnitAnimationEventHandler>();
        CommandSystem = GetComponent<UnitCommandSystem>();
    }
#endif
    private void Awake()
    {
        if (!Animation) Animation = GetComponent<UnitAnimation>();
        if (!StatsSystem) StatsSystem = GetComponent<UnitStatsSystem>();
        if (!HealthSystem) HealthSystem = GetComponent<UnitHealthSystem>();
        if (!SkillSystem) SkillSystem = GetComponent<UnitSkillSystem>();
        if (!Movement) Movement = GetComponent<UnitMovement>();
        if (!GaugeSystem) GaugeSystem = GetComponent<UnitGaugeSystem>();
        if (!StabilitySystem) StabilitySystem = GetComponent<UnitStabilitySystem>();
        if (!ShieldSystem) ShieldSystem = GetComponent<UnitShieldSystem>();
        if (!CoverSystem) CoverSystem = GetComponent<UnitCoverSystem>();
        if (!AnimationEventHandler) AnimationEventHandler = GetComponent<UnitAnimationEventHandler>();
        if (!CommandSystem) CommandSystem = GetComponent<UnitCommandSystem>();
    }

    public virtual void Init(int unitIndex, Vector2 initCoord, Vector3 initRotate, UnitInstance instance)
    {
        index = unitIndex;
        curCoord = initCoord;
        data = instance;

        gameObject.SetActive(true);
        transform.position = StageManager.Instance.cellMaps[curCoord].transform.position;
        transform.eulerAngles = initRotate;

        StageManager.Instance.cellMaps[curCoord].UnitEnter(this);

        InitMesh(data.UnitBase.Key);

        CommandSystem.Initialize(this);

        Animation.Initialize();
        StatsSystem.InitStats(data);
        GaugeSystem.InitGauge<ReMolding>(6, 6);

        HealthSystem.Initialize();
        HealthSystem.OnDamageTaken += damage => Animation.ReactHit();
        HealthSystem.OnDead += OnCharacterDead;

        SkillSystem.Initialize(this, data.SkillList);
        Movement.Initialize(this);

        StabilitySystem.Initialize(this);
        ShieldSystem.Initialize(this);
        CoverSystem.Initialize(this);
    }

    void InitMesh(int characterId)
    {
        GameObject meshPrefab = Resources.Load<GameObject>($"{MESH_PATH}{characterId}_Mesh");
        GameObject meshObject = Instantiate(meshPrefab, transform);

        Animator animator = GetComponent<Animator>();
        animator.applyRootMotion = true;

        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>($"{ANIMATOR_PATH}{characterId}_Controller");
        Avatar avatar = Resources.Load<Avatar>($"{AVATAR_PATH}{characterId}_Avatar");

        animator.runtimeAnimatorController = controller;
        animator.avatar = avatar;

        Requirement = meshObject.GetComponent<UnitRequirement>();
    }

    protected void OnCharacterDead()
    {
        float dieAnimationLength = Animation.Data.DieAnimationLength;

        Invoke("DelayedOff", dieAnimationLength + 1f);

        if (data.UnitBase.Behaviour == (int)BehaviourType.Boss)
        {
            GameManager.Instance.Indicator.HideEnemyRelated();
        }

        Animation.SetDead();
        GameUnitManager.Instance.RemoveUnit(type, this);
        Core.EventManager.Publish(new UnitOnDeathEvent(type));

        if (type == UnitType.EnemyUnit)
        {
            Core.EventManager.Publish(new AchievementEvent(ExternalEnums.AchActionType.Kill,
                ExternalEnums.AchTargetType.Monster, 1, data.UnitBase.Key));
        }

        StageManager.Instance.cellMaps[curCoord].UnitExit();
    }

    void DelayedOff()
    {
        // Destroy(gameObject);
        gameObject.SetActive(false);
    }

    public virtual string GetInfo()
    {
        // TODO : Undo 기능 구현에 쓸 unit의 저장 정보 Json으로 만들어줄 메서드
        return "";
    }

    public bool IsInAttackRange(Vector2 targetCoord, Vector2? updateCoord = null)
    {
        bool result = false;
        int range = SkillSystem.ActiveSkills[0].Data.SkillRange;
        updateCoord ??= curCoord;

        StageManager.Interaction.FloodFill(updateCoord.Value, range, StageManager.Interaction.AddCostForSkill,
            (coord) =>
            {
                float magnitude = (targetCoord - coord).sqrMagnitude;
                if (!result && magnitude <= 0.01f)
                    result = true;
            });

        return result;
    }

    public void ReleaseUnit()
    {
        Destroy(gameObject);

        OnReleaseUnit?.Invoke();

        OnReleaseUnit = null;
    }
}