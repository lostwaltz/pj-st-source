using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class UnitInfo : DataModel
{
    /// <summary>
    /// 이름
    /// </summary>
    public string Name;

    /// <summary>
    /// 안정지수
    /// </summary>
    public int Stability;

    /// <summary>
    /// 공격력
    /// </summary>
    public int AttackPower;

    /// <summary>
    /// 방어력
    /// </summary>
    public int DefensivePower;

    /// <summary>
    /// 체력
    /// </summary>
    public int Health;

    /// <summary>
    /// 이동가능 거리
    /// </summary>
    public int StepRange;

    /// <summary>
    /// 치명타
    /// </summary>
    public int Critical;

    /// <summary>
    /// 치명타 배율
    /// </summary>
    public int Multiple;

    /// <summary>
    /// 필요 경험치
    /// </summary>
    public float MaxExp;

    /// <summary>
    /// 최대 레벨
    /// </summary>
    public int MaxLevel;

    /// <summary>
    /// 레벨업당 증가폭
    /// </summary>
    public float AttackScaling;

    /// <summary>
    /// 레벨업당 증가폭
    /// </summary>
    public float DefensiveScaling;

    /// <summary>
    /// 레벨업당 증가폭
    /// </summary>
    public float HealthScaling;

    /// <summary>
    /// 레벨업당 증가폭
    /// </summary>
    public float ExpScaling;

    /// <summary>
    /// 배치비용
    /// </summary>
    public int Cost;

    /// <summary>
    /// 움직임 유형
    /// </summary>
    public int Behaviour;

    /// <summary>
    /// 병과
    /// </summary>
    public ExternalEnums.Speciality SpecialityType;

    /// <summary>
    /// 사용 무기
    /// </summary>
    public ExternalEnums.WeaponType WeaponType;

    /// <summary>
    /// 사용무기 외래키
    /// </summary>
    public int WeaponKey;

    /// <summary>
    /// 보유스킬
    /// </summary>
    public List<int> Skill;

    /// <summary>
    /// 경로
    /// </summary>
    public string Path;

    /// <summary>
    /// 고해상도 Avatar 경로
    /// </summary>
    public string BustPath;

    /// <summary>
    /// Gacha 캐릭터 전신
    /// </summary>
    public string WholePath;

    /// <summary>
    /// Gacha 캐릭터 배너
    /// </summary>
    public string GachaPath;

    /// <summary>
    /// Gacha병과
    /// </summary>
    public string SpecialityPath;

    /// <summary>
    /// 캐릭터병과
    /// </summary>
    public string SpecialityInfo;

    /// <summary>
    /// 캐릭터 등급
    /// </summary>
    public ExternalEnums.Grade Grade;

    /// <summary>
    /// 캐릭터 고유특성
    /// </summary>
    public List<string> GachaSpecialityInfo;

    /// <summary>
    /// 캐릭터 설명
    /// </summary>
    public string UnitDescription;

    /// <summary>
    /// 경로
    /// </summary>
    public string GachaUnitPiece;

}
