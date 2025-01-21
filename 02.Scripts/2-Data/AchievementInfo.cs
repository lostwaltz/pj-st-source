using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class AchievementInfo : DataModel
{
    /// <summary>
    /// 업적명
    /// </summary>
    public string Title;

    /// <summary>
    /// 업적 카테고리
    /// </summary>
    public string CategoryTitle;

    /// <summary>
    /// 업적 카테고리 한글 맵핑
    /// </summary>
    public string CategoryTitleKor;

    /// <summary>
    /// 업적 정보
    /// </summary>
    public string Description;

    /// <summary>
    /// 달성 값
    /// </summary>
    public int TargetValue;

    /// <summary>
    /// 업적 행동
    /// </summary>
    public ExternalEnums.AchActionType AchAction;

    /// <summary>
    /// 업적 대상
    /// </summary>
    public ExternalEnums.AchTargetType AchTarget;

    /// <summary>
    /// 업적 달성 조건 외래키
    /// </summary>
    public int ID;

    /// <summary>
    /// 리워드 타입 경로
    /// </summary>
    public string RewardPath;

    /// <summary>
    /// 리워드 개수
    /// </summary>
    public int RewardAmount;

    /// <summary>
    /// 업적 달성 조건
    /// </summary>
    public string ConditionPath;

}
