using System.Collections.Generic;
using UnityEngine;

public class GameIndicator : MonoBehaviour
{
    private IndicatorPool playerPool;
    private IndicatorPool enemyPool;
    private IndicatorPool tutorialPool;

    public void Awake()
    {
        playerPool = new IndicatorPool();
        enemyPool = new IndicatorPool();
        tutorialPool = new IndicatorPool();
    }

    public T Get<T>(IndicatorOption option = IndicatorOption.Multiple) where T : IndicatorComponent
    {
        return playerPool.TryGet<T>(option);
    }

    public void Show<T>(Vector2 coord, IndicatorOption option = IndicatorOption.Multiple) where T : IndicatorComponent
    {
        Get<T>(option).Show(coord);
    }

    public void Show<T>(Vector2 start, Vector2 end, IndicatorOption option = IndicatorOption.Multiple) where T : IndicatorConnect
    {
        Get<T>(option).Show(start, end);
    }

    // 직선 표시는 다중
    public void ShowStraight(Vector2 start, Vector2 end, IndicatorStraightOption straightOpt, IndicatorOption option = IndicatorOption.Multiple)
    {
        Get<IndicatorStraightConnect>(option).Show(start, end, straightOpt);
    }

    // 아이콘 다중 고정
    public void ShowIcon(Vector3 position, Vector3 direction, IndicatorOption option = IndicatorOption.Multiple)
    {
        Get<IndicatorIcon>(option).Show(position, direction);
    }

    public void Hide<T>(IndicatorOption option = IndicatorOption.Multiple) where T : IndicatorComponent
    {
        playerPool.Hide<T>(option);
    }

    public void ShowStageCell(int colorOpt, Vector2 center, int range)
    {
        HashSet<Vector2> area = StageManager.Instance.ShowStageCells(colorOpt, center, range);
        
        Hide<IndicatorIcon>();
        ShowCoverPoint(ref area);
    }

    void ShowCoverPoint(ref HashSet<Vector2> area)
    {
        var coverMaps = StageManager.Instance.coverMaps;
        foreach (Vector2 coord in area)
        {
            if (coverMaps.TryGetValue(coord, out List<CoverData> datas))
            {
                foreach (var data in datas)
                    ShowIcon(data.position, data.direction);
            }
        }
    }

    public void HideStageCell()
    {
        StageManager.Instance.HideStageCells();
    }

    public void HideAll()
    {
        playerPool.HideAll();
        StageManager.Instance.HideStageCells();
    }


    public void ShowSelectedPlayer(Vector2 coord)
    {
        Show<IndicatorSelectedPlayer>(coord, IndicatorOption.Unique);
    }

    public void ShowMovementIndicator(Unit unit, Vector2 coord)
    {
        Get<IndicatorCursor>(IndicatorOption.Unique).Show(unit, coord);
        Show<IndicatorPath>(unit.curCoord, coord, IndicatorOption.Unique);

        Hide<IndicatorStraightConnect>();
        Hide<IndicatorArcConnect>();

        foreach (var enemy in GameUnitManager.Instance.Units[GameUnitManager.Enemy])
        {
            int coverBonus = enemy.CoverSystem.GetCoverBonus(enemy.curCoord, coord);
            ShowStraight(coord, enemy.curCoord, coverBonus > 0 ? IndicatorStraightOption.Normal : IndicatorStraightOption.Yellow);

            if ((enemy as EnemyUnit).IsInAttackRange(coord))
                Show<IndicatorArcConnect>(enemy.curCoord, coord);
        }
    }

    public void HideMovementIndicator()
    {
        Hide<IndicatorCursor>(IndicatorOption.Unique);
        Hide<IndicatorPath>(IndicatorOption.Unique);
        Hide<IndicatorStraightConnect>();
        Hide<IndicatorArcConnect>();
    }

    public void ShowAttackIndicator(Vector2 start, Vector2 dest)
    {
        // 공격 지시선 (유일)
        Show<IndicatorAttackConnect>(start, dest, IndicatorOption.Unique);
    }

    public void ShowAttackEstimate(Unit subject, Vector2 attackPoint, ref List<Unit> targets)
    {
        Hide<IndicatorSelectedEnemy>(); // 공격 대상 표시 (다수)
        Hide<IndicatorSkillEstimate>(); // 공격 평가 표시 (다수)

        for (int i = 0; i < targets.Count; i++)
        {
            IndicatorSkillEstimate estimate = Get<IndicatorSkillEstimate>();
            if (!subject.type.Equals(targets[i].type))
            {
                int coverBonus = targets[i].CoverSystem.GetCoverBonus(targets[i].curCoord, attackPoint);
                estimate.SetInfo(targets[i].StabilitySystem.StabilityBonus, coverBonus, subject.data.UnitBase.Critical);
                estimate.Show(targets[i].Requirement.cameraPoint.transform.position);
                Show<IndicatorSelectedEnemy>(targets[i].curCoord);
            }
            else if (subject.Equals(targets[i]))
            {
                Vector3 pointPos = targets[i].Requirement.cameraPoint.transform.position;
                Vector3 cellPos = StageManager.Instance.cellMaps[attackPoint].transform.position;
                pointPos.x = cellPos.x;
                pointPos.z = cellPos.z;
                estimate.ShowWithoutInfo(pointPos);
            }
            else
            {
                estimate.ShowWithoutInfo(targets[i].Requirement.cameraPoint.transform.position);
            }
            
        }
    }

    public void HideAttackRelatedIndicator()
    {
        Hide<IndicatorAttackConnect>(IndicatorOption.Unique);
        Hide<IndicatorSelectedEnemy>();
        Hide<IndicatorSkillEstimate>();
        Hide<IndicatorRangeCell>();
        Hide<IndicatorDirectionCell>();
    }
    
    
    public T GetInEnemy<T>(IndicatorOption option = IndicatorOption.Multiple) where T : IndicatorComponent
    {
        return enemyPool.TryGet<T>(option);
    }

    public void HideEnemyRelated()
    {
        enemyPool.HideAll();
    }
    public T GetInTutorial<T>(IndicatorOption option = IndicatorOption.Multiple) where T : IndicatorComponent
    {
        return tutorialPool.TryGet<T>(option);
    }
    public void HideTutorialRelated()
    {
        tutorialPool.HideAll();
    }
}