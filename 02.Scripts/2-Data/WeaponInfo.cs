using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class WeaponInfo : DataModel
{
    /// <summary>
    /// 이름
    /// </summary>
    public string Name;

    /// <summary>
    /// 설명
    /// </summary>
    public string Description;

    /// <summary>
    /// 희귀도
    /// </summary>
    public ExternalEnums.Grade GradeType;

    /// <summary>
    /// 공격력
    /// </summary>
    public int Attack;

    /// <summary>
    /// 공격력배율
    /// </summary>
    public float Attack_Bonus;

    /// <summary>
    /// 크리티컬확률
    /// </summary>
    public float Critical_Chance;

    /// <summary>
    /// 무기속성
    /// </summary>
    public ExternalEnums.WeaponType WeaponType;

    /// <summary>
    /// 총알속성
    /// </summary>
    public ExternalEnums.AmmoType WeaponAmmoType;

    /// <summary>
    /// 무기 이미지 경로
    /// </summary>
    public string SpritePath;

}
