using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnumTypes;
using UnityEngine;

public class ActiveSkill : Skill, ICommandReceiver
{
    public SelectorSO Selector;
    private readonly int index = 0;
    
    public int Option { get; set; }
    public Vector2 CommandCoord { get; set; }
    public ReceiverStep ReceiverStep { get; set; }

    private Vector2 attackPoint;
    public List<Unit> Targets = new();
    public List<int> Damages = new();
    
    public ActiveSkill(int idx, Unit owner, SkillInstance inst) : base(owner, inst)
    {
        Type = SkillType.Active;
        index = idx;
        SetSelect((SkillSelectType)Data.SkillBase.SelectType);
        
        EffectHandler.OnHitActivated += ActivateAction;
    }
    
    void ActivateAction(EffectContext context)
    {
        if(0 < context.AttackCount)
            CalculateDamages(context);
        
        ActionHandler.ActivateSkillActions(this, context.MultiTarget, context.CalculatedDamages);
    }

    void CalculateDamages(EffectContext context)
    {
        // 휴먼 에러 유의
        // TODO : 반드시 Attack Count만큼 호출할 것을 보장해야함
        for (int i = 0; i < context.Damages.Count; i++)
        {
            int splitDamage = context.Damages[i] / context.AttackCount;
            context.CalculatedDamages[i] = splitDamage;
        }
    }

    public void ActivatePostAction()
    {
        ActionHandler.ActivateSkillPostActions(this);
    }

    private void SetSelect(SkillSelectType type)
    {
        Selector = Core.DataManager.SkillSelectorList.GetSelector(type);
    }
    
    public virtual IEnumerator Activate(List<Unit> targets, List<int> damages)
    {
        // CommandCoord
        if (targets.Count > 0)
        {
            Vector3 attackPos = targets.Count == 1 ? targets.First().transform.position : StageManager.Instance.cellMaps[CommandCoord].transform.position;
            OwnerUnit.transform.rotation = Quaternion.LookRotation(attackPos - OwnerUnit.transform.position);   
        }

        var targetDestTransforms = targets.Count != 0 ? new List<Transform>() : null;

        foreach (var unit in targets)
            targetDestTransforms?.Add(unit.transform);
        
        EffectContext = new EffectContext(
            null, null,targetDestTransforms?.First(), targets, damages,
            Data.SkillBase.AttackCount, Data.SkillBase.HitPerAttack, Data.SkillBase.AttackMethod);
        
        yield return WaitForSeconds;
        // ActionHandler.ActivateSkillActions(this, targets, damages);
        
        float clipSec = OwnerUnit.Animation.Animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(clipSec - 0.5f);
    }


    public bool Ready(int opt, bool isReverted = false)
    {
        if (OwnerUnit.GaugeSystem.GetGauge<ReMolding>().Value < Data.Cost)
        {
            // 사용 불가 : 리몰딩 부족
            Debug.Log($"리몰딩 부족!");
            return false;
        }
        
        Option = opt;
        ReceiverStep = ReceiverStep.Ready;
        
        Targets.Clear();
        Damages.Clear();
        
        // 인디케이터 표시
        StageManager.Instance.ChangeCellColor(opt); // skill에서 들어온 opt는 useSkill임

        attackPoint = 
            OwnerUnit.CommandSystem.commandCoord.ContainsKey(opt - 1) ?
            OwnerUnit.CommandSystem.commandCoord[opt - 1] : OwnerUnit.curCoord;
        
        Selector.ShowSelectionRange(this, attackPoint);
        
        GameManager.Instance.Interaction.UpdateLayerMask(Selector.layerMask);
        
        return true;
    }

    public void Interact()
    {
        // 클릭 위임받아 처리
        if (GameManager.Instance.Interaction.Click(out RaycastHit hit) && 
            Selector.Interact(hit, ((UnitType)Data.SkillBase.TargetType) == OwnerUnit.type ? 
                OwnerUnit.type : 
                OwnerUnit.type == UnitType.PlayableUnit ? UnitType.EnemyUnit : UnitType.PlayableUnit,
                out Vector2 targetCoord))
        {
            Targets.Clear();
            Damages.Clear();
            
            // 사운드 호출
            UIBattle.PlayUIBattleClickVirtualAttack();

            // 선택 진행
            Selector.Select(this, ref attackPoint, ref targetCoord, ref Targets);
            if(Targets.Count == 0)
                return;
            
            CommandCoord = targetCoord;
            
            // 선택에 맞는 대상 전달하기 : UI
            if (hit.collider.TryGetComponent(out IClickable clickable))
                GameManager.Instance.Mediator.CallSkillInteracted(clickable);
            
            // 데미지 계산
            DamageCalculator.CalculateDamage(OwnerUnit, index, attackPoint, ref Targets, out Damages);
            
            // 명령 업데이트
            OwnerUnit.CommandSystem.UpdateCommand(Option, CommandCoord, CreateCommand);
            
            // 리시버 상태 업데이트
            ReceiverStep = ReceiverStep.Determine;
        }
        else
        {
            // TODO : UI 표시 - 잘못된 대상 선택
            Debug.Log("잘못된 대상 선택");
        }
    }

    public void Revert()
    {
        if (ReceiverStep == ReceiverStep.Ready)
        {
            OwnerUnit.CommandSystem.ChangeReceiver(OwnerUnit.Movement, PlayerCommandType.Move, 0, true);
            return;
        }
        
        Selector.HideSelectionRange();
        Selector.ShowSelectionRange(this, attackPoint);
        ReceiverStep = ReceiverStep.Ready;
    }
    public void Clear()
    {
        GameManager.Instance.Interaction.RestoreLayerMask();
        Selector.HideSelectionRange();
    }

    public IUnitCommand CreateCommand()
    {
        return new AttackCommand(OwnerUnit, index, Targets, Damages);
    }
}