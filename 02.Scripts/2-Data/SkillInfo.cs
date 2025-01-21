using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SkillInfo : DataModel
{
    /// <summary>
    /// 이름
    /// </summary>
    public string Name;

    /// <summary>
    /// 스킬 설명
    /// </summary>
    public string Description;

    /// <summary>
    /// 안정지수 데미지
    /// </summary>
    public int StabilityDamage;

    /// <summary>
    /// 계수
    /// </summary>
    public float Power;

    /// <summary>
    /// 범위
    /// </summary>
    public int Range;

    /// <summary>
    /// 탄착범위
    /// </summary>
    public int Scope;

    /// <summary>
    /// 탄착범위가로
    /// </summary>
    public int ScopeWidth;

    /// <summary>
    /// 탄착범위세로
    /// </summary>
    public int ScopeHeight;

    /// <summary>
    /// 코스트
    /// </summary>
    public int Cost;

    /// <summary>
    /// 경로
    /// </summary>
    public string Path;

    /// <summary>
    /// 레벨당 증가 계수
    /// </summary>
    public float PowerScaling;

    /// <summary>
    /// 증가 범위
    /// </summary>
    public int RangeAmount;

    /// <summary>
    /// 범위 증가 간격
    /// </summary>
    public int RangeAmountStep;

    /// <summary>
    /// 레벨 한계
    /// </summary>
    public int MaxLevel;

    /// <summary>
    /// 스킬 타입
    /// </summary>
    public int SelectType;

    /// <summary>
    /// 스킬 대상
    /// </summary>
    public int TargetType;

    /// <summary>
    /// 이펙트 경로
    /// </summary>
    public string EffectPath;

    /// <summary>
    /// 액션 경로
    /// </summary>
    public string ActionPath;

    /// <summary>
    /// 목표 설명
    /// </summary>
    public string TargetDiscription;

    /// <summary>
    /// 스킬 타입
    /// </summary>
    public string SkillType;

    /// <summary>
    /// 스킬 범위 타입
    /// </summary>
    public string SkillRangeType;

    /// <summary>
    /// 스킬 대사
    /// </summary>
    public string Script;

    /// <summary>
    /// 공격 방식
    /// </summary>
    public int AttackMethod;

    /// <summary>
    /// 공격 횟수
    /// </summary>
    public int AttackCount;

    /// <summary>
    /// 공격 당 타격 횟수
    /// </summary>
    public int HitPerAttack;

}
