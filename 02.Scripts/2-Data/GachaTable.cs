using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class GachaTable : DataModel
{
    /// <summary>
    /// Name
    /// </summary>
    public string Name;

    /// <summary>
    /// 확률
    /// </summary>
    public int LegendaryRate;

    /// <summary>
    /// 확률
    /// </summary>
    public int EpicRate;

    /// <summary>
    /// 확률
    /// </summary>
    public int RareRate;

    /// <summary>
    /// 확률
    /// </summary>
    public int CommonRate;

    /// <summary>
    /// 캐릭터 리스트
    /// </summary>
    public List<int> Units;

}
